// Copyright (c) Drew Noakes. All Rights Reserved. Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;

namespace Suo
{
    internal interface ICommand
    {
        string Argument { get; }

        string ShortDescription { get; }
        
        string CommandLineSyntax { get; }

        bool Initialize(IEnumerable<string> args);

        bool Execute();
    }
}