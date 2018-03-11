using System;

namespace Axe.Cli.Parser
{
    interface IOptionDefinition : IEquatable<IOptionDefinition>, IOptionDefinitionMetadata
    {
        IOptionSymbol Symbol { get; }
        bool IsConflict(IOptionDefinition optionDefinition);
        bool IsMatch(string argument);
    }
}