using System.Collections.Generic;

namespace Axe.Cli.Parser
{
    public interface ICommandDefinitionMetadata
    {
        string Symbol { get; }
        string Description { get; }
        bool AllowFreeValue { get; }
        string ToString();

        IEnumerable<ICliOptionDefinition> GetRegisteredOptionsMetadata();
        IEnumerable<IFreeValueDefinitionMetadata> GetRegisteredFreeValuesMetadata();
    }
}