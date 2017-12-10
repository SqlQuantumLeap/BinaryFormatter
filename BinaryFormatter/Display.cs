/*
 * BinaryFormatter
 * Version 2.0
 * Copyright (c) 2009-2017 Solomon Rutzky. All rights reserved.
 *
 * Location: https://github.com/SqlQuantumLeap/BinaryFormatter
 * 
 * Blog: https://SqlQuantumLeap.com/
 *
 */
using System;

namespace SqlQuantumLeap.BinaryFormatter
{
    internal class Display
    {
        internal static void Usage()
        {
            Console.WriteLine("\nBinaryFormatter");
            Console.WriteLine("Version {0}",
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(2));
            Console.WriteLine("Copyright (c) 2009-2017 Solomon Rutzky. All rights reserved.");
            Console.WriteLine("https://SqlQuantumLeap.com/\n");

            Console.WriteLine("Format binary file into hex string for SQL script.");
            Console.WriteLine("Output broken into multiple lines of \"ChunkSize\" bytes.");
            Console.WriteLine("If there are multiple lines, all lines except the final");
            Console.WriteLine("line are appended with the line-continuation character of: \\");

            Console.Write("\nUsage:");
            Console.WriteLine("\tBinaryFormatter [ /? | /help ]");
            Console.WriteLine("\n\tDisplay this help info");

            Console.Write("\nUsage:");
            Console.WriteLine("\tBinaryFormatter path\\to\\binary_file.ext [path\\to\\OutputFile.sql] [ChunkSize]");
            Console.WriteLine("\t\t[ /Clipboard ] [ /Console ] [ /NoFile ]\n");
            Console.WriteLine("\tChunkSize = the number of bytes per row. A byte is 2 characters: 00 - FF.");
            Console.WriteLine("\t/Clipboard = Copy output to clipboard (to then paste with Control-V)");
            Console.WriteLine("\t/Console = Send output to console");
            Console.WriteLine("\t/NoFile = Do not save to file, even if specified");
            Console.WriteLine("");

            Console.WriteLine("\tDefault ChunkSize = 10000");
            Console.WriteLine("\tDefault OutputFile = {path\\to\\binary_file_name}.sql");
            Console.WriteLine("\tMaximum line length = (ChunkSize * 2) + 1.");

            Console.WriteLine("\nVisit https://SqlQuantumLeap.com/ for other useful utilities and more.");

            return;
        }

        internal static void Error(string ErrorMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ErrorMessage);
            Console.ResetColor();

            return;
        }
    }
}
