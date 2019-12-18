// Copyright (c) Drew Noakes. All Rights Reserved. Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;

using OpenMcdf;

namespace Suo
{
    internal sealed class KeysCommand : CommandBase
    {
        public override string Argument => "keys";
        public override string ShortDescription => "Outputs key names";
        public override string CommandLineSyntax => "suo keys --no-empty --show-size <file>";

        private bool _showEmpty = true;
        private bool _showSize = false;
        private string? _suoFile = null;

        protected override bool TryProcessFlag(string key)
        {
            switch (key)
            {
                case "no-empty":
                    _showEmpty = false;
                    return true;
                case "show-size":
                    _showSize = true;
                    return true;
                default:
                    return base.TryProcessFlag(key);
            }
        }

        protected override bool TryProcessArgument(string arg)
        {
            if (_suoFile == null)
            {
                _suoFile = arg;
                
                return true;
            }

            return base.TryProcessArgument(arg);
        }

        public override bool Execute()
        {
            return VisitFile(_suoFile, VisitItem);

            void VisitItem(CFStream item)
            {
                var data = item.GetData();

                if (_showEmpty || data.Length != 0)
                {
                    Console.Out.WriteLine(
                        _showSize
                            ? $"{item.Name} {data.Length:#,#0} byte{(data.Length == 0 ? "" : "s")}"
                            : item.Name);
                }
            }
        }
    }
}