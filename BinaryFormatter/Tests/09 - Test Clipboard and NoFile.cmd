
@ TITLE BinaryFormatter Tests ( Sql Quantum Leap -- https://SqlQuantumLeap.com/ )
@ ECHO.

SET BinPath=..\bin\Release

@ REM Clean up stale test output
IF EXIST SampleBinary.sql DEL SampleBinary.sql

@ ECHO.
@ ECHO.

%BinPath%\BinaryFormatter.exe 10 ./SampleBinary.bin /NoFile /Clipboard

@ ECHO.
@ ECHO.
@ ECHO 	Please verify that the Clipboard contents are 10 rows of 10 bytes (20 chars 0 to F) each
@ ECHO.

@ ECHO.
@ PAUSE

IF EXIST SampleBinary.sql ( @ECHO. & @ECHO 	ERROR: Output file should NOT exist! )

@ ECHO.
@ ECHO.
@ PAUSE
