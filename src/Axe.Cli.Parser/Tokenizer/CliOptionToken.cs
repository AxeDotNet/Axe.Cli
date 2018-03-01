using System;

namespace Axe.Cli.Parser.Tokenizer
{
    class CliOptionToken : ICliOptionToken
    {
        public CliOptionToken(ICliOptionDefinition definition)
        {
            if (definition.Type != OptionType.Flag)
            {
                throw new ArgumentException(
                    $"This constructor is for flag definition only. While current is {definition.Type}");
            }

            Definition = definition;
            Value = null;
        }

        public CliOptionToken(ICliOptionDefinition definition, string value)
        {
            if (definition.Type == OptionType.Flag && value != null)
            {
                throw new ArgumentException(
                    $"The value for flag option should be null. While current is '{value}'");
            }
            
            if (definition.Type == OptionType.KeyValue && value == null)
            {
                throw new ArgumentNullException(
                    nameof(value),
                    "The value for key-value option cannot be null");
            }

            Definition = definition;
            Value = value;
        }
        
        public ICliOptionDefinition Definition { get; }
        public string Value { get; }
    }
}