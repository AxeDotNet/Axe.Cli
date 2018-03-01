namespace Axe.Cli.Parser.Tokenizer
{
    interface ICliOptionToken
    {
        ICliOptionDefinition Definition { get; }
        string Value { get; }
    }
}