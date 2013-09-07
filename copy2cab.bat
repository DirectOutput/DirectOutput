cd bin\debug

timeout /t 1
dir "X:\Visual Pinball\Tables\plugin\DirectOutput" >nul 2>nul
if not errorlevel 1 (

  copy *.exe "X:\Visual Pinball\Tables\plugin\DirectOutput"
  copy *.dll "X:\Visual Pinball\Tables\plugin\DirectOutput"
)
