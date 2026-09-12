; Inno Setup — installer Windows 10/11 (x86 + x64, .NET 6 self-contained)
; Build: tools\innosetup\ISCC.exe BuildInstaller.iss

[Setup]
AppId={{A1B2C3D4-E5F6-4A5B-8C9D-0E1F2A3B4C5D}
AppName=AutoMouseKeyboard
AppVersion=1.0.0
AppPublisher=AutoMouseKeyboard
AppSupportURL=https://github.com/Ndqitisme/AutoMouseKeyboard
AppUpdatesURL=https://github.com/Ndqitisme/AutoMouseKeyboard
DefaultDirName={autopf}\AutoMouseKeyboard
DefaultGroupName=AutoMouseKeyboard
OutputDir=Installer
OutputBaseFilename=AutoMouseKeyboard_Setup_Win10
Compression=lzma
SolidCompression=yes
SetupIconFile=AutoMouseKeyboard\assets\app.ico
UninstallDisplayIcon={app}\AutoMouseKeyboard.exe
UninstallDisplayName=AutoMouseKeyboard
PrivilegesRequired=admin
MinVersion=10.0
ArchitecturesInstallIn64BitMode=x64compatible
ArchitecturesAllowed=x86compatible x64compatible
RestartIfNeededByRun=no
CloseApplications=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "Tạo shortcut trên Desktop"; GroupDescription: "Shortcuts:"
Name: "quicklaunchicon"; Description: "Tạo shortcut trong Quick Launch"; GroupDescription: "Shortcuts:"; Flags: unchecked

[Files]
; x64 build - chỉ cài trên hệ thống 64-bit
Source: "publish\win10-x64\AutoMouseKeyboard.exe"; DestDir: "{app}"; Flags: ignoreversion; Check: Is64BitInstallMode
; x86 build - cài trên hệ thống 32-bit
Source: "publish\win10-x86\AutoMouseKeyboard.exe"; DestDir: "{app}"; Flags: ignoreversion; Check: not Is64BitInstallMode

[Icons]
Name: "{group}\AutoMouseKeyboard"; Filename: "{app}\AutoMouseKeyboard.exe"; WorkingDir: "{app}"
Name: "{group}\Uninstall AutoMouseKeyboard"; Filename: "{uninstallexe}"
Name: "{autodesktop}\AutoMouseKeyboard"; Filename: "{app}\AutoMouseKeyboard.exe"; Tasks: desktopicon; WorkingDir: "{app}"
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\AutoMouseKeyboard"; Filename: "{app}\AutoMouseKeyboard.exe"; Tasks: quicklaunchicon; WorkingDir: "{app}"

[Run]
Filename: "{app}\AutoMouseKeyboard.exe"; Description: "Mở AutoMouseKeyboard"; Flags: nowait postinstall skipifsilent
