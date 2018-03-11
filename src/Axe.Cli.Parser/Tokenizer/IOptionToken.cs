namespace Axe.Cli.Parser.Tokenizer
{
    interface IOptionToken
    {
        IOptionDefinition Definition { get; }
        string Value { get; }
    }
}