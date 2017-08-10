# BinaryFormatter

Simple command-line utility that reads in a binary file and writes out a
text file containing the hex representation of the source bytes.

A feature / benefit of BinaryFormatter is that it splits the hex string
across N lines of "ChunkSize" bytes each, with all lines prior to the
final line being appended with the T-SQL line-continuation character: `/`.

**Usage:**

`BinaryFormatter path\to\binary_file.ext path\to\Output.sql [ ChunkSize ]`

* _ChunkSize_ = the number of bytes per row. A byte is 2 characters: 00 - FF.
* Maximum line length = (ChunkSize * 2) + 1.
* Default ChunkSize = 10000
