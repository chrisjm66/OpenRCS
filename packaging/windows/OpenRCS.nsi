Unicode true
!include "MUI2.nsh"
!include "x64.nsh"

!ifndef APP_VERSION
  !error "APP_VERSION is required"
!endif
!ifndef PUBLISH_DIR
  !error "PUBLISH_DIR is required"
!endif
!ifndef OUTPUT_FILE
  !error "OUTPUT_FILE is required"
!endif

Name "OpenRCS"
OutFile "${OUTPUT_FILE}"
InstallDir "$PROGRAMFILES64\OpenRCS"
RequestExecutionLevel admin
SetCompressor /SOLID lzma
VIProductVersion "${APP_VERSION}.0"
VIAddVersionKey "ProductName" "OpenRCS"
VIAddVersionKey "FileDescription" "OpenRCS Installer"
VIAddVersionKey "FileVersion" "${APP_VERSION}"
VIAddVersionKey "LegalCopyright" "OpenRCS contributors"
!define MUI_ICON "..\..\OpenRCS.Ui\Assets\openrcs.ico"
!define MUI_UNICON "..\..\OpenRCS.Ui\Assets\openrcs.ico"
!define MUI_ABORTWARNING
!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_LANGUAGE "English"

Function .onInit
  ${IfNot} ${RunningX64}
    MessageBox MB_ICONSTOP "OpenRCS requires 64-bit Windows."
    Abort
  ${EndIf}
  SetRegView 64
  SetShellVarContext all
FunctionEnd

Section "OpenRCS"
  SetOutPath "$INSTDIR"
  SetOverwrite on
  ClearErrors
  File /r "${PUBLISH_DIR}\*"
  ${If} ${Errors}
    MessageBox MB_ICONSTOP "Could not install OpenRCS. Close the application and run this installer again."
    Abort
  ${EndIf}
  WriteUninstaller "$INSTDIR\Uninstall.exe"
  CreateDirectory "$SMPROGRAMS\OpenRCS"
  CreateShortcut "$SMPROGRAMS\OpenRCS\OpenRCS.lnk" "$INSTDIR\OpenRCS.Ui.exe"
  CreateShortcut "$SMPROGRAMS\OpenRCS\Uninstall.lnk" "$INSTDIR\Uninstall.exe"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\OpenRCS" "DisplayName" "OpenRCS"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\OpenRCS" "DisplayVersion" "${APP_VERSION}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\OpenRCS" "Publisher" "OpenRCS"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\OpenRCS" "InstallLocation" "$INSTDIR"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\OpenRCS" "DisplayIcon" "$INSTDIR\OpenRCS.Ui.exe"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\OpenRCS" "UninstallString" '"$INSTDIR\Uninstall.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\OpenRCS" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\OpenRCS" "NoRepair" 1
SectionEnd

Function un.onInit
  SetRegView 64
  SetShellVarContext all
FunctionEnd

Section "Uninstall"
  Delete "$SMPROGRAMS\OpenRCS\OpenRCS.lnk"
  Delete "$SMPROGRAMS\OpenRCS\Uninstall.lnk"
  RMDir "$SMPROGRAMS\OpenRCS"
  RMDir /r "$INSTDIR"
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\OpenRCS"
SectionEnd
