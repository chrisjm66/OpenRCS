# Desktop installers

The scripts publish self-contained Native AOT binaries using .NET 10 and package
them with native macOS tools, NSIS, and Debian tools. No packaging subscription
or license key is required. Run each build on the target operating system.
Versions must use numeric `major.minor.patch` format, for example `1.0.0`.
Outputs are written under the ignored `artifacts/` directory.

## macOS

Install the .NET 10 SDK and Xcode command-line tools, then run:

```sh
bash scripts/package-macos.sh 1.0.0
```

This creates `OpenRCS-1.0.0-macos-universal.dmg` (drag the app onto the Applications
shortcut) and `OpenRCS-1.0.0-macos-universal.pkg` (installs into `/Applications`).
The app and every native library are merged and checked for both Intel and Apple
silicon architectures. Signing is ad hoc by default. Set `APPLE_SIGNING_IDENTITY`
to an installed Developer ID Application identity for distribution signing.
The PKG itself is unsigned; Apple notarization is not configured.

## Windows

Install the .NET 10 SDK, PowerShell 7, Visual Studio C++ build tools for Native AOT,
and [NSIS](https://nsis.sourceforge.io/Download). Add `makensis.exe` to PATH or
install NSIS in its default `Program Files (x86)/NSIS` directory. Then run:

```powershell
./scripts/package-windows.ps1 -Version 1.0.0
```

This creates `OpenRCS-1.0.0-win-x64-setup.exe` and a portable ZIP. The installer
requires administrator access, installs under `Program Files/OpenRCS`, creates
Start Menu shortcuts, and registers an uninstaller in Windows installed apps.
Re-running the installer updates the application files. Close OpenRCS before
installing an update. Windows artifacts are unsigned.

## Linux

On Ubuntu 22.04 or newer, install the .NET 10 SDK and the build dependencies:

```sh
sudo apt-get install clang zlib1g-dev libicu-dev libssl-dev dpkg-dev desktop-file-utils
bash scripts/package-linux.sh 1.0.0
sudo apt install ./artifacts/OpenRCS-1.0.0-linux-x64.deb
```

This creates a DEB and a portable tar.gz. The DEB installs binaries under
`/usr/lib/openrcs`, provides the `openrcs` command, and registers a desktop menu
entry and icon. System dependencies are derived from the ELF binaries, with
additional dependencies for dynamically loaded libraries. Uninstall with
`sudo apt remove openrcs`. Build on Ubuntu 22.04 for the release glibc baseline;
building on a newer distribution can increase the minimum supported version.

## GitHub releases

Run **Release desktop apps** manually and enter the version. The workflow builds
on each target OS and uploads all six files to a GitHub release at the selected
workflow commit only after all packaging jobs succeed.

No secrets are required for ad hoc macOS builds. To enable Developer ID app
signing, supply these repository Actions secrets:

- `APPLE_CERTIFICATE_BASE64`: base64-encoded Developer ID Application P12.
- `APPLE_CERTIFICATE_PASSWORD`: password for the P12 (may be empty).
- `APPLE_SIGNING_IDENTITY`: identity matching that certificate.

Public macOS distribution still requires Apple's signing/notarization process
for normal Gatekeeper acceptance; these scripts currently perform application
signing only. The PKG and Windows installer remain unsigned.
