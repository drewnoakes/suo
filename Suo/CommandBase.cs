// Copyright (c) Drew Noakes. All Rights Reserved. Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

using OpenMcdf;

namespace Suo
{
    internal abstract class CommandBase : ICommand
    {
        private static readonly Regex _optionRegex = new Regex(@"^--(?<key>\w+)$");
        private static readonly Regex _optionWithValueRegex = new Regex(@"^--(?<key>\w+)=(?<value>\S*)$");

        public abstract string Argument { get; }
        public abstract string ShortDescription { get; }
        public abstract string CommandLineSyntax { get; }

        public bool Initialize(IEnumerable<string> args)
        {
            foreach (var arg in args)
            {
                var match = _optionRegex.Match(arg);
                if (match.Success)
                {
                    if (!TryProcessFlag(match.Groups["key"].Value))
                        return false;
                    continue;
                }

                match = _optionWithValueRegex.Match(arg);
                if (match.Success)
                {
                    if (!TryProcessFlagWithValue(match.Groups["key"].Value, match.Groups["value"].Value))
                        return false;
                    continue;
                }

                if (!TryProcessArgument(arg))
                    return false;
            }

            return true;
        }

        public abstract bool Execute();

        protected virtual bool TryProcessFlag(string key)
        {
            Console.Error.WriteLine($"Unsupported flag \"{key}\"");
            return false;
        }

        protected virtual bool TryProcessFlagWithValue(string key, string value)
        {
            Console.Error.WriteLine($"Unsupported flag \"{key}\"");
            return false;
        }

        protected virtual bool TryProcessArgument(string arg)
        {
            Console.Error.WriteLine($"Unexpected argument \"{arg}\"");
            return false;
        }

        protected static bool VisitFile(string? path, Action<CFStream> visitItem)
        {
            if (path == null)
            {
                Console.Error.WriteLine("Must specify a SUO file path.");
                return false;
            }

            // If the path is relative, make it absolute with respect to the current directory.
            path = Path.Combine(Environment.CurrentDirectory, path);

            if (!File.Exists(path))
            {
                Console.Error.WriteLine($"Cannot open file {path}");
                return false;
            }

            using (var cf = new CompoundFile(path))
            {
                cf.RootStorage.VisitEntries(VisitItem, recursive: false);
            }

            return true;

            void VisitItem(CFItem item)
            {
                // NOTE SUO files are a flat list -- top level items are always streams
                if (item is CFStream stream)
                    visitItem(stream);
                else
                    Debug.Fail("Encountered a top level item which was not a stream");
            }
        }
    }
}