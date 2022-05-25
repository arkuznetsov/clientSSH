@ECHO OFF

echo %~dp0
@chcp 65001

@"%~dp0../test/nuget.exe" restore %~dp0../src

@dotnet restore %~dp0../src
@dotnet msbuild %~dp0../src -property:Configuration=Release

@rd /S /Q "%~dp0bin" 
@mkdir "%~dp0bin"
@xcopy %~dp0..\src\oscript-ssh\bin\Release\net452\*.dll %~dp0bin\
@xcopy %~dp0..\src\oscript-ssh\bin\Release\net452\*.xml %~dp0bin\
@del /F /Q "%~dp0bin\OneScript*.*" "%~dp0bin\ScriptEngine*.*" "%~dp0bin\NewtonSoft*.*" "%~dp0bin\DotNetZip*.*"

@"C:\windows\system32\WindowsPowerShell\v1.0\powershell.exe" curl -o "%~dp0OneScriptDocumenter.zip" https://github.com/dmpas/OneScriptDocumenter/releases/download/1.0.14/documenter.zip
@"C:\Program Files\7-Zip\7z.exe" x -o%~dp0OneScriptDocumenter -y %~dp0OneScriptDocumenter.zip
@del /F /Q "%~dp0OneScriptDocumenter*.*"

@%~dp0OneScriptDocumenter\OneScriptDocumenter.exe json %~dp0bin\syntaxHelp.json %~dp0..\src\oscript-ssh\bin\Release\net452\oscript-ssh.dll

@rd /S /Q "%~dp0OneScriptDocumenter"

opm build -o %~dp0 %~dp0
@exit /b %ERRORLEVEL%