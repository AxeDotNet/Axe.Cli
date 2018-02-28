using System;
using System.Diagnostics;

namespace Axe.Cli.Parser.Tokenizer
{
    class WaitingValueWithCommandState : TokenizerStateBase
    {
        readonly ICliCommandSymbolDefinition defaultCommand;
        readonly ICliOptionDefinition kvOption;
        readonly string labelArgument;
        readonly TokenizedResultBuilder resultBuilder;

        public WaitingValueWithCommandState(
            ICliCommandSymbolDefinition defaultCommand,
            ICliOptionDefinition kvOption,
            string labelArgument,
            TokenizedResultBuilder resultBuilder)
        {
            Debug.Assert(defaultCommand != null);
            Debug.Assert(kvOption != null);
            Debug.Assert(resultBuilder != null);

            this.defaultCommand = defaultCommand;
            this.kvOption = kvOption;
            this.labelArgument = labelArgument;
            this.resultBuilder = resultBuilder;
        }

        public override ITokenizerState MoveToNext(string argument)
        {
            if (IsEndOfArguments(argument))
            {
                throw new CliArgParsingException(CliArgsParsingErrorCode.CannotFindValueForOption, labelArgument);
            }

            throw new NotImplementedException();
        }
    }
}