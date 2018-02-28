using System;

namespace Axe.Cli.Parser.Tokenizer
{
    class CliOptionToken : ICliOptionToken
    {
        public CliOptionToken(ICliOptionDefinition definition, object value)
        {
            if (definition.Type == OptionType.Flag && !(value is bool))
            {
                throw new ArgumentException(
                    $"The definition is a flag while the value is '{value}'");
            }
            
            if (definition.Type == OptionType.KeyValue && !(value is string))
            {
                throw new ArgumentException(
                    $"The definition is a key-value and the value is '{value}'");
            }

            Definition = definition;
            Value = value;
        }
        
        public ICliOptionDefinition Definition { get; }
        public object Value { get; }
    }
}