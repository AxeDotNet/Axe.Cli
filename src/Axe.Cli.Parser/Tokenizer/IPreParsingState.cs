namespace Axe.Cli.Parser.Tokenizer
{
    interface IPreParsingState
    {
        IPreParsingState MoveToNext(string argument);
    }
}