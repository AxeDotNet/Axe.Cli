using System;
using System.Diagnostics;

namespace Axe.Cli.Parser.Tokenizer
{
    class WaitingValueWithCommandState : TokenizerStateBase
    {
        readonly ICliCommandDefinition command;
        readonly ICliOptionDefinition kvOption;
        readonly string labelArgument;
        readonly TokenizedResultBuilder resultBuilder;

        public WaitingValueWithCommandState(
            ICliCommandDefinition command,
            ICliOptionDefinition kvOption,
            string labelArgument,
            TokenizedResultBuilder resultBuilder)
        {
            Debug.Assert(command != null);
            Debug.Assert(kvOption != null);
            Debug.Assert(resultBuilder != null);

            this.command = command;
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

            resultBuilder.AppendOptionToken(new CliOptionToken(kvOption, argument), $"{labelArgument} {argument}");
            return new ContinueWithCommandState(command, resultBuilder);
        }
    }
}