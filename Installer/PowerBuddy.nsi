; PowerBuddy.nsi
;
; This script is based on example2.nsi

;--------------------------------

; The name of the installer
Name "Power Buddy"

; The file to write
OutFile "PowerBuddyInstaller.exe"

; The default installation directory
InstallDir "$PROGRAMFILES\Power Buddy"

; Registry key to check for directory (so if you install again, it will 
; overwrite the old one automatically)
InstallDirRegKey HKLM "Software\MetaFight\PowerBuddy" "Install_Dir"

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
Section "Power Buddy (required)"

  SectionIn RO
  
  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  ; Put file there
  File "..\PC.PowerBuddy\bin\Release\PC.PowerBuddy.exe"
  File "..\PC.PowerBuddy\bin\Release\PC.PowerBuddy.exe.config"
  File "..\PC.PowerBuddy\bin\Release\Microsoft.Practices.Prism.dll"
  
  ; Write the installation path into the registry
  WriteRegStr HKLM "Software\MetaFight\PowerBuddy" "Install_Dir" "$INSTDIR"
  
  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\PowerBuddy" "DisplayName" "Power Buddy"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\PowerBuddy" "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\PowerBuddy" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\PowerBuddy" "NoRepair" 1
  WriteUninstaller "uninstall.exe"
  
SectionEnd

; Optional section (can be disabled by the user)
Section "Start Menu Shortcuts"

  CreateDirectory "$SMPROGRAMS\Power Buddy"
  CreateShortCut "$SMPROGRAMS\Power Buddy\Uninstall.lnk" "$INSTDIR\uninstall.exe" "" "$INSTDIR\uninstall.exe" 0
  CreateShortCut "$SMPROGRAMS\Power Buddy\Power Buddy.lnk" "$INSTDIR\PC.PowerBuddy.exe" "" "" 0
  
SectionEnd

Section "Desktop Shortcuts"

	CreateShortCut "$DESKTOP\Power Buddy.lnk" "$INSTDIR\PC.PowerBuddy.exe" "" "" 0

SectionEnd

;--------------------------------

; Uninstaller

Section "Uninstall"
  
  ; Remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\PowerBuddy"
  DeleteRegKey HKLM "Software\MetaFight\PowerBuddy"

  ; Remove files and uninstaller
  Delete $INSTDIR\PC.PowerBuddy.exe
  Delete $INSTDIR\PC.PowerBuddy.exe.config
  Delete $INSTDIR\Microsoft.Practices.Prism.dll
  Delete $INSTDIR\uninstall.exe

  ; Remove shortcuts, if any
  Delete "$SMPROGRAMS\Power Buddy\*.*"

  ; Remove directories used
  RMDir "$SMPROGRAMS\Power Buddy"
  RMDir "$INSTDIR"

SectionEnd
