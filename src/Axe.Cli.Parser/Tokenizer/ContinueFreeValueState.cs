namespace Axe.Cli.Parser.Tokenizer
{
    class ContinueFreeValueState : TokenizerStateBase
    {
        readonly ICliCommandDefinition command;
        readonly TokenizedResultBuilder resultBuilder;

        public ContinueFreeValueState(ICliCommandDefinition command, TokenizedResultBuilder resultBuilder)
        {
            this.command = command;
            this.resultBuilder = resultBuilder;
        }

        public override ITokenizerState MoveToNext(string argument)
        {
            if (IsEndOfArguments(argument)) { return null; }
            return HandleFreeValueArgument(command, resultBuilder, argument);
        }
    }
}