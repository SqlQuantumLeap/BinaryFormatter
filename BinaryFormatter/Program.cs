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
using System.Text;
using System.IO;
using System.Windows;

namespace SqlQuantumLeap.BinaryFormatter
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.Title = "Binary Formatter ( by Solomon Rutzky / Sql Quantum Leap -- https://SqlQuantumLeap.com/ )";

            if (args.Length < 1 ||
                (args.Length == 1 &&
                    "|/?|-?|/help|-help|"
                        .IndexOf("|" + args[0] + "|", StringComparison.InvariantCultureIgnoreCase) > -1)
                )
            {
                Display.Usage();

                return;
            }

            Configuration _Config = new Configuration(args);


            FileStream _FileIn = null;
            BinaryReader _DataIn = null;
            FileStream _FileOut = null;
            StreamWriter _DataOut = null;
            StringBuilder _CurrentLine = new StringBuilder(String.Empty, (_Config.ChunkSize * 2)
                + 10); // add 10 for good measure, but should only need 2: backslash + newline
            StringBuilder _AllLines = null;

            try
            {
                using (_FileIn = new FileStream(_Config.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (_DataIn = new BinaryReader(_FileIn))
                    {
                        if (_Config.SaveToClipboard)
                        {
                            _AllLines = new StringBuilder(((int)_DataIn.BaseStream.Length * 2)
                                + (((int)_DataIn.BaseStream.Length / _Config.ChunkSize) * 2)
                                + 10); // add 10 for good measure
                        }

                        if (!_Config.NoOutputFile)
                        {
                            _FileOut = new FileStream(_Config.OutFile, FileMode.Create, FileAccess.Write, FileShare.Read);
                            _DataOut = new StreamWriter(_FileOut, Encoding.ASCII);
                        }

                        _DataIn.BaseStream.Position = 0;

                        byte[] _Bytes = new byte[_Config.ChunkSize];

                        int _ReadAmount = 0;

                        while ((_ReadAmount = _DataIn.Read(_Bytes, 0, _Config.ChunkSize)) > 0)
                        {
                            for (int _Index = 0; _Index < _ReadAmount; _Index++)
                            {
                                _CurrentLine.AppendFormat("{0:X2}", _Bytes[_Index]);
                            }

                            if (_DataIn.BaseStream.Position < _DataIn.BaseStream.Length)
                            {
                                _CurrentLine.AppendLine(@"\");

                                if (_Config.SendToConsole)
                                {
                                    Console.Write(_CurrentLine.ToString());
                                }
                                if (!_Config.NoOutputFile)
                                {
                                    _DataOut.Write(_CurrentLine.ToString());
                                }
                                if (_Config.SaveToClipboard)
                                {
                                    _AllLines.Append(_CurrentLine.ToString());
                                }

                                _CurrentLine.Clear();
                            }
                        }

                        if (_Config.SendToConsole)
                        {
                            Console.WriteLine(_CurrentLine.ToString());
                        }
                        if (!_Config.NoOutputFile)
                        {
                            _DataOut.WriteLine(_CurrentLine.ToString());
                        }
                        if (_Config.SaveToClipboard)
                        {
                            _AllLines.AppendLine(_CurrentLine.ToString());

                            // Clipboard.SetText Method ( https://docs.microsoft.com/en-us/dotnet/api/system.windows.clipboard?view=netframework-4.5.2 )
                            Clipboard.SetText(_AllLines.ToString(), TextDataFormat.Text);
                        }

                    }
                }
            }
            catch (Exception _Exception)
            {
                Display.Error(_Exception.Message);
                Console.WriteLine("\n\tPress any key to continue...\n");
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

    }
}
