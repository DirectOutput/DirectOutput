

rem if %1!==! goto exit


set DllPath=C:\Users\Tom\Documents\GitHub\DirectOutput\B2SServerPlugin\bin\Debug\
set LedControlTesterPath=C:\Users\Tom\Documents\GitHub\DirectOutput\LedControlFileTester\bin\Debug\




set TempDate=%date%

set CurrDate=%TempDate:~6,4%-%TempDate:~3,2%-%TempDate:~0,2%

rem copy file.txt file-%currdate%.txt

tools\GetAssemblyVersion.exe tag "%DllPath%DirectOutput.dll" >temp.txt

set /p VersionTag=<temp.txt

"C:\Program Files (x86)\Git\bin\git.exe" tag -a %VersionTag% -m '%VersionTag%'

del Temp.txt

tools\GetAssemblyVersion.exe filename "%DllPath%DirectOutput.dll" >temp.txt

set /p FileNameVersion=<temp.txt

del Temp.txt


set Path=C:\Users\Tom\Google Drive\DirectOutput\Alpha\%CurrDate%\

mkdir %Path%

set ZipName=DirectOutput %FileNameVersion%.zip

echo Will create %Path%%ZipName%

del "%Path%%ZipName%"

"C:\Program Files\7-Zip\7z.exe" a -tzip -x!B2SServerPluginInterface*.dll  "%Path%%ZipName%" "%DllPath%*.dll" 
"C:\Program Files\7-Zip\7z.exe" a -tzip "%Path%%ZipName%" "%LedControlTesterPath%LedControlFileTester.exe" 

:exit