// Copyright (c) Drew Noakes. All Rights Reserved. Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;

using OpenMcdf;

namespace Suo
{
    internal sealed class ViewCommand : CommandBase
    {
        public override string Argument => "view";
        public override string ShortDescription => "Outputs keys and values";
        public override string CommandLineSyntax => "suo view --format=utf8|utf16|ascii|hex <key1> <key2> <file>";

        private enum Format { Utf8, Utf16, Ascii, Hex }

        private Format _format = Format.Hex;
        private readonly HashSet<string> _keys = new HashSet<string>();
        private string? _suoFile = null;

        protected override bool TryProcessFlagWithValue(string key, string value)
        {
            switch (key)
            {
                case "format":
                {
                    if (!Enum.TryParse(value, ignoreCase: true, out Format format))
                    {
                        Console.Error.WriteLine($"Unsupported format \"{value}\".");
                        return false;
                    }

                    _format = format;
                    return true;
                }
            }

            return base.TryProcessFlagWithValue(key, value);
        }

        protected override bool TryProcessArgument(string arg)
        {
            if (_suoFile != null)
                _keys.Add(_suoFile);

            _suoFile = arg;
            return true;
        }

        public override bool Execute()
        {
            bool first = true;

            return VisitFile(_suoFile, VisitItem);

            void VisitItem(CFStream item)
            {
                if (_keys.Count != 0 && !_keys.Contains(item.Name))
                    return;

                if (first)
                {
                    first = false;
                }
                else
                {
                    Console.WriteLine();
                }

                var data = item.GetData();

                Console.WriteLine(item.Name);
                Console.WriteLine();
                if (data.Length == 0)
                    Console.WriteLine("<no data>");

                switch (_format)
                {
                    case Format.Ascii:
                        Console.WriteLine(Encoding.ASCII.GetString(data));
                        break;
                    case Format.Utf8:
                        Console.WriteLine(Encoding.UTF8.GetString(data));
                        break;
                    case Format.Utf16:
                        Console.WriteLine(Encoding.Unicode.GetString(data));
                        break;
                    case Format.Hex:
                        Console.WriteLine(HexDump.HexDump.Format(data));
                        break;
                }
            }
        }
    }
}