namespace Axe.Cli.Parser.Tokenizer
{
    class ContinueFreeValueState : PreParsingStateBase
    {
        readonly ICommandDefinition command;
        readonly PreParserResultBuilder resultBuilder;

        public ContinueFreeValueState(ICommandDefinition command, PreParserResultBuilder resultBuilder)
        {
            this.command = command;
            this.resultBuilder = resultBuilder;
        }

        public override IPreParsingState MoveToNext(string argument)
        {
            if (IsEndOfArguments(argument)) { return null; }
            return HandleFreeValueArgument(command, resultBuilder, argument);
        }
    }
}