; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "NoBrand Focuser"
#define MyAppVersion "1.0"
#define MyAppPublisher "NoBrand"
#define MyAppURL "https://ascom-standards.org"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{E646068D-CC17-4F54-8352-D4EE19918E97}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\NoBrand\Focuser
DisableProgramGroupPage=yes
; Uncomment the following line to run in non administrative install mode (install for current user only.)
;PrivilegesRequired=lowest
OutputDir=Setup
OutputBaseFilename=NBFocuserSetup1.0
SetupIconFile=Focuser.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern
;WizardSmallImageFile=Ascom.bmp

[Languages]
;Name: "korean"; MessagesFile: "compiler:Languages\Korean.isl"
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
Source: "bin\Release\NBFocuser.exe"; DestDir: "{app}"; Flags: ignoreversion 
Source: "bin\Release\NBFocuser.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\FTD2XX_NET.dll"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Run]
Filename: "{app}\NBFocuser.exe"; Parameters: "/regserver"

[UninstallRun]
Filename: "{app}\NBFocuser.exe"; Parameters: "/unregserver"

