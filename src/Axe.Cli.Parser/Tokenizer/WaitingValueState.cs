using System;
using System.Diagnostics;

namespace Axe.Cli.Parser.Tokenizer
{
    class WaitingValueState : PreParsingStateBase
    {
        readonly ICliCommandDefinition command;
        readonly ICliOptionDefinition kvOption;
        readonly string labelArgument;
        readonly PreParserResultBuilder resultBuilder;

        public WaitingValueState(
            ICliCommandDefinition command,
            ICliOptionDefinition kvOption,
            string labelArgument,
            PreParserResultBuilder resultBuilder)
        {
            Debug.Assert(command != null);
            Debug.Assert(kvOption != null);
            Debug.Assert(resultBuilder != null);

            this.command = command;
            this.kvOption = kvOption;
            this.labelArgument = labelArgument;
            this.resultBuilder = resultBuilder;
        }

        public override IPreParsingState MoveToNext(string argument)
        {
            if (IsEndOfArguments(argument))
            {
                throw new CliArgParsingException(CliArgsParsingErrorCode.CannotFindValueForOption, labelArgument);
            }

            resultBuilder.AppendOptionToken(new CliOptionToken(kvOption, argument), $"{labelArgument} {argument}");
            return new ContinueState(command, resultBuilder);
        }
    }
}