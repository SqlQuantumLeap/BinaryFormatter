
@ TITLE BinaryFormatter Tests ( Sql Quantum Leap -- https://SqlQuantumLeap.com/ )
@ ECHO.

SET BinPath=..\bin\Release


@ REM Clean up stale test output
IF EXIST SampleBinary.sql DEL SampleBinary.sql

@ ECHO.
@ ECHO.
@ ECHO.
@ ECHO 	This test should fail the check at the end!
@ ECHO.

%BinPath%\BinaryFormatter.exe ./SampleBinary.bin 8

@ ECHO.
@ PAUSE

@ ECHO.
FC /B SampleBinary.sql SampleOutput_ChunkSize-7.sql

@ ECHO.
@ PAUSE
