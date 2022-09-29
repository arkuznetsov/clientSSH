@ECHO OFF

echo %~dp0
@chcp 65001

set OSC_TEST_NAME=ClientSSH

set OSC_TEST_ROOT=%~dp0
set OSC_TEST_CWD=%OSC_TEST_ROOT%..
set OSC_TEST_SRC=%OSC_TEST_ROOT%../src

set OSC_TEST_LIB=%OSC_TEST_SRC%/%OSC_TEST_NAME%/bin/Debug/net452/%OSC_TEST_NAME%.dll

set OSC_TEST_TOOLS=%OSC_TEST_ROOT%
set OSC_TEST_NUGET=%OSC_TEST_TOOLS%nuget.exe
set OSC_TEST_NUNIT3=%OSC_TEST_TOOLS%NUnit.ConsoleRunner.3.6.1

set OSC_TEST_REPORTS=%OSC_TEST_ROOT%../build/reports

set SSH_TEST_KEY_PATH=%OSC_TEST_TOOLS%openssh/files/sftp-key

IF EXIST "%OSC_TEST_ROOT%.env" (
  FOR /f "usebackq tokens=*" %%a in ("%OSC_TEST_ROOT%.env") DO (
    FOR /F "tokens=1,2 delims==" %%b IN ("%%a") DO (
      set "%%b=%%c"
    )
  )
)

IF NOT EXIST "%OSC_TEST_NUGET%" (
  @"C:\windows\system32\WindowsPowerShell\v1.0\powershell.exe" curl -o "%OSC_TEST_NUGET%" https://dist.nuget.org/win-x86-commandline/latest/nuget.exe
)

@%OSC_TEST_NUGET% restore %OSC_TEST_SRC%

IF NOT EXIST "%OSC_TEST_NUNIT3%" (
  @%OSC_TEST_NUGET% install NUnit.ConsoleRunner -Version 3.6.1 -OutputDirectory %OSC_TEST_TOOLS%
)

@dotnet tool install --global dotnet-coverage
@dotnet restore %OSC_TEST_SRC%
@dotnet build %OSC_TEST_SRC% --configuration Debug

IF EXIST "%OSC_TEST_ROOT%docker-compose.yml" (
  @docker-compose --file %OSC_TEST_ROOT%docker-compose.yml --env-file %OSC_TEST_ROOT%.env up --build -d
)

@mkdir "%OSC_TEST_REPORTS%"
@dotnet-coverage collect -o %OSC_TEST_REPORTS%/coverage.xml -f xml "%OSC_TEST_NUNIT3%/tools/nunit3-console.exe %OSC_TEST_SRC%/NUnitTests/bin/Debug/net452/NUnitTests.dll --result=%OSC_TEST_REPORTS%/nunit-result.xml"
rem @rd /S /Q "%OSC_TEST_NUNIT3%"

IF EXIST "%OSC_TEST_ROOT%docker-compose.yml" (
  @docker-compose --file %OSC_TEST_ROOT%docker-compose.yml down
)

@exit /b %ERRORLEVEL%