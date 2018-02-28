using System.Collections.Generic;

namespace Axe.Cli.Parser
{
    interface ICliOptionDefinitionContainer
    {
        IReadOnlyList<CliOptionDefinition> GetRegisteredOptions();
        void RegisterOption(CliOptionDefinition option);
    }
}