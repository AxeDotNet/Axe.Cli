using System.Diagnostics;

namespace Axe.Cli.Parser.Tokenizer
{
    class WaitingValueState : PreParsingStateBase
    {
        readonly ICommandDefinition command;
        readonly IOptionDefinition kvOption;
        readonly string labelArgument;
        readonly PreParserResultBuilder resultBuilder;

        public WaitingValueState(
            ICommandDefinition command,
            IOptionDefinition kvOption,
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
                throw new ArgParsingException(ArgsParsingErrorCode.CannotFindValueForOption, labelArgument);
            }

            resultBuilder.AppendOptionToken(new OptionToken(kvOption, argument), $"{labelArgument} {argument}");
            return new ContinueState(command, resultBuilder);
        }
    }
}