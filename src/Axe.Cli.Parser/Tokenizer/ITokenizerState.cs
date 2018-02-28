namespace Axe.Cli.Parser.Tokenizer
{
    interface ITokenizerState
    {
        ITokenizerState MoveToNext(string argument);
    }
}