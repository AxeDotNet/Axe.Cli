using System;

namespace Axe.Cli.Parser
{
    /// <summary>
    /// Representing the definition of a free value. These definition may change the way the parser
    /// parsing the command.
    /// </summary>
    public interface IFreeValueDefinitionMetadata
    {
        /// <summary>
        /// The identity of the definition. This value is used internally and it is not mean to be
        /// used in your code.
        /// </summary>
        Guid Id { get; }
        
        /// <summary>
        /// Get the name of the free value definition. This name will be used to get the free value
        /// from the <see cref="ArgsParsingResult"/>.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The description of the free value. All the line breaks will be removed from the
        /// description for a consisitant console display.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// The value transformer to translate the string argument. If not set, the
        /// <see cref="ArgsTransformers.Default"/> will be used.
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