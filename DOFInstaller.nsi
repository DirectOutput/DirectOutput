; DirectOutput Framework NSIS Installer

!define PRODUCT_NAME "DirectOutput Framework"
!ifndef PRODUCT_VERSION
!define PRODUCT_VERSION "3.9.9"
!endif
!define PRODUCT_PUBLISHER "Swisslizard & the DirectOutput team"
!define PRODUCT_WEB_SITE "https://github.com/DirectOutput/DirectOutput"
!define COPYRIGHTYEAR "2025"
!ifndef FILE_VERSION
!define FILE_VERSION "${PRODUCT_VERSION}.0"
!endif

; Architecture handling
!ifndef ARCH
  !define ARCH "32"
!endif

!if ${ARCH} == "64"
  !define BINDIR "x64"
  !define UPGRADE_CODE "{A7EAB3EB-6524-4173-B5D8-25FC867BD29E}"
!else
  !define BINDIR "x86"
  !define UPGRADE_CODE "{94E0D0EE-C078-42C6-AD9F-4030B329A040}"
!endif

!define PRODUCT_UNINST_ROOT_KEY "HKCU"
!define PRODUCT_SHORT_NAME "DirectOutput${ARCH}"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_SHORT_NAME}"
!define PRODUCT_DIR_REGKEY "SOFTWARE\DirectOutput\DirectOutput"
  
; Include Modern UI
!include "MUI2.nsh"
!include "x64.nsh"
!include "FileFunc.nsh"
!include "LogicLib.nsh"

; Installer attributes
Name "${PRODUCT_NAME} ${ARCH}"
OutFile "Output\DOFSetup-NSIS-${ARCH}bit-Setup.exe"
InstallDir "C:\DirectOutput"
InstallDirRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_DIR_REGKEY}" "InstallPath"
ShowInstDetails show
ShowUnInstDetails show
RequestExecutionLevel user

; Version Info
VIProductVersion "${PRODUCT_VERSION}.0"
VIAddVersionKey "ProductName" "${PRODUCT_NAME}"
VIAddVersionKey "ProductVersion" "${PRODUCT_VERSION}"
VIAddVersionKey "CompanyName" "${PRODUCT_PUBLISHER}"
VIAddVersionKey "FileDescription" "${PRODUCT_NAME}  ${ARCH} Installer"
VIAddVersionKey "FileVersion" "${PRODUCT_VERSION}.0"
VIAddVersionKey "LegalCopyright" "Copyright © ${COPYRIGHTYEAR} ${PRODUCT_PUBLISHER}"

; MUI Settings
!define MUI_ABORTWARNING
!define MUI_ICON "DirectOutput\DirectOutputIcon_32.ico"
!define MUI_UNICON "DirectOutput\DirectOutputIcon_32.ico"
!define MUI_WELCOMEFINISHPAGE_BITMAP "DOFSetup\Res\DialogSideCropped.bmp"
!define MUI_UNWELCOMEFINISHPAGE_BITMAP "DOFSetup\Res\DialogSideCropped.bmp"

; Pages
!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_LICENSE "LICENSE"
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH

; Uninstaller pages
!insertmacro MUI_UNPAGE_WELCOME
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_UNPAGE_FINISH

; Languages
!insertmacro MUI_LANGUAGE "English"

; Functions
Function .onInit
!if ${ARCH} == "64"
  ; Check if running on 64-bit Windows
  ${IfNot} ${RunningX64}
    MessageBox MB_OK|MB_ICONSTOP "This installer requires a 64-bit version of Windows."
    Abort
  ${EndIf}
!endif

  ; Check for existing installation of same architecture
  ReadRegStr $R1 HKLM "${PRODUCT_UNINST_KEY}" "UpgradeCode"
  StrCmp $R1 "${UPGRADE_CODE}" same_version
  
  ; Check for existing installation (legacy method)
  ReadRegStr $R0 HKLM "${PRODUCT_UNINST_KEY}" "UninstallString"
  StrCmp $R0 "" done
  
  MessageBox MB_OKCANCEL|MB_ICONEXCLAMATION \
    "${PRODUCT_NAME} (${ARCH}) is already installed. $\n$\nClick OK to remove the previous version or Cancel to cancel this upgrade." \
    IDOK uninst
  Abort

same_version:
  ; Same upgrade code found - this is an upgrade/reinstall
  ReadRegStr $R0 HKLM "${PRODUCT_UNINST_KEY}" "UninstallString"
  MessageBox MB_OKCANCEL|MB_ICONQUESTION \
    "${PRODUCT_NAME} ${PRODUCT_VERSION} (${ARCH}) is already installed. $\n$\nClick OK to reinstall or Cancel to exit." \
    IDOK uninst
  Abort

uninst:
  ClearErrors
  ExecWait '$R0 /S _?=$INSTDIR'
  
  IfErrors no_remove_uninstaller done
    Delete $R0
    RMDir $INSTDIR

no_remove_uninstaller:
done:
FunctionEnd

