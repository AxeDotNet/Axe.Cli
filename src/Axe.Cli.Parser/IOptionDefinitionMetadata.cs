using System;

namespace Axe.Cli.Parser
{
    /// <summary>
    /// Represents the metadata of a key-value option or a flag definition.
    /// </summary>
    public interface IOptionDefinitionMetadata
    {
        /// <summary>
        /// The identity of the definition. This value is used internally and it is not mean to be
        /// used in your code.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Get full form / abbreviation form of option key.
        /// </summary>
        IOptionSymbolMetadata SymbolMetadata { get; }

        /// <summary>
        /// The description of the free value. All the line breaks will be removed from the
        /// description for a consisitant console display.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Get a value indicating whether the option is mandatory. For flag. This value is always
        /// <c>false</c>.
        /// </summary>
        bool IsRequired { get; }

        /// <summary>
        /// Get the type of the definition.
        /// </summary>
        OptionType Type { get; }

        /// <summary>
        /// Get the transformer binded with the definition.
        /// </summary>
        ValueTransformer Transformer { get; }

        /// <summary>
        /// Get a meaningful string representation of the definition. This is mainly used for
        /// diagnostic purpose.
        /// </summary>
        /// <returns>The string representing current definition.</returns>
        string ToString();
    }
}