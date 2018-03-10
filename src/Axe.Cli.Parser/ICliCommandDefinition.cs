using System.Collections.Generic;

namespace Axe.Cli.Parser
{
    public interface ICliCommandDefinition
    {
        string Symbol { get; }
        string Description { get; }
        bool AllowFreeValue { get; }
        
        bool IsConflict(ICliCommandDefinition commandDefinition);
        bool IsMatch(string argument);
        IReadOnlyList<ICliOptionDefinition> GetRegisteredOptions();
        void RegisterOption(ICliOptionDefinition option);
        string ToString();
    }
}