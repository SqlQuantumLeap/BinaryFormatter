# BinaryFormatter

Simple command-line utility that reads in a binary file and converts it to a string representation of those bytes (e.g. single byte of "3C" converts to two characters "3" followed by "C").

Output options are any combination of:

1. Text File
1. Clipboard (to then paste via <kbd>Control</kbd> + <kbd>V</kbd>)
1. Console (mostly for debugging / testing / research)


A feature / benefit of BinaryFormatter is that it splits the hex string
across N lines of "ChunkSize" bytes each, with all lines prior to the
final line being appended with the T-SQL line-continuation character: `\`. This makes it easier to work with long hex strings (anything over 100k characters).

This was originally intended for automating builds of SQLCLR projects (i.e. convert the DLL into a hex bytes string for the `CREATE ASSEMBLY [name] FROM 0x\` statement) and for creating T-SQL scripts that create Certificates (i.e. convert the `.cer` file, and optionally the `.pvk` file, into a hex bytes strings for the `CREATE CERTIFICATE [name] FROM BINARY = 0x\` statement). However, it can be used for many other purposes. And, if needed, it would be easy enough to add an input parameter to allow for overriding the line-continuation character (e.g. `^` for DOS).

### Example

Source binary file = `0123456789ABCDEF`

Output, using a _ChunkSize_ of `3`, contains:

```
012345\
6789AB\
CDEF
```

### Requirements

.NET 4.5.2 (or newer)

## Command Prompt / Automation Usage


```
BinaryFormatter path\to\binary_file_name.ext [ path\to\OutputFile.sql ] [ ChunkSize ]
	[ /Clipboard ] [ /Console ] [ /NoFile ]
```

* `ChunkSize` = the number of bytes per row. A byte is 2 characters: 00 - FF.
* `/Clipboard` = Copy output to clipboard (to then paste with Control-V)
* `/Console` = Send output to console
* `/NoFile` = Do not save to file, even if OutputFile is specified

Notes:

* Default ChunkSize = 10000
* Default OutputFile = {path\\to\\binary\_file\_name}.sql
* Maximum line length = (ChunkSize * 2) + 1.
* If `ChunkSize` is not supplied, the user is prompted to enter a value. A default value is shown inside square-brackets (e.g. `[ 10000 ]`), and hitting <kbd>Enter</kbd> without entering anything else will accept that default. Or, you can enter an integer value that is above zero. Any other value will cause the request to be repeated.


## Point-and-Click / "Send to" Usage

`BinaryFormatter.exe` can be used as a "Send to" destination so that you can easily convert binary files in Windows File Explorer simply by right-clicking on them, going to "Send to", and selecting **BinaryFormatter**.

To set this up, do the following:

1. Open File Explorer
1. Navigate to the `C:\Users\{your_windows_logon}\AppData\Roaming\Microsoft\Windows\SendTo` folder
1. Right-click in that folder, go to **New &gt;**, and select **Shortcut** (2nd from the top)
1. Click the **Browse...** button
1. Select `BinaryFormatter.exe` and click the **OK** button
1. Click the **Next** button
1. Change the name to: **Binary Formatter**
1. Click the **Finish** button
    <br><br>
  At this point you have a shortcut that will always prompt you to enter in a ChunkSize value, or at least just hit <kbd>Enter</kbd> to accept the default value, and will save to a file (no Console or Clipboard) using the default filename. If you want to convert files without being prompted to specify the Chunk Size, and/or if you some other combination of output options, do the following:<br><br>
1. Right-click on the `Binary Formatter` shortcut and select **Properties**
1. Go to the **Shortcut** tab
1. In the **Target:** text field, add any combination of the following options to the right of `...BinaryFormatter.exe`:
    * An `<integer>` &gt;= 1 (for ChunkSize).  I find that a value between 39 and 70 works best for scripts that will be posted online, and up to 10,000 (the default) is fine for Assemblies in release scripts.
    * `/Clipboard`
    * `/Console`
    * `/NoFile`
1. For the **Run:** drop-down, select **Minimized**
1. Click the **OK** button


And, you can even have multiple shortcuts, each one with a different combination of options :smile: .

### Example Target:

I use the following for testing. I use a ChunkSize of 39 since 39 bytes * 2 = 78 characters, plus 1 for the backslash = 79 characters total on all but the final line (unless there is only a single line, then no backslash), which is under the 80 character limit of some environments.

`...BinaryFormatter.exe 39 /Clipboard /NoFile`


## Roadmap

Create a separate Visual Studio project that re-uses relevant portions of the code and compile into a DLL / Assembly. The purpose would be to allow for direct use within code or frameworks that can reference Assemblies. The result would be a simple method call that returns a string (and/or creates an output file) which can be used inline, and no need to shell out to run an executable and then, most likely, read in the contents of the output file.
