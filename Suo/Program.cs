// Copyright (c) Drew Noakes. All Rights Reserved. Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Suo
{
    internal static class Program
    {
        private static readonly ICommand[] _commands =
        {
            new KeysCommand(),
            new ViewCommand()
            // new HelpCommand
        };

        public static IReadOnlyDictionary<string, ICommand> CommandByArgument = _commands.ToDictionary(f => f.Argument);

        private static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                PrintUsage();
                return 1;
            }

            if (!CommandByArgument.TryGetValue(args[0], out ICommand? command))
            {
                Console.Error.WriteLine($"Unknown command \"{args[0]}\".");
                Console.WriteLine();
                PrintUsage();
                return 1;
            }

            if (!command.Initialize(args.Skip(1)) || !command.Execute())
            {
                return 1;
            }

            return 0;

            static void PrintUsage()
            {
                Console.WriteLine("suo — inspects Visual Studio .suo files");
                Console.WriteLine();
                Console.WriteLine("Commands:");
                Console.WriteLine();
                foreach (var command in _commands)
                    Console.WriteLine($"    {command.Argument,-8} {command.ShortDescription}");
                Console.WriteLine();
                Console.WriteLine("Options:");
                Console.WriteLine();
                foreach (var command in _commands)
                    Console.WriteLine($"    {command.CommandLineSyntax}");
            }
        }
    }
}
