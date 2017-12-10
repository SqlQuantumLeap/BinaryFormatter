
@ TITLE BinaryFormatter Tests ( Sql Quantum Leap -- https://SqlQuantumLeap.com/ )
@ ECHO.

SET BinPath=..\bin\Release

@ REM Clean up stale test output
IF EXIST SampleBinary.sql DEL SampleBinary.sql

@ ECHO.
@ ECHO.

%BinPath%\BinaryFormatter.exe ./SampleBinary.bin 25 /Console

@ ECHO.
@ ECHO.
@ ECHO 	Please verify that the contents of the file are the same as the output shown above
@ ECHO.

@ ECHO.
@ PAUSE

@ ECHO.
TYPE SampleBinary.sql

@ ECHO.
@ PAUSE
