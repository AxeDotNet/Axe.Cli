namespace Axe.Cli.Parser.Tokenizer
{
    interface ICliOptionToken
    {
        ICliOptionDefinition Definition { get; set; }
        object Value { get; }
    }
}