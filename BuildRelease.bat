@echo off
rem   DirectOutput Release Builder script

if %1# == # (
  echo usage: BuildRelease ^<author-tag^>
  echo.
  echo ^<author-tag^> is a short identifier, such as your initials, that will be embedded
  echo in the .msi and .zip filenames to help identify the source of the build files, if you
  echo plan to distribute them.
  goto EOF
)

rem Clean the release configurations
echo ^>^>^> Removing old builds
msbuild DirectOutput.sln -t:Clean -p:Configuration=Release;Platform=x86 -v:q -nologo
if errorlevel 1 goto abort
msbuild DirectOutput.sln -t:Clean -p:Configuration=Release;Platform=x64 -v:q -nologo
if errorlevel 1 goto abort

rem Build the release configurations
echo.
echo ^>^>^> Building Release^|x86
msbuild DirectOutput.sln -t:Build -p:Configuration=Release;Platform=x86 -v:q -nologo
if errorlevel 1 goto abort

echo.
echo ^>^>^> Building Release^|x64
msbuild DirectOutput.sln -t:Build -p:Configuration=Release;Platform=x64 -v:q -nologo
if errorlevel 1 goto abort

rem Build the release files
echo.
echo ^>^>^> Creating release packages in .\Builds
call MakeZip x86 release %1
call MakeZip x64 release %1
 
rem successful completion
echo === Build completed successfully ===
goto EOF


:abort
echo MSBUILD exited with error - aborted

:EOF
