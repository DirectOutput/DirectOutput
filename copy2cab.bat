cd bin\debug

timeout /t 1
dir "X:\DirectOutput" >nul 2>nul
if not errorlevel 1 (

  copy *.exe "X:\DirectOutput"
  copy *.dll "X:\DirectOutput"
  copy *.pdb "X:\DirectOutput"
  copy *Shapes.*" "X:\DirectOutput"
)

pause