using System;
using Axe.Cli.Parser.Extensions;
using Axe.Cli.Parser.Transformers;

namespace Axe.Cli.Parser
{
    class OptionDefinition : IOptionDefinition
    {
        public OptionDefinition(
            string symbol,
            char? abbreviation,
            string description,
            bool isRequired = false,
            OptionType type = OptionType.KeyValue,
            ValueTransformer transformer = null)
        {
            Symbol = new OptionSymbol(symbol, abbreviation);
            Description = description ?? string.Empty;
            IsRequired = isRequired;
            Type = type;
            Transformer = transformer ?? DefaultTransformer.Instance;
        }

        public Guid Id { get; } = Guid.NewGuid();
        public IOptionSymbol Symbol { get; }
        public IOptionSymbolMetadata SymbolMetadata => Symbol;
        public string Description { get; }
        public bool IsRequired { get; }
        public OptionType Type { get; }
        public ValueTransformer Transformer { get; }

        public bool IsConflict(IOptionDefinition optionDefinition)
        {
            return Symbol.IsConflict(optionDefinition.Symbol);
        }

        public bool Equals(IOptionDefinition other)
        {
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return Equals((IOptionDefinition) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            string symbolString = Symbol.ToString();
            return IsRequired ? $"{symbolString}" : $"[{symbolString}]";
        }

        public bool IsMatch(string argument)
        {
            return Symbol.IsMatch(argument);
        }
    }
}