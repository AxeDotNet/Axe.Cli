using System;
using Axe.Cli.Parser.Extensions;

namespace Axe.Cli.Parser
{
    class CliOptionDefinition : ICliOptionDefinition
    {
        public CliOptionDefinition(
            string symbol,
            char? abbreviation,
            string description,
            bool isRequired = false,
            OptionType type = OptionType.KeyValue,
            ValueTransformer transformer = null)
        {
            Symbol = new OptionSymbol(symbol, abbreviation);
            Description = description.MakeSingleLine();
            IsRequired = isRequired;
            Type = type;
            Transformer = transformer ?? ArgsTransformers.Default;
        }

        public Guid Id { get; } = Guid.NewGuid();
        public ICliOptionSymbol Symbol { get; }
        public string Description { get; }
        public bool IsRequired { get; }
        public OptionType Type { get; }
        public ValueTransformer Transformer { get; }

        public bool IsConflict(ICliOptionDefinition optionDefinition)
        {
            return Symbol.IsConflict(optionDefinition.Symbol);
        }

        public bool Equals(ICliOptionDefinition other)
        {
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return Equals((ICliOptionDefinition) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            string symbolString = Symbol.ToString();
            string required = IsRequired ? "required" : "optional";
            return $"{symbolString}; {required}; {Type}";
        }

        public bool IsMatch(string argument)
        {
            return Symbol.IsMatch(argument);
        }
    }
}