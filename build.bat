@echo off
cls

.paket\paket.exe restore
if errorlevel 1 (
  exit /b %errorlevel%
)

IF %1.==. GOTO BUILD_ALL

set arg1=%1
ECHO Building %arg1%
"packages\FAKE\tools\Fake.exe" %arg1% -st
GOTO END

:BUILD_ALL
 ECHO Building all
 "packages\FAKE\tools\Fake.exe" build.fsx
GOTO END

:END
exit /b %errorlevel%