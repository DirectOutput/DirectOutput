@echo off

rem if %1!==! goto exit


set DllPath=C:\Users\Tom\Documents\GitHub\DirectOutput\B2SServerPlugin\bin\Debug\




set TempDate=%date%

set CurrDate=%TempDate:~6,4%-%TempDate:~3,2%-%TempDate:~0,2%

copy file.txt file-%currdate%.txt

tools\GetAssemblyVersion.exe filename "%DllPath%DirectOutput.dll" >temp.txt

set /p FileNameVersion=<temp.txt

del Temp.txt


set Path=C:\Users\Tom\Google Drive\DirectOutput\Alpha\%CurrDate%\

mkdir %Path%

set ZipName=DirectOutput %FileNameVersion%.zip

echo Will create %Path%%ZipName%

del "%Path%%ZipName%"

"C:\Program Files\7-Zip\7z.exe" a -tzip -x!B2SServerPluginInterface*.dll  "%Path%%ZipName%" "%DllPath%*.dll" 


:exit