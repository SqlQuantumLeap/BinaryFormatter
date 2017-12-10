
@ TITLE BinaryFormatter Tests ( Sql Quantum Leap -- https://SqlQuantumLeap.com/ )
@ ECHO.

SET BinPath=..\bin\Release

@ REM Clean up stale test output
IF EXIST SampleBinary.sql DEL SampleBinary.sql

@ ECHO.
@ ECHO.

%BinPath%\BinaryFormatter.exe 32 ./SampleBinary.bin /Console /NoFile /Clipboard

@ ECHO.
@ ECHO.
@ ECHO 	Please verify that the contents of the Clipboard are the same as the output shown above
@ ECHO.

@ ECHO.
@ PAUSE

IF EXIST SampleBinary.sql ( @ECHO. & @ECHO 	ERROR: Output file should NOT exist! )

@ ECHO.
@ ECHO.
@ PAUSE
