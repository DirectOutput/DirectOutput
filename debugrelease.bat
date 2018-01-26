@echo off

rem ===========================================================
rem ###   PATH CONFIGURATION

rem ###   DOF DLL path
set DofDllPath=bin\Debug

rem ###   ProPinball bridge DLL path
set ProPinBridgePath=Debug

rem ###   ProPinball DOFSlave path
set ProPinSlavePath=ProPinballSlave\bin\x86\Debug

rem ###   Zip file path
set ZipPath=%cd%\Builds
set ZipName=DirectOutput-mjr-%CurrDate%.zip
set ZipFile=%ZipPath%\%ZipName%


rem ===========================================================

rem ###   Date/time to embed in zip file name
set TempDate=%date%
set CurrDate=%TempDate:~10,4%%TempDate:~7,2%%TempDate:~4,2%
set TempTime=%time%
set CurrTime=%TempTime:~0,2%%TempTime:~3,2%%TempTime:~6,2%

rem ###   Current DOF assembly version
tools\GetAssemblyVersion.exe tag "%DofDllPath%\DirectOutput.dll" >temp.txt
set /p VersionTag=<temp.txt
del Temp.txt

rem ###   Announce what we're doing
echo %VersionTag% Debug
echo -^> %ZipFile%
echo.

rem ###   Add DOF files
pushd %DofDllPath%
if exist "%ZipFile%" del "%ZipFile%"
zip "%ZipFile%" *.dll *.exe *Shapes.* *.xml
popd

rem ###   Add ProPinball support files
zip -j "%ZipFile%" "%ProPinSlavePath%\*.exe"

echo.
