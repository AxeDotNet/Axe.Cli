using System;
using System.Collections.Generic;

namespace Axe.Cli.Parser
{
    public interface ICliCommandDefinition : IEquatable<ICliCommandDefinition>
    {
        Guid Id { get; }

        string Symbol { get; }
        string Description { get; }
        
        bool IsConflict(ICliCommandDefinition commandDefinition);
        bool IsMatch(string argument);
        IReadOnlyList<ICliOptionDefinition> GetRegisteredOptions();
        void RegisterOption(ICliOptionDefinition option);
        string ToString();
    }
}