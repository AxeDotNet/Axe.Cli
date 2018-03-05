namespace Axe.Cli.Parser.Tokenizer
{
    class ContinueFreeValueState : PreParsingStateBase
    {
        readonly ICliCommandDefinition command;
        readonly PreParserResultBuilder resultBuilder;

        public ContinueFreeValueState(ICliCommandDefinition command, PreParserResultBuilder resultBuilder)
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