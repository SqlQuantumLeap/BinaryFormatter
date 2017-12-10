
@ TITLE BinaryFormatter Tests ( Sql Quantum Leap -- https://SqlQuantumLeap.com/ )
@ ECHO.

SET BinPath=..\bin\Release


@ REM Clean up stale test output
IF EXIST SampleBinary.sql DEL SampleBinary.sql

@ ECHO.
@ ECHO.
@ ECHO.
@ ECHO 	Please just hit "enter" when prompted for the ChunkSize
@ ECHO.

%BinPath%\BinaryFormatter.exe ./SampleBinary.bin

@ ECHO.
@ PAUSE

@ ECHO.
FC /B SampleBinary.sql SampleOutput_ChunkSize-100(orMore).sql

@ ECHO.
@ PAUSE
