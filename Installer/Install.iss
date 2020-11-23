[Setup]
AppName=Elastic Search Manager
AppVerName=ElasticSearchManager
AppCopyright=Copyright (C) 2019-2021 Robert Gelb
AppPublisher=Robert Gelb
DefaultDirName={userpf}\ElasticSearchManager
DisableDirPage=yes
DisableProgramGroupPage=yes
DisableReadyPage=yes
UninstallDisplayIcon={app}\ElasticSearchManager.exe
OutputBaseFilename=ElasticSearchManagerSetup
AppID=ElasticSearchManager.1
VersionInfoVersion=0.7.2
PrivilegesRequired=lowest

[Files]
Source: "..\bin\debug\ElasticSearchManager.exe"; DestDir: "{app}"
Source: "CleanFiles\ElasticSearchManager.exe.config"; DestDir: "{app}"
Source: "..\bin\debug\ScintillaNET.dll"; DestDir: "{app}"
Source: "..\bin\debug\Elasticsearch.Net.dll"; DestDir: "{app}"
Source: "..\bin\debug\Nest.dll"; DestDir: "{app}"
Source: "..\bin\debug\Automapper.dll"; DestDir: "{app}"
Source: "..\bin\debug\Newtonsoft.Json.dll"; DestDir: "{app}"

[Icons]
Name: "{group}\Elastic Search Manager"; Filename: "{app}\ElasticSearchManager.exe"; 

[Run]
Filename: "{app}\ElasticSearchManager.exe"; Description: "Launch Elastic Search Manager"; Flags: postinstall nowait skipifsilent

; rename the uninstaller to avoid tripping windows security
; unfortunately doesn't work because in the Apps & Features, the uninstaller is still being kicked off with privilige elevation
; Filename: {cmd}; Parameters: "/C Move ""{app}\unins000.exe"" ""{app}\RemoveElasticSearchManager.exe"""; StatusMsg: Installing Elastic Search Manager...; Flags: RunHidden WaitUntilTerminated
; Filename: {cmd}; Parameters: "/C Move ""{app}\unins000.dat"" ""{app}\RemoveElasticSearchManager.dat"""; StatusMsg: Installing Elastic Search Manager...; Flags: RunHidden WaitUntilTerminated
; Filename: REG.exe; Parameters: "ADD ""HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\ElasticSearchManager.1_is1"" /V ""UninstallString""      /T ""REG_SZ"" /D ""\""{app}\RemoveElasticSearchManager.exe\"""" /F"; StatusMsg: Installing Elastic Search Manager...; Flags: RunHidden WaitUntilTerminated
; Filename: REG.exe; Parameters: "ADD ""HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\ElasticSearchManager.1_is1"" /V ""QuietUninstallString"" /T ""REG_SZ"" /D ""\""{app}\RemoveElasticSearchManager.exe\"" /SILENT"" /F"; StatusMsg: Installing Elastic Search Manager...; Flags: RunHidden WaitUntilTerminated