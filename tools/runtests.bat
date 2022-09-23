@ECHO OFF

echo %~dp0
@chcp 65001

set SSH_TEST_CWD=%~dp0..
set SSH_TEST_TOOLS=%~dp0
set SSH_TEST_SRC=%SSH_TEST_CWD%/src
set SSH_TEST_KEY_PATH=%SSH_TEST_TOOLS%openssh/files/sftp-key
set SSH_TEST_LIB_PATH=%SSH_TEST_CWD%/src/clientSSH/bin/Debug/net452/ClientSSH.dll

FOR /f "usebackq tokens=*" %%a in ("%~dp0openssh\.env") DO (
  FOR /F "tokens=1,2 delims==" %%b IN ("%%a") DO (
    set "%%b=%%c"
  )
)

IF NOT EXIST "%~dp0nuget.exe" (
  @"C:\windows\system32\WindowsPowerShell\v1.0\powershell.exe" curl -o "%~dp0nuget.exe" https://dist.nuget.org/win-x86-commandline/latest/nuget.exe
)
@%~dp0nuget.exe restore %~dp0../src
@%~dp0nuget.exe install NUnit.ConsoleRunner -Version 3.6.1 -OutputDirectory %~dp0../tools
@dotnet tool install --global dotnet-coverage
@dotnet restore %~dp0../src
@dotnet build %~dp0../src --configuration Debug
@docker-compose --file %~dp0openssh/docker-compose.yml --env-file %~dp0openssh/.env up --build -d
@mkdir "%~dp0../build/reports"
@dotnet-coverage collect -o %~dp0../build/reports/coverage.xml -f xml "%~dp0../tools/NUnit.ConsoleRunner.3.6.1/tools/nunit3-console.exe %~dp0../src/NUnitTests/bin/Debug/net452/NUnitTests.dll --result=%~dp0../build/reports/nunit-result.xml"
@rd /S /Q "%~dp0../tools/NUnit.ConsoleRunner.3.6.1"
@docker-compose --file %~dp0openssh/docker-compose.yml down
@exit /b %ERRORLEVEL%