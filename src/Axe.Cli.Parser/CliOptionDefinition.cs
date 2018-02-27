using System;

namespace Axe.Cli.Parser
{
    class CliOptionDefinition
    {
        public CliOptionDefinition(
            string symbol,
            char? abbreviation,
            string description,
            bool isRequired = false,
            OptionType type = OptionType.KeyValue)
        {
            Symbol = new OptionSymbol(symbol, abbreviation);
            Description = description;
            IsRequired = isRequired;
            Type = type;
        }

        public OptionSymbol Symbol { get; }
        public string Description { get; }
        public bool IsRequired { get; }
        public OptionType Type { get; }

        public bool IsConflict(CliOptionDefinition optionDefinition)
        {
            if (optionDefinition == null) { throw new ArgumentNullException(nameof(optionDefinition)); }
            return Symbol.IsConflict(optionDefinition.Symbol);
        }

        public override string ToString()
        {
            string symbolString = Symbol.ToString();
            string required = IsRequired ? "required" : "optional";
            return $"{symbolString}; {required}; {Type}";
        }
    }
}