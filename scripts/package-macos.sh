#!/usr/bin/env bash
set -euo pipefail

if [[ $# -ne 1 || ! "$1" =~ ^[0-9]+\.[0-9]+\.[0-9]+$ ]]; then
    echo "Usage: bash scripts/package-macos.sh <major.minor.patch>" >&2
    exit 64
fi
if [[ "$(uname)" != Darwin ]]; then
    echo "macOS packaging must run on macOS." >&2
    exit 1
fi
cd "$(dirname "$0")/.."
version="$1"
release_dir="artifacts/macos-$version"
publish_dir="$release_dir/publish"
app_bundle="$release_dir/OpenRCS.app"
dmg_path="artifacts/OpenRCS-$version-macos-universal.dmg"
pkg_path="artifacts/OpenRCS-$version-macos-universal.pkg"
rm -rf "$release_dir"
mkdir -p "$publish_dir"

for runtime in osx-arm64 osx-x64; do
    dotnet publish OpenRCS.Ui/OpenRCS.Ui.csproj -c Release -r "$runtime" \
        --self-contained true -p:Version="$version" -p:DebugType=None \
        -p:DebugSymbols=false -o "$publish_dir/$runtime"
    find "$publish_dir/$runtime" -type d -name "*.dSYM" -prune -exec rm -rf {} +
done
mkdir -p "$app_bundle/Contents/MacOS" "$app_bundle/Contents/Resources" "$release_dir/slices"
ditto "$publish_dir/osx-arm64" "$app_bundle/Contents/MacOS"

# Merge every native binary, including Avalonia/Skia libraries, not just the app.
while IFS= read -r -d '' arm_file; do
    if [[ "$(file -b "$arm_file")" != *Mach-O* ]]; then
        continue
    fi
    relative="${arm_file#"$publish_dir/osx-arm64/"}"
    intel_file="$publish_dir/osx-x64/$relative"
    if [[ ! -f "$intel_file" ]]; then
        echo "Missing Intel counterpart: $relative" >&2
        exit 1
    fi
    target="$app_bundle/Contents/MacOS/$relative"
    # Some NuGet native libraries are already universal in both publish trees.
    # Extract one architecture from each to avoid duplicate slices in lipo.
    arm_archs="$(lipo -archs "$arm_file")"
    intel_archs="$(lipo -archs "$intel_file")"
    if [[ "$arm_archs" == arm64 ]]; then
        cp "$arm_file" "$release_dir/slices/arm64"
    else
        lipo "$arm_file" -thin arm64 -output "$release_dir/slices/arm64"
    fi
    if [[ "$intel_archs" == x86_64 ]]; then
        cp "$intel_file" "$release_dir/slices/x86_64"
    else
        lipo "$intel_file" -thin x86_64 -output "$release_dir/slices/x86_64"
    fi
    lipo -create "$release_dir/slices/arm64" "$release_dir/slices/x86_64" -output "$target"
    architectures=" $(lipo -archs "$target") "
    if [[ "$architectures" != *" arm64 "* || "$architectures" != *" x86_64 "* ]]; then
        echo "Universal binary validation failed: $relative ($architectures)" >&2
        exit 1
    fi
done < <(find "$publish_dir/osx-arm64" -type f -print0)
# Also reject Intel-only native files that the arm64 staging copy would omit.
while IFS= read -r -d '' intel_file; do
    relative="${intel_file#"$publish_dir/osx-x64/"}"
    if [[ "$(file -b "$intel_file")" == *Mach-O* && ! -f "$publish_dir/osx-arm64/$relative" ]]; then
        echo "Missing Apple silicon counterpart: $relative" >&2
        exit 1
    fi
done < <(find "$publish_dir/osx-x64" -type f -print0)

# Keep data outside Apple's executable directory while preserving app paths.
if [[ -d "$app_bundle/Contents/MacOS/Assets" ]]; then
    mv "$app_bundle/Contents/MacOS/Assets" "$app_bundle/Contents/Resources/Assets"
    ln -s ../Resources/Assets "$app_bundle/Contents/MacOS/Assets"
fi
while IFS= read -r -d '' resource; do
    if [[ "$(file -b "$resource")" != *Mach-O* ]]; then
        relative="${resource#"$app_bundle/Contents/MacOS/"}"
        mkdir -p "$app_bundle/Contents/Resources/$(dirname "$relative")"
        mv "$resource" "$app_bundle/Contents/Resources/$relative"
        # Current publish data files are at the root; fail on an unexpected layout.
        if [[ "$relative" == */* ]]; then
            echo "Unexpected nested resource path: $relative" >&2
            exit 1
        fi
        ln -s "../Resources/$relative" "$resource"
    fi
done < <(find "$app_bundle/Contents/MacOS" -type f -print0)

cp OpenRCS.Ui/Assets/openrcs.icns "$app_bundle/Contents/Resources/OpenRCS.icns"
cp packaging/macos/Info.plist "$app_bundle/Contents/Info.plist"
/usr/libexec/PlistBuddy -c "Set :CFBundleShortVersionString $version" "$app_bundle/Contents/Info.plist"
/usr/libexec/PlistBuddy -c "Set :CFBundleVersion $version" "$app_bundle/Contents/Info.plist"

# Ad hoc signing supports local builds without a Developer ID certificate.
signing_identity="${APPLE_SIGNING_IDENTITY:--}"
sign_options=(--force --sign "$signing_identity")
if [[ "$signing_identity" != - ]]; then
    sign_options+=(--options runtime --timestamp)
fi
while IFS= read -r -d '' binary; do
    if [[ "$(file -b "$binary")" == *Mach-O* ]]; then
        codesign "${sign_options[@]}" "$binary"
    fi
done < <(find "$app_bundle/Contents/MacOS" -type f -print0)
codesign "${sign_options[@]}" "$app_bundle"
codesign --verify --deep --strict "$app_bundle"

mkdir -p "$release_dir/dmg"
ditto "$app_bundle" "$release_dir/dmg/OpenRCS.app"
ln -s /Applications "$release_dir/dmg/Applications"
rm -f "$dmg_path" "$pkg_path"
hdiutil create -volname OpenRCS -srcfolder "$release_dir/dmg" -ov -format UDZO "$dmg_path"
hdiutil verify "$dmg_path"
productbuild --component "$app_bundle" /Applications "$pkg_path"
echo "Created $dmg_path and $pkg_path"