Section "Main Installation" SEC01
  SetOutPath "$INSTDIR"
  SetOverwrite try

  ; Install documentation and config files
  File "bin\${BINDIR}\Release\DirectOutputShapes.png"
  File "bin\${BINDIR}\Release\DirectOutputShapes.xml"
  File "LICENSE"

  CreateDirectory "$INSTDIR\${BINDIR}"
  SetOutPath "$INSTDIR\${BINDIR}"

  ; Install main files
  File "bin\${BINDIR}\Release\DirectOutput.dll"
  File "bin\${BINDIR}\Release\B2SServerDirectOutputPlugin.dll"
  File "bin\${BINDIR}\Release\DirectOutput PinballX Plugin.dll"
  File "bin\${BINDIR}\Release\DirectOutputComObject.dll"
  File "bin\${BINDIR}\Release\DirectOutputComObject.tlb"
  File "bin\${BINDIR}\Release\RegisterDirectOutputComObject.exe"
  File "bin\${BINDIR}\Release\DirectOutputConfigTester.exe"
  File "bin\${BINDIR}\Release\GlobalConfigEditor.exe"
  File "bin\${BINDIR}\Release\LedControlFileTester.exe"
  File "bin\${BINDIR}\Release\DOFSlave.exe"

  File "bin\${BINDIR}\Release\FixupRunner.exe"
  File "bin\${BINDIR}\Release\DOFSetupB2SFixup.dll"
  File "bin\${BINDIR}\Release\DOFSetupPBXFixup.dll"
  File "bin\${BINDIR}\Release\System.Reflection.Metadata.dll"
  File "bin\${BINDIR}\Release\System.Collections.Immutable.dll"

  ; Create config directory
  CreateDirectory "$INSTDIR\Config\Examples"
  SetOutPath "$INSTDIR\Config\Examples"
  File "config\examples\Cabinet.xml"
  File "config\examples\GlobalConfig_B2SServer.xml"

  DetailPrint "Running integration fixups via FixupRunner..."
  ExecWait '"$INSTDIR\${BINDIR}\FixupRunner.exe" "$INSTDIR" "${BINDIR}" "${ARCH}"' $R0
  ${If} $R0 != 0
    MessageBox MB_OK|MB_ICONEXCLAMATION "DirectOutput fixups returned exit code $R0. Review log files in $INSTDIR for details."
  ${EndIf}

  Delete "$INSTDIR\${BINDIR}\FixupRunner.exe"
  Delete "$INSTDIR\${BINDIR}\DOFSetupB2SFixup.dll"
  Delete "$INSTDIR\${BINDIR}\DOFSetupPBXFixup.dll"
  Delete "$INSTDIR\${BINDIR}\System.Reflection.Metadata.dll"
  Delete "$INSTDIR\${BINDIR}\System.Collections.Immutable.dll"

SectionEnd

Section -AdditionalIcons
  WriteIniStr "$INSTDIR\${PRODUCT_NAME}.url" "InternetShortcut" "URL" "${PRODUCT_WEB_SITE}"
SectionEnd

Section -Post
  WriteUninstaller "$INSTDIR\uninst${ARCH}.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_DIR_REGKEY}" "InstallPath" "$INSTDIR\"

  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" '"$INSTDIR\uninst${ARCH}.exe"'

  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "QuietUninstallString" '"$INSTDIR\uninst${ARCH}.exe" /S'

  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "InstallLocation" "$INSTDIR"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\uninst${ARCH}.exe,0"
  
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"

  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UpgradeCode" "${UPGRADE_CODE}"
SectionEnd

Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) was successfully removed from your computer."
FunctionEnd

Function un.onInit
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "Are you sure you want to completely remove $(^Name) and all of its components?" IDYES +2
  Abort
FunctionEnd

Section Uninstall
  ;Delete "$INSTDIR\${PRODUCT_NAME}.url"
  Delete "$INSTDIR\uninst${ARCH}.exe"
  
  ; Remove files
  Delete "$INSTDIR\${BINDIR}\B2SServerDirectOutputPlugin.dll"
  Delete "$INSTDIR\${BINDIR}\DirectOutput PinballX Plugin.dll"
  Delete "$INSTDIR\${BINDIR}\DirectOutput.dll"
  Delete "$INSTDIR\${BINDIR}\DirectOutputComObject.dll"
  Delete "$INSTDIR\${BINDIR}\DirectOutputComObject.tlb"
  Delete "$INSTDIR\${BINDIR}\DirectOutputConfigTester.exe"
  Delete "$INSTDIR\${BINDIR}\DOFSlave.exe"
  Delete "$INSTDIR\${BINDIR}\GlobalConfigEditor.exe"
  Delete "$INSTDIR\${BINDIR}\LedControlFileTester.exe"
  Delete "$INSTDIR\${BINDIR}\RegisterDirectOutputComObject.exe"
  Delete "$INSTDIR\DirectOutputShapes.png"
  Delete "$INSTDIR\DirectOutputShapes.xml"
  Delete "$INSTDIR\LICENSE"
  Delete "$INSTDIR\${PRODUCT_NAME}.url"
  
  ; Remove directories
  RMDir /r "$INSTDIR\Config\Examples"
  RMDir "$INSTDIR\Config"
  RMDir "$INSTDIR\${BINDIR}"
  RMDir "$INSTDIR"
  
  ; Remove shortcuts
  ;Delete "$SMPROGRAMS\${PRODUCT_NAME} (${ARCH}-bit)\Uninstall.lnk"
  ;Delete "$SMPROGRAMS\${PRODUCT_NAME} (${ARCH}-bit)\Website.lnk"
  ;RMDir "$SMPROGRAMS\${PRODUCT_NAME} (${ARCH}-bit)"
  
  ; Remove registry keys
  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_DIR_REGKEY}"
  SetAutoClose true
SectionEnd