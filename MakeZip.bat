@echo off
rem ===========================================================
rem
rem   DOF release ZIP file builder
rem
rem   Usage: MakeZip <cpu> <configuration> <author>
rem
rem   <cpu> = x86 | x64
rem
rem   <configuration> = debug | release
rem
rem   <author> = author ID tag (a short name to embed in the .zip and
rem   .msi filenames to help identify the source of the files, if you
rem   plan to distribute them; I use my initials "mjr"
rem
rem   The list of DOF DLL builds files is taken from .\manifest.x86.txt
rem   and .\manifest.x64.txt.  Edit those files to add or remove files.
rem
rem ===========================================================


if %1# == x86# goto platformOK
if %1# == x64# goto platformOK
echo Invalid platform - must be one of "x86" or "x64"
goto usageExit
:platformOK

if %2# == debug# goto configOK
if %2# == release# goto configOK
echo Invalid configuration - must be one of "debug" or "release"
goto usageExit

:configOK
if %3# == # goto usageExit

goto paramsOK

:usageExit
echo.
echo Usage: MakeReleaseZip ^<platform^> ^<config^> ^<author^>
echo.
echo platform = x86 ^| x64
echo config   = debug ^| release
echo author   = a short author name or initials to include in the generated .msi and .zip filenames
echo.
goto EOF

:paramsOK

rem ###   PATH CONFIGURATION

rem ###   DOF DLL path
set DofDllPath=bin\%1\%2

rem ###   Zip file path
set ZipPath=Builds

rem ###   MSI setup output
set MSIPath=DOFSetup\bin\%1\%2

rem ===========================================================
rem ###   Date/time to embed in zip file name
set TempDate=
for /f "skip=1" %%i in ('wmic os get localdatetime') do if not defined TempDate set TempDate=%%i
set CurrDate=%TempDate:~0,8%
set CurrTime=%TempDate:~8,6%

rem ###   Zip file
set ZipName=DirectOutput-%3-%1-%2-%CurrDate%.zip
set ZipFile=%ZipPath%\%ZipName%


rem ===========================================================
rem ###
rem ###   Main build steps
rem ###


rem ###   Announce what we're doing
echo Creating %ZipFile%...
echo.

rem ###   Delete any old copy of the ZIP file
if exist "%ZipFile%" del "%ZipFile%"

rem ###   Add the LICENSE file
zip -j "%ZipFile%" LICENSE

rem ###   Add DOF DLL files
for /F "eol=# delims=" %%i in (manifest.%1.txt) do (
    zip -j "%ZipFile%" "%DofDllPath%\%%i"
)

rem ###   Add DOF config example files
zip "%ZipFile%" config\examples\*.xml


rem ###   Copy MSI setup to build folder
copy "%MSIPath%\DOFSetup.msi" "%ZipPath%\DirectOutput-%3-%1-%2-%CurrDate%.msi"

echo.

:EOF
