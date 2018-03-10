using System;

namespace Axe.Cli.Parser
{
    public interface ICliOptionDefinition : IEquatable<ICliOptionDefinition>
    {
        Guid Id { get; }

        ICliOptionSymbol Symbol { get; }
        string Description { get; }
        bool IsRequired { get; }
        OptionType Type { get; }
        ValueTransformer Transformer { get; }

        bool IsConflict(ICliOptionDefinition optionDefinition);
        string ToString();
        bool IsMatch(string argument);
    }
}