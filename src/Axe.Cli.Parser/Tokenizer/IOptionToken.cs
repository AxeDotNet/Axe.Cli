namespace Axe.Cli.Parser.Tokenizer
{
    interface IOptionToken
    {
        ICliOptionDefinition Definition { get; }
        string Value { get; }
    }
}