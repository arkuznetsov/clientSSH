@ECHO OFF

echo %~dp0
@chcp 65001

set OSC_BUILD_NAME=clientSSH

set OSC_BUILD_ROOT=%~dp0
set OSC_BUILD_CWD=%OSC_BUILD_ROOT%..
set OSC_BUILD_SRC=%OSC_BUILD_ROOT%../src
set OSC_BUILD_BIN=%OSC_BUILD_ROOT%bin
set OSC_BUILD_BIN_OUT=%OSC_BUILD_SRC%\%OSC_BUILD_NAME%\bin\Release\net452

set OSC_BUILD_TOOLS=%OSC_BUILD_ROOT%../tools
set OSC_BUILD_NUGET=%OSC_BUILD_TOOLS%/nuget.exe

IF EXIST "%OSC_BUILD_ROOT%.env" (
  FOR /f "usebackq tokens=*" %%a in ("%OSC_BUILD_ROOT%.env") DO (
    FOR /F "tokens=1,2 delims==" %%b IN ("%%a") DO (
      set "%%b=%%c"
    )
  )
)

IF NOT EXIST "%OSC_BUILD_NUGET%" (
    @"C:\windows\system32\WindowsPowerShell\v1.0\powershell.exe" curl -o "%OSC_BUILD_NUGET%" https://dist.nuget.org/win-x86-commandline/latest/nuget.exe
)

@"%OSC_BUILD_NUGET%" restore %OSC_BUILD_SRC%

@dotnet restore %OSC_BUILD_SRC%
rem @dotnet msbuild %OSC_BUILD_SRC% -property:Configuration=Release
@dotnet build %OSC_BUILD_SRC% --configuration Release

@rd /S /Q "%OSC_BUILD_BIN%" 
@mkdir "%OSC_BUILD_BIN%"
@xcopy %OSC_BUILD_BIN_OUT%\*.dll %OSC_BUILD_BIN%\
@xcopy %OSC_BUILD_BIN_OUT%\*.xml %OSC_BUILD_BIN%\
@del /F /Q "%OSC_BUILD_BIN%\OneScript*.*" "%OSC_BUILD_BIN%\ScriptEngine*.*" "%OSC_BUILD_BIN%\DotNetZip*.*" "%OSC_BUILD_BIN%\Newtonsoft*.*"

@"C:\windows\system32\WindowsPowerShell\v1.0\powershell.exe" curl -o "%OSC_BUILD_ROOT%OneScriptDocumenter.zip" https://github.com/dmpas/OneScriptDocumenter/releases/download/1.0.14/documenter.zip
@"C:\Program Files\7-Zip\7z.exe" x -o%OSC_BUILD_ROOT%OneScriptDocumenter -y %OSC_BUILD_ROOT%OneScriptDocumenter.zip
@del /F /Q "%OSC_BUILD_ROOT%OneScriptDocumenter*.*"

@%OSC_BUILD_ROOT%OneScriptDocumenter\OneScriptDocumenter.exe json %OSC_BUILD_BIN%\syntaxHelp.json %OSC_BUILD_BIN_OUT%\%OSC_BUILD_NAME%.dll

@rd /S /Q "%OSC_BUILD_ROOT%OneScriptDocumenter"

opm build -o %OSC_BUILD_ROOT% %OSC_BUILD_ROOT%
@exit /b %ERRORLEVEL%