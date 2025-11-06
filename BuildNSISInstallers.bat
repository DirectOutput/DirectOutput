@echo off
echo Building DirectOutput NSIS Installers...

REM Check if NSIS is installed
set "MAKENSIS=C:\Program Files (x86)\NSIS\makensis.exe"
if not exist "%MAKENSIS%" (
    set "MAKENSIS=C:\Program Files\NSIS\makensis.exe"
)
if not exist "%MAKENSIS%" (
    echo NSIS is not installed.
    echo Please install NSIS from https://nsis.sourceforge.io/
    pause
    exit /b 1
)

REM Build 32-bit installer
echo.
echo Building 32-bit installer...
"%MAKENSIS%" /DARCH=32 DOFInstaller.nsi
if %ERRORLEVEL% neq 0 (
    echo Failed to build 32-bit installer
    pause
    exit /b 1
)

REM Build 64-bit installer
echo.
echo Building 64-bit installer...
"%MAKENSIS%" /DARCH=64 DOFInstaller.nsi
if %ERRORLEVEL% neq 0 (
    echo Failed to build 64-bit installer
    pause
    exit /b 1
)

echo.
echo Both installers built successfully!
echo Output files:
echo - Output\DOFSetup-NSIS-32bit-Setup.exe
echo - Output\DOFSetup-NSIS-64bit-Setup.exe
pause