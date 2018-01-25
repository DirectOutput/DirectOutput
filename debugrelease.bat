set DllPath=bin\Debug
set TempDate=%date%
set CurrDate=%TempDate:~6,4%%TempDate:~3,2%%TempDate:~0,2%
set TempTime=%time%
set CurrTime=%TempTime:~0,2%%TempTime:~3,2%%TempTime:~6,2%

rem tools\GetAssemblyVersion.exe tag "%DllPath%\DirectOutput.dll" >temp.txt
rem set /p VersionTag=<temp.txt
rem del Temp.txt

set Path=%cd%\DirectOutput\Builds

set ZipName=DirectOutput_rambo3_%CurrDate%_%CurrTime%.zip

cd %DllPath%
"C:\Program Files\7-Zip\7z.exe" a -tzip "%Path%\%ZipName%" "*.dll" 
"C:\Program Files\7-Zip\7z.exe" a -tzip "%Path%\%ZipName%" "*.exe"
"C:\Program Files\7-Zip\7z.exe" a -tzip "%Path%\%ZipName%" "*Shapes.*"
"C:\Program Files\7-Zip\7z.exe" a -tzip "%Path%\%ZipName%" "*.xml"
"C:\Program Files\7-Zip\7z.exe" a -tzip "%Path%\%ZipName%" "*.pdb"
cd ..\..


pause