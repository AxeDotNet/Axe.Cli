namespace Axe.Cli.Parser.TokenizerStates
{
    interface ITokenizerState
    {
        ITokenizerState MoveToNext(string argument);
    }
}