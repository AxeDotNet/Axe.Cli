﻿using System;
using System.Diagnostics;

namespace Axe.Cli.Parser.TokenizerStates
{
    class ContinueWithCommandState : TokenizerStateBase
    {
        readonly CliCommandDefinition command;
        readonly TokenizedResultBuilder resultBuilder;

        public ContinueWithCommandState(CliCommandDefinition command, TokenizedResultBuilder resultBuilder)
        {
            Debug.Assert(command != null);
            Debug.Assert(resultBuilder != null);

            this.command = command;
            this.resultBuilder = resultBuilder;
        }

        public override ITokenizerState MoveToNext(string argument)
        {
            if (argument == null) { return null; }
            throw new NotImplementedException();
        }
    }
}