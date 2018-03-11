using System;

namespace Axe.Cli.Parser
{
    interface IFreeValueDefinition : IEquatable<IFreeValueDefinition>, IFreeValueDefinitionMetadata
    {
        bool IsConflict(IFreeValueDefinition freeValueDefinition);
        bool IsMatch(string name);
    }
}