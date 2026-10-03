param(
    [Parameter(Mandatory = $true)]
    [ValidatePattern('^[0-9]+\.[0-9]+\.[0-9]+$')]
    [string]$Version
)
$ErrorActionPreference = 'Stop'
if (-not $IsWindows) { throw 'Windows packaging must run on Windows using PowerShell 7.' }
Set-Location (Join-Path $PSScriptRoot '..')
$releaseDirectory = Join-Path $PWD "artifacts/windows-$Version"
$publishDirectory = Join-Path $releaseDirectory 'publish'
$installerPath = Join-Path $PWD "artifacts/OpenRCS-$Version-win-x64-setup.exe"
$zipPath = Join-Path $PWD "artifacts/OpenRCS-$Version-win-x64.zip"
$compiler = Get-Command makensis.exe -ErrorAction SilentlyContinue
if ($compiler) {
    $nsis = $compiler.Source
} else {
    $nsis = Join-Path ${env:ProgramFiles(x86)} 'NSIS/makensis.exe'
    if (-not (Test-Path $nsis)) { throw 'Install NSIS and add makensis.exe to PATH (or install under Program Files (x86)/NSIS).' }
}
if (Test-Path $releaseDirectory) { Remove-Item $releaseDirectory -Recurse -Force }
New-Item $publishDirectory -ItemType Directory -Force | Out-Null
& dotnet publish OpenRCS.Ui/OpenRCS.Ui.csproj -c Release -r win-x64 --self-contained true "-p:Version=$Version" -p:DebugType=None -p:DebugSymbols=false -o $publishDirectory
if ($LASTEXITCODE -ne 0) { throw 'dotnet publish failed.' }
if (-not (Test-Path "$publishDirectory/OpenRCS.Ui.exe")) { throw 'Published executable is missing.' }
& $nsis /WX /V3 "/DAPP_VERSION=$Version" "/DPUBLISH_DIR=$publishDirectory" "/DOUTPUT_FILE=$installerPath" packaging/windows/OpenRCS.nsi
if ($LASTEXITCODE -ne 0) { throw 'NSIS installer compilation failed.' }
if (-not (Test-Path $installerPath)) { throw 'Installer output is missing.' }
Compress-Archive -Path "$publishDirectory/*" -DestinationPath $zipPath -Force
Write-Host "Created $installerPath and $zipPath"
