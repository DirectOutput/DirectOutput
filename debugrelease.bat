@echo off

rem ===========================================================
rem
rem   The list of DOF DLL builds files is taken from
rem   .\manifest.debug.x86.txt.  Edit that file to add or
rem   remove files.

rem ===========================================================
rem ###   PATH CONFIGURATION

rem ###   DOF DLL path
set DofDllPath=bin\Debug

rem ###   ProPinball bridge DLL path
set ProPinBridgePath=Debug

rem ###   ProPinball DOFSlave path
set ProPinSlavePath=ProPinballSlave\bin\x86\Debug

rem ###   Zip file path
set ZipPath=Builds

rem ###   MSI setup output
set MSIPath=DOFSetup\bin\Debug

rem ===========================================================
rem ###   Date/time to embed in zip file name
set TempDate=
for /f "skip=1" %%i in ('wmic os get localdatetime') do if not defined TempDate set TempDate=%%i
set CurrDate=%TempDate:~0,8%
set CurrTime=%TempDate:~8,6%

rem ###   Zip file
set ZipName=DirectOutput-mjr-%CurrDate%.zip
set ZipFile=%ZipPath%\%ZipName%


rem ===========================================================
rem ###
rem ###   Main build steps
rem ###


rem ###   Current DOF assembly version
tools\GetAssemblyVersion.exe tag "%DofDllPath%\DirectOutput.dll" >temp.txt
set /p VersionTag=<temp.txt
del Temp.txt

rem ###   Announce what we're doing
echo %VersionTag% Debug
echo -^> %ZipFile%
echo.

rem ###   Delete any old copy of the ZIP file
if exist "%ZipFile%" del "%ZipFile%"

rem ###   Add the LICENSE file
zip -j "%ZipFile%" LICENSE

rem ###   Add DOF DLL files
for /F "eol=# delims=" %%i in (manifest.debug.x86.txt) do (
    zip -j "%ZipFile%" "%DofDllPath%\%%i"
)

rem ###   Add DOF config example files
zip "%ZipFile%" config\examples\*.xml

rem ###   Add ProPinball support files
zip -j "%ZipFile%" "%ProPinSlavePath%\*.exe"


rem ###   Copy MSI setup to build folder
copy "%MSIPath%\DOFSetup.msi" "%ZipPath%\DirectOutput-mjr-%CurrDate%.msi"

echo.
