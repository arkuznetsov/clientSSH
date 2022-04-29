@ECHO OFF

echo %~dp0
@docker-compose --file %~dp0docker-compose.yml --env-file %~dp0.env up --build -d
@%~dp0../../testrunner/NUnit.ConsoleRunner.3.6.1/tools/nunit3-console.exe %~dp0../../NUnitTests/bin/Release/net452/NUnitTests.dll
@docker-compose --file %~dp0docker-compose.yml down
@exit /b %ERRORLEVEL%