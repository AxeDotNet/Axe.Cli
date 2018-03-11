using System;

namespace Axe.Cli.Parser.Tokenizer
{
    class OptionToken : IOptionToken
    {
        public OptionToken(IOptionDefinition definition)
        {
            if (definition.Type != OptionType.Flag)
            {
                throw new ArgumentException(
                    $"This constructor is for flag definition only. While current is {definition.Type}");
            }

            Definition = definition;
            Value = null;
        }

        public OptionToken(IOptionDefinition definition, string value)
        {
            if (definition.Type != OptionType.KeyValue)
            {
                throw new ArgumentException(
                    $"This constructor is for key-value definition only. While current is {definition.Type}.");
            }

            Definition = definition;
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
        
        public IOptionDefinition Definition { get; }
        public string Value { get; }
    }
}