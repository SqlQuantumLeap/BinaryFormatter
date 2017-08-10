/*
 * BinaryFormatter
 * Version 1.0.1
 * Copyright (c) 2009-2017 Solomon Rutzky. All rights reserved.
 *
 * Blog: https://SqlQuantumLeap.com/
 *
 */
using System;
using System.Text;
using System.IO;

namespace BinaryFormatter
{
	class Program
	{
		static void Main(string[] args)
		{
            if (args.Length < 2)
            {
                DisplayUsage();

                return;
            }

            int _ChunkSize = 10000;
			string _FilePath = String.Empty;
			string _OutFile = String.Empty;

			foreach (string _Arg in args)
			{
				int _TempSize;

				if (int.TryParse(_Arg, out _TempSize))
				{
					_ChunkSize = _TempSize;
				}
				else
				{
					if (_FilePath == string.Empty)
					{
						_FilePath = _Arg;
					}
					else if (_OutFile == String.Empty)
					{
						_OutFile = _Arg;
					}
				}
			}

			if (_FilePath == String.Empty)
			{
                throw new ArgumentNullException("You must specify a source file.");
			}
			if (_OutFile == String.Empty)
			{
				_OutFile = @".\Assembly.sql";
			}

			FileStream _FileIn = null;
			BinaryReader _DataIn = null;
			FileStream _FileOut = null;
			StreamWriter _DataOut = null;

			try
			{
				using (_FileIn = new FileStream(_FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					using (_DataIn = new BinaryReader(_FileIn))
					{

						using (_FileOut = new FileStream(_OutFile, FileMode.Create, FileAccess.Write, FileShare.Read))
						{
							using (_DataOut = new StreamWriter(_FileOut, Encoding.ASCII))
							{
								_DataIn.BaseStream.Position = 0;

								byte[] _Bytes = new byte[_ChunkSize];

								int _ReadAmount = 0;

								while ((_ReadAmount = _DataIn.Read(_Bytes, 0, _ChunkSize)) > 0)
								{
									for (int _Index = 0; _Index < _ReadAmount; _Index++)
									{
										_DataOut.Write(_Bytes[_Index].ToString("X2"));
									}

									if (_DataIn.BaseStream.Position < _DataIn.BaseStream.Length)
									{
										_DataOut.WriteLine(@"\");
									}
								}
							}
						}
					}
				}
			}
			catch (Exception _Exception)
			{
				System.Console.WriteLine(_Exception.Message);
                System.Console.WriteLine("\n\tPress the <any> key to continue...\n");
				System.Console.ReadLine();
			}
			finally
			{
				_DataOut.Dispose();
				_FileOut.Dispose();
				_DataIn.Dispose();
				_FileIn.Dispose();
			}

			
		}

        private static void DisplayUsage()
        {
            System.Console.WriteLine("\nBinaryFormatter");
            System.Console.WriteLine("Version 1.0.1");
            System.Console.WriteLine("Copyright (c) 2009-2017 Solomon Rutzky. All rights reserved.\n");
            System.Console.WriteLine("https://SqlQuantumLeap.com/\n");

            System.Console.WriteLine("Format binary file into hex string for SQL script.");
            System.Console.WriteLine("Output broken into multiple lines of \"ChunkSize\" bytes.");
            System.Console.WriteLine("If there are multiple lines, all lines except the final");
            System.Console.WriteLine("line are appended with the line-continuation character of: \\");

            System.Console.WriteLine("\nUsage:");
            System.Console.WriteLine("\n\tBinaryFormatter path\\to\\binary_file.ext [path\\to\\OutputFile.sql] [ChunkSize]");
            System.Console.WriteLine("\n\tChunkSize = the number of bytes per row. A byte is 2 characters: 00 - FF.");
            System.Console.WriteLine("\tMaximum line length = (ChunkSize * 2) + 1.");
            System.Console.WriteLine("\tDefault OutputFile = Assembly.sql\n");
            System.Console.WriteLine("\tDefault ChunkSize = 10000\n");
            return;
        }
    }

}
