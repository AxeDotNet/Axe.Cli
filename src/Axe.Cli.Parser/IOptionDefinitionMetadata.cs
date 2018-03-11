using System;

namespace Axe.Cli.Parser
{
    public interface IOptionDefinitionMetadata
    {
        Guid Id { get; }
        string Description { get; }
        bool IsRequired { get; }
        OptionType Type { get; }
        ValueTransformer Transformer { get; }

        string ToString();
    }
}