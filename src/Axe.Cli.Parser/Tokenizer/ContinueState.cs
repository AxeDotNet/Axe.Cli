using System.Diagnostics;

namespace Axe.Cli.Parser.Tokenizer
{
    class ContinueState : PreParsingStateBase
    {
        readonly ICommandDefinition command;
        readonly PreParserResultBuilder resultBuilder;

        public ContinueState(ICommandDefinition command, PreParserResultBuilder resultBuilder)
        {
            Debug.Assert(command != null);
            Debug.Assert(resultBuilder != null);

            this.command = command;
            this.resultBuilder = resultBuilder;
        }

        public override IPreParsingState MoveToNext(string argument)
        {
            if (IsEndOfArguments(argument)) { return null; }

            IPreParsingState handleKeyValueOptionState = HandleKeyValueOptionArgument(
                command,
                resultBuilder,
                argument);
            if (handleKeyValueOptionState != null) { return handleKeyValueOptionState; }

            IPreParsingState handleFlagOptionState = HandleFlagOptionArgument(
                command, resultBuilder, argument);
            if (handleFlagOptionState != null) { return handleFlagOptionState; }
            
            return HandleFreeValueArgument(command, resultBuilder, argument);
        }
    }
}