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
using System.IO;

namespace SqlQuantumLeap.BinaryFormatter
{
    internal class Configuration
    {
        private int _ChunkSize = 10000;
        private bool _ChunkSizeIsCustom = false;
        private string _FilePath = String.Empty;
        private string _OutFile = String.Empty;
        private bool _SaveToClipboard = false;
        private bool _SendToConsole = false;
        private bool _NoOutputFile = false;


        public int ChunkSize
        {
            get
            {
                return _ChunkSize;
            }
        }
        public bool ChunkSizeIsCustom
        {
            get
            {
                return _ChunkSizeIsCustom;
            }
        }
        public string FilePath
        {
            get
            {
                return _FilePath;
            }
        }
        public string OutFile
        {
            get
            {
                return _OutFile;
            }
        }
        public bool SaveToClipboard
        {
            get
            {
                return _SaveToClipboard;
            }
        }
        public bool SendToConsole
        {
            get
            {
                return _SendToConsole;
            }
        }
        public bool NoOutputFile
        {
            get
            {
                return _NoOutputFile;
            }
        }


        public Configuration(string[] InputParameters)
        {
            foreach (string _Arg in InputParameters)
            {
                int _TempSize;

                if (!_ChunkSizeIsCustom // Skip test if already found
                    && int.TryParse(_Arg, out _TempSize))
                {
                    _ChunkSize = _TempSize;
                    _ChunkSizeIsCustom = true;
                }
                else
                {
                    switch (_Arg.ToUpperInvariant())
                    {
                        case "-CLIPBOARD":
                        case "/CLIPBOARD":
                            _SaveToClipboard = true;
                            break;

                        case "-CONSOLE":
                        case "/CONSOLE":
                            _SendToConsole = true;
                            break;

                        case "-NOFILE":
                        case "/NOFILE":
                            _NoOutputFile = true;
                            break;

                        default:
                            if (_FilePath == String.Empty)
                            {
                                _FilePath = _Arg;
                            }
                            else if (_OutFile == String.Empty)
                            {
                                _OutFile = _Arg;
                            }
                            break;
                    } /* switch() */
                } /* if (!_ChunkSizeIsCustom... */
            } /* foreach(string _Arg... */


            if (_FilePath == String.Empty)
            {
                throw new ArgumentNullException("You must specify a source file.");
            }


            // Set default output file name
            if (!_NoOutputFile // Skip if user requested no output file
                && _OutFile == String.Empty)
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
                                Display.Error("ChunkSize must be > 0.");
                            }
                        }
                        else
                        {
                            Display.Error("ChunkSize must be a number.");
                        }
                    } /* if (String.IsNullOrWhiteSpace(_CustomChunkSizeString)) ... else */
                } /* while (!_GotValidAnswer) */
            } /* if (!_ChunkSizeIsCustom) */

            return;
        }
    }
}
