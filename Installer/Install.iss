[Setup]
AppName=Elastic Search Manager
AppVerName=ElasticSearchManager
AppCopyright=Copyright (C) 2018 Robert Gelb
AppPublisher=Robert Gelb
DefaultDirName={userpf}\ElasticSearchManager
DisableDirPage=yes
DisableProgramGroupPage=yes
DisableReadyPage=yes
UninstallDisplayIcon={app}\ElasticSearchManager.exe
OutputBaseFilename=ElasticSearchManagerSetup
AppID=ElasticSearchManager.1
VersionInfoVersion=0.4

[Files]
Source: "..\bin\release\ElasticSearchManager.exe"; DestDir: "{app}"
Source: "CleanFiles\ElasticSearchManager.exe.config"; DestDir: "{app}"
Source: "..\bin\release\ScintillaNET.dll"; DestDir: "{app}"
Source: "..\bin\release\Elasticsearch.Net.dll"; DestDir: "{app}"
Source: "..\bin\release\Nest.dll"; DestDir: "{app}"

[Icons]
Name: "{group}\Elastic Search Manager"; Filename: "{app}\ElasticSearchManager.exe"; 

[Run]
Filename: "{app}\ElasticSearchManager.exe"; Description: "Launch Elastic Search Manager"; Flags: postinstall nowait skipifsilent