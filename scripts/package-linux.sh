#!/usr/bin/env bash
set -euo pipefail

if [[ $# -ne 1 || ! "$1" =~ ^[0-9]+\.[0-9]+\.[0-9]+$ ]]; then
    echo "Usage: bash scripts/package-linux.sh <major.minor.patch>" >&2
    exit 64
fi
if [[ "$(uname)" != Linux ]]; then
    echo "Linux packaging must run on Linux." >&2
    exit 1
fi
for tool in dotnet dpkg-deb dpkg-shlibdeps desktop-file-validate; do
    command -v "$tool" >/dev/null || { echo "Missing packaging tool: $tool" >&2; exit 1; }
done
cd "$(dirname "$0")/.."
version="$1"
release_dir="$(pwd)/artifacts/linux-$version"
publish_dir="$release_dir/publish"
staging_dir="$release_dir/deb"
rm -rf "$release_dir"
mkdir -p "$publish_dir" "$staging_dir/DEBIAN" "$staging_dir/usr/lib/openrcs" \
    "$staging_dir/usr/bin" "$staging_dir/usr/share/applications" \
    "$staging_dir/usr/share/pixmaps"
dotnet publish OpenRCS.Ui/OpenRCS.Ui.csproj -c Release -r linux-x64 \
    --self-contained true -p:Version="$version" -p:DebugType=None \
    -p:DebugSymbols=false -o "$publish_dir"
test -x "$publish_dir/OpenRCS.Ui"
cp -a "$publish_dir/." "$staging_dir/usr/lib/openrcs/"
ln -s ../lib/openrcs/OpenRCS.Ui "$staging_dir/usr/bin/openrcs"
cp packaging/linux/openrcs.desktop "$staging_dir/usr/share/applications/"
cp OpenRCS.Ui/Assets/openrcs.png "$staging_dir/usr/share/pixmaps/openrcs.png"
desktop-file-validate "$staging_dir/usr/share/applications/openrcs.desktop"

# Derive minimum versions of linked system libraries from the actual binaries.
# ICU, OpenSSL and X11 are also loaded dynamically, so list them explicitly.
mkdir -p "$release_dir/debian"
printf 'Source: openrcs\nSection: games\nPriority: optional\nMaintainer: OpenRCS contributors <chrismangan@users.noreply.github.com>\n\nPackage: openrcs\nArchitecture: amd64\nDescription: OpenRCS desktop application\n' > "$release_dir/debian/control"
elf_args=()
while IFS= read -r -d '' binary; do
    if [[ "$(file -b "$binary")" == ELF* ]]; then
        elf_args+=("-e$binary")
    fi
done < <(find "$publish_dir" -type f -print0)
linked_dependencies="$(cd "$release_dir" && dpkg-shlibdeps -O "-l$publish_dir" "${elf_args[@]}")"
linked_dependencies="${linked_dependencies#shlibs:Depends=}"
cat > "$staging_dir/DEBIAN/control" <<CONTROL
Package: openrcs
Version: $version
Section: games
Priority: optional
Architecture: amd64
Maintainer: OpenRCS contributors <chrismangan@users.noreply.github.com>
Installed-Size: $(du -sk "$staging_dir/usr" | cut -f1)
Depends: $linked_dependencies, libx11-6, libice6, libsm6, libfontconfig1, libssl3, libicu70 | libicu72 | libicu74 | libicu76 | libicu78, ca-certificates, tzdata
Homepage: https://github.com/chrisjm66/openrcs
Description: OpenRCS desktop application
 OpenRCS desktop simulation application built with Avalonia.
CONTROL
chmod -R go-w "$staging_dir"
chmod 755 "$staging_dir/DEBIAN"
chmod 644 "$staging_dir/DEBIAN/control"
deb_path="artifacts/OpenRCS-$version-linux-x64.deb"
dpkg-deb --root-owner-group --build "$staging_dir" "$deb_path"
dpkg-deb --info "$deb_path"
tar -C "$publish_dir" -czf "artifacts/OpenRCS-$version-linux-x64.tar.gz" .
echo "Created $deb_path and artifacts/OpenRCS-$version-linux-x64.tar.gz"
