using System.Collections.Generic;

namespace Axe.Cli.Parser
{
    interface ICliCommandDefinition
    {
        IReadOnlyList<CliOptionDefinition> GetRegisteredOptions();
        void RegisterOption(CliOptionDefinition option);
        bool IsConflict(ICliCommandDefinition commandDefinition);
        string ToString();
    }
}