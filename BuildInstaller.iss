; Script Inno Setup để tạo installer cho cả x86 và x64 (Universal)
; Cần cài Inno Setup: https://jrsoftware.org/isdl.php

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
OutputBaseFilename=AutoMouseKeyboard_Setup
Compression=lzma
SolidCompression=yes
SetupIconFile=AutoMouseKeyboard\assets\app.ico
UninstallDisplayIcon={app}\AutoMouseKeyboard.exe
UninstallDisplayName=AutoMouseKeyboard
PrivilegesRequired=admin
ArchitecturesInstallIn64BitMode=x64
ArchitecturesAllowed=x86compatible x64
RestartIfNeededByRun=no
CloseApplications=no

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "Tạo shortcut trên Desktop"; GroupDescription: "Shortcuts:"
Name: "quicklaunchicon"; Description: "Tạo shortcut trong Quick Launch"; GroupDescription: "Shortcuts:"; Flags: unchecked

[Files]
; File x64 - chỉ cài trên hệ thống 64-bit
Source: "AutoMouseKeyboard\bin\Release\net6.0-windows\win-x64\publish\AutoMouseKeyboard.exe"; DestDir: "{app}"; Flags: ignoreversion; Check: Is64BitInstallMode
; File x86 - cài trên hệ thống 32-bit hoặc 64-bit
Source: "AutoMouseKeyboard\bin\Release\net6.0-windows\win-x86\publish\AutoMouseKeyboard.exe"; DestDir: "{app}"; Flags: ignoreversion; Check: not Is64BitInstallMode
; Bản vá KB4474419 v3 cho Windows 7 x64 (yêu cầu SHA-2)
Source: "Redistributables\windows6.1-kb4474419-v3-x64_b5614c6cea5cb4e198717789633dca16308ef79c.msu"; DestDir: "{tmp}"; Flags: deleteafterinstall; Check: ShouldInstallKB4474419
; Bản cập nhật phục vụ KB4490628 (servicing stack) cho Windows 7 x64
Source: "Redistributables\windows6.1-kb4490628-x64_d3de52d6987f7c8bdc2c015dca69eac96047c76e.msu"; DestDir: "{tmp}"; Flags: deleteafterinstall; Check: ShouldInstallKB4490628

[Icons]
Name: "{group}\AutoMouseKeyboard"; Filename: "{app}\AutoMouseKeyboard.exe"; WorkingDir: "{app}"
Name: "{group}\Uninstall AutoMouseKeyboard"; Filename: "{uninstallexe}"
Name: "{autodesktop}\AutoMouseKeyboard"; Filename: "{app}\AutoMouseKeyboard.exe"; Tasks: desktopicon; WorkingDir: "{app}"
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\AutoMouseKeyboard"; Filename: "{app}\AutoMouseKeyboard.exe"; Tasks: quicklaunchicon; WorkingDir: "{app}"

[Run]
; Cài đặt KB4474419 v3 nếu đang ở Windows 7 x64 và chưa có bản vá (dùng wusa để mở .msu)
Filename: "{sys}\wusa.exe"; Parameters: "/quiet /norestart ""{tmp}\windows6.1-kb4474419-v3-x64_b5614c6cea5cb4e198717789633dca16308ef79c.msu"""; StatusMsg: "Đang cài đặt bản vá KB4474419..."; Flags: runhidden; Check: ShouldInstallKB4474419; AfterInstall: MarkPatchNotice
; Cài đặt KB4490628 (servicing stack) nếu đang ở Windows 7 x64 và chưa có (dùng wusa để mở .msu)
Filename: "{sys}\wusa.exe"; Parameters: "/quiet /norestart ""{tmp}\windows6.1-kb4490628-x64_d3de52d6987f7c8bdc2c015dca69eac96047c76e.msu"""; StatusMsg: "Đang cài đặt bản vá KB4490628..."; Flags: runhidden; Check: ShouldInstallKB4490628; AfterInstall: MarkPatchNotice

[Code]
function IsWin7x64: Boolean;
var
  Version: TWindowsVersion;
begin
  GetWindowsVersionEx(Version);
  Result := (Version.Major = 6) and (Version.Minor = 1) and Is64BitInstallMode;
end;

function IsKB4474419Installed: Boolean;
var
  State: Cardinal;
  Key: String;
  Keys: array[0..1] of String;
  i: Integer;
begin
  Result := False;
  { Kiểm tra nhiều phiên bản gói; 6.1.3.2 (mới hơn) và 6.1.1.5 (cũ hơn) }
  Keys[0] := 'SOFTWARE\Microsoft\Windows\CurrentVersion\Component Based Servicing\Packages\Package_for_KB4474419~31bf3856ad364e35~amd64~~6.1.3.2';
  Keys[1] := 'SOFTWARE\Microsoft\Windows\CurrentVersion\Component Based Servicing\Packages\Package_for_KB4474419~31bf3856ad364e35~amd64~~6.1.1.5';

  for i := 0 to GetArrayLength(Keys) - 1 do
  begin
    Key := Keys[i];
    if RegQueryDWordValue(HKLM, Key, 'CurrentState', State) then
    begin
      { 0x70 (112) = installed }
      if State >= $70 then
      begin
        Result := True;
        exit;
      end;
    end;
  end;
end;

function ShouldInstallKB4474419: Boolean;
begin
  Result := IsWin7x64 and (not IsKB4474419Installed);
end;

function IsKB4490628Installed: Boolean;
var
  State: Cardinal;
  Key: String;
begin
  Result := False;
  Key := 'SOFTWARE\Microsoft\Windows\CurrentVersion\Component Based Servicing\Packages\Package_for_KB4490628~31bf3856ad364e35~amd64~~6.1.1.2';
  if RegQueryDWordValue(HKLM, Key, 'CurrentState', State) then
  begin
    { 0x70 (112) = installed }
    Result := State >= $70;
  end;
end;

function ShouldInstallKB4490628: Boolean;
begin
  Result := IsWin7x64 and (not IsKB4490628Installed);
end;

var
  PatchNoticeNeeded: Boolean;

procedure MarkPatchNotice;
begin
  PatchNoticeNeeded := True;
end;

procedure CurStepChanged(CurStep: TSetupStep);
var
  MsgText: String;
begin
  if (CurStep = ssPostInstall) and PatchNoticeNeeded then
  begin
    MsgText :=
      'If the app does not open, please restart your computer.' + #13#10#13#10 +
      'Nếu không mở được ứng dụng, vui lòng khởi động lại máy tính.' + #13#10#13#10 +
      '如果无法打开应用，请重启电脑。';
    MsgBox(MsgText, mbInformation, MB_OK);
  end;
end;