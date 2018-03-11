using System.Collections.Generic;

namespace Axe.Cli.Parser
{
    interface ICommandDefinition : ICommandDefinitionMetadata
    {
        bool IsConflict(ICommandDefinition commandDefinition);
        bool IsMatch(string argument);
        void RegisterOption(ICliOptionDefinition option);
        void RegisterFreeValue(IFreeValueDefinition freeValue);
        IEnumerable<ICliOptionDefinition> GetRegisteredOptions();
        IEnumerable<IFreeValueDefinition> GetRegisteredFreeValues();
    }
}