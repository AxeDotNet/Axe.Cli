using System;
using System.Diagnostics;

namespace Axe.Cli.Parser.Tokenizer
{
    class ContinueWithCommandState : TokenizerStateBase
    {
        readonly ICliCommandDefinition command;
        readonly TokenizedResultBuilder resultBuilder;

        public ContinueWithCommandState(ICliCommandDefinition command, TokenizedResultBuilder resultBuilder)
        {
            Debug.Assert(command != null);
            Debug.Assert(resultBuilder != null);

            this.command = command;
            this.resultBuilder = resultBuilder;
        }

        public override ITokenizerState MoveToNext(string argument)
        {
            if (IsEndOfArguments(argument)) { return null; }
            throw new NotImplementedException();
        }
    }
}