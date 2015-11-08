; PowerBuddy.nsi
;
; This script is based on example2.nsi

;--------------------------------

; Constants
!define ProductName "Power Buddy"
!define CompactProductName "PowerBuddy"
!define ProjectName "PC.PowerBuddy"
!define BinaryFileName "PC.PowerBuddy.exe"
!define CompanyName "Perfectly Cromulent Ltd"

; The name of the installer
Name "${ProductName}"

; Product Details
!getdllversion "..\${ProjectName}\bin\Release\${BinaryFileName}" versionParts_
!define ProductVersion "${versionParts_1}.${versionParts_2}.${versionParts_3}.${versionParts_4}"
VIProductVersion ${ProductVersion}
VIAddVersionKey ProductVersion ${ProductVersion}
VIAddVersionKey FileVersion ${ProductVersion}
VIAddVersionKey FileDescription "Installer for ${ProductName}"
VIAddVersionKey LegalCopyright ""
VIAddVersionKey CompanyName "${CompanyName}"

; The file to write
OutFile "${CompactProductName}Installer.exe"

; The default installation directory
InstallDir "$PROGRAMFILES\${ProductName}"

; Registry key to check for directory (so if you install again, it will 
; overwrite the old one automatically)
InstallDirRegKey HKLM "Software\${CompanyName}\${CompactProductName}" "Install_Dir"

; Request application privileges for Windows Vista
RequestExecutionLevel admin

;--------------------------------

; Pages

Page components
Page directory
Page instfiles

UninstPage uninstConfirm
UninstPage instfiles

;--------------------------------

; The stuff to install
Section "${ProductName} (required)"

  SectionIn RO
  
  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  ; Put file there
  File "..\${ProjectName}\bin\Release\${BinaryFileName}"
  File "..\${ProjectName}\bin\Release\${BinaryFileName}.config"
  File "..\${ProjectName}\bin\Release\Microsoft.Practices.Prism.dll"
  
  ; Write the installation path into the registry
  WriteRegStr HKLM "Software\${CompanyName}\${CompactProductName}" "Install_Dir" "$INSTDIR"
  
  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${CompactProductName}" "DisplayName" "${ProductName}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${CompactProductName}" "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${CompactProductName}" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${CompactProductName}" "NoRepair" 1
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${CompactProductName}" "Publisher" "${CompanyName}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${CompactProductName}" "DisplayVersion" "${ProductVersion}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${CompactProductName}" "DisplayIcon" "$INSTDIR\${BinaryFileName},0"
  WriteUninstaller "uninstall.exe"
  
SectionEnd

; Optional section (can be disabled by the user)
Section "Start Menu Shortcuts"

  CreateDirectory "$SMPROGRAMS\${ProductName}"
  CreateShortCut "$SMPROGRAMS\${ProductName}\Uninstall.lnk" "$INSTDIR\uninstall.exe" "" "$INSTDIR\uninstall.exe" 0
  CreateShortCut "$SMPROGRAMS\${ProductName}\${ProductName}.lnk" "$INSTDIR\${BinaryFileName}" "" "" 0
  
SectionEnd

Section "Desktop Shortcuts"

	CreateShortCut "$DESKTOP\${ProductName}.lnk" "$INSTDIR\${BinaryFileName}" "" "" 0

SectionEnd

;--------------------------------

; Uninstaller

Section "Uninstall"
  
  ; Remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${CompactProductName}"
  DeleteRegKey HKLM "Software\${CompanyName}\${CompactProductName}"

  ; Remove files and uninstaller
  Delete $INSTDIR\${BinaryFileName}
  Delete $INSTDIR\${BinaryFileName}.config
  Delete $INSTDIR\Microsoft.Practices.Prism.dll
  Delete $INSTDIR\uninstall.exe

  ; Remove shortcuts, if any
  Delete "$SMPROGRAMS\${ProductName}\*.*"

  ; Remove directories used
  RMDir "$SMPROGRAMS\${ProductName}"
  RMDir "$INSTDIR"

SectionEnd
