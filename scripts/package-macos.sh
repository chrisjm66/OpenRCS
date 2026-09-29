#!/usr/bin/env bash
set -euo pipefail

if [[ $# -ne 1 ]]; then
    echo "Usage: bash scripts/package-macos.sh <version>" >&2
    exit 64
fi

if [[ "$(uname)" != "Darwin" ]]; then
    echo "macOS packaging must run on macOS." >&2
    exit 1
fi

version="$1"
if [[ ! "$version" =~ ^[0-9A-Za-z._-]+$ ]]; then
    echo "Version may contain only letters, numbers, periods, underscores, and hyphens." >&2
    exit 64
fi

release_dir="artifacts/release-$version"
publish_dir="$release_dir/publish"
app_bundle="$release_dir/OpenRCS.app"
dmg_path="artifacts/OpenRCS-$version-macos-universal.dmg"
project="OpenRCS.Ui/OpenRCS.Ui.csproj"

rm -rf "$release_dir"
mkdir -p "$publish_dir/osx-arm64" "$publish_dir/osx-x64"

dotnet publish "$project" --configuration Release --runtime osx-arm64 --self-contained true --output "$publish_dir/osx-arm64"
dotnet publish "$project" --configuration Release --runtime osx-x64 --self-contained true --output "$publish_dir/osx-x64"

mkdir -p "$app_bundle/Contents/MacOS" "$app_bundle/Contents/Resources"
ditto "$publish_dir/osx-arm64" "$app_bundle/Contents/MacOS"
lipo -create "$publish_dir/osx-arm64/OpenRCS.Ui" "$publish_dir/osx-x64/OpenRCS.Ui" \
    -output "$app_bundle/Contents/MacOS/OpenRCS.Ui"

cp OpenRCS.Ui/Assets/openrcs.icns "$app_bundle/Contents/Resources/OpenRCS.icns"
cp packaging/macos/Info.plist "$app_bundle/Contents/Info.plist"
/usr/libexec/PlistBuddy -c "Set :CFBundleShortVersionString $version" "$app_bundle/Contents/Info.plist"
/usr/libexec/PlistBuddy -c "Set :CFBundleVersion $version" "$app_bundle/Contents/Info.plist"

if [[ -n "${APPLE_SIGNING_IDENTITY:-}" ]]; then
    codesign --force --deep --options runtime --sign "$APPLE_SIGNING_IDENTITY" "$app_bundle"
fi

rm -f "$dmg_path"
hdiutil create -volname OpenRCS -srcfolder "$app_bundle" -ov -format UDZO "$dmg_path"
echo "Created $dmg_path"
