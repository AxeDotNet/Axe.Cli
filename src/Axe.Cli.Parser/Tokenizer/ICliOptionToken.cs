using System.Diagnostics;

namespace Axe.Cli.Parser.Tokenizer
{
    interface ICliOptionToken
    {
        ICliOptionDefinition Definition { get; }
        object Value { get; }
    }
}