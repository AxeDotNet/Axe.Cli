using System.Collections.Generic;

namespace Axe.Cli.Parser
{
    interface ICommandDefinition : ICommandDefinitionMetadata
    {
        bool IsConflict(ICommandDefinition commandDefinition);
        bool IsMatch(string argument);
        void RegisterOption(IOptionDefinition option);
        void RegisterFreeValue(IFreeValueDefinition freeValue);
        IEnumerable<IOptionDefinition> GetRegisteredOptions();
        IEnumerable<IFreeValueDefinition> GetRegisteredFreeValues();
    }
}