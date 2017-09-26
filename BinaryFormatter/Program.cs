/*
 * BinaryFormatter
 * Version 1.0.2
 * Copyright (c) 2009-2017 Solomon Rutzky. All rights reserved.
 *
 * Location: https://github.com/SqlQuantumLeap/BinaryFormatter
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
            if (args.Length < 1)
            {
                DisplayUsage();

                return;
            }

            int _ChunkSize = 10000;
            bool _ChunkSizeIsCustom = false;
			string _FilePath = String.Empty;
			string _OutFile = String.Empty;

			foreach (string _Arg in args)
			{
				int _TempSize;

				if (int.TryParse(_Arg, out _TempSize))
				{
					_ChunkSize = _TempSize;
                    _ChunkSizeIsCustom = true;
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
                _OutFile = Path.GetDirectoryName(_FilePath) + @"\" +
                    Path.GetFileNameWithoutExtension(_FilePath) + @".sql";
			}
            if (!_ChunkSizeIsCustom)
            {
                string _CustomChunkSizeString;
                int _CustomChunkSizeInt;
                bool _GotValidAnswer = false;

                while (!_GotValidAnswer)
                {
                    Console.Write("Chunk Size [ {0} ]: ", _ChunkSize);
                    _CustomChunkSizeString = Console.ReadLine();
                    if (String.IsNullOrWhiteSpace(_CustomChunkSizeString))
                    {
                        _GotValidAnswer = true;
                    }
                    else
                    {
                        if (int.TryParse(_CustomChunkSizeString, out _CustomChunkSizeInt))
                        {
                            if (_CustomChunkSizeInt > 0)
                            {
                                _GotValidAnswer = true;
                                _ChunkSize = _CustomChunkSizeInt;
                                _ChunkSizeIsCustom = true; // not used below, but don't leave in inconsistent state
                            }
                            else
                            {
                                DisplayError("ChunkSize must be > 0.");
                            }
                        }
                        else
                        {
                            DisplayError("ChunkSize must be a number.");
                        }
                    } /* if (String.IsNullOrWhiteSpace(_CustomChunkSizeString)) ... else */
                }
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

                                _DataOut.WriteLine(String.Empty);
							}
						}
					}
				}
			}
			catch (Exception _Exception)
			{
                DisplayError(_Exception.Message);
                Console.WriteLine("\n\tPress the <any> key to continue...\n");
				Console.ReadLine();
			}
			finally
			{
                if (_DataOut != null)
                {
                    _DataOut.Dispose();
                }
                if (_FileOut != null)
                {
                    _FileOut.Dispose();
                }
                if (_DataIn != null)
                {
                    _DataIn.Dispose();
                }
                if (_FileIn != null)
                {
                    _FileIn.Dispose();
                }
			}

			
		}

        private static void DisplayUsage()
        {
            System.Console.WriteLine("\nBinaryFormatter");
            System.Console.WriteLine("Version {0}",
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(3));
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
            System.Console.WriteLine("\tDefault ChunkSize = 10000");
            System.Console.WriteLine("\tDefault OutputFile = {path\\to\\binary_file_name}.sql\n");
            return;
        }

        private static void DisplayError(string ErrorMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ErrorMessage);
            Console.ResetColor();

            return;
        }
    }
}
