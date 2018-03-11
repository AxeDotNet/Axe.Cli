using System.Collections.Generic;

namespace Axe.Cli.Parser
{
    /// <summary>
    /// Represents the metadata of a command definition.
    /// </summary>
    public interface ICommandDefinitionMetadata
    {
        /// <summary>
        /// Get the name of the command.
        /// </summary>
        string Symbol { get; }

        /// <summary>
        /// Get the description of the command.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Get a value indicating whether the command supports free value.
        /// </summary>
        bool AllowFreeValue { get; }

        /// <summary>
        /// Get a human readable string representation of current command definition.
        /// </summary>
        /// <returns>A string represents current object.</returns>
        string ToString();

        /// <summary>
        /// Get key-value and flag options definition metadata for this command.
        /// </summary>
        /// <returns>The key-value and flag options definition metadata collection.</returns>
        IEnumerable<IOptionDefinitionMetadata> GetRegisteredOptionsMetadata();

        /// <summary>
        /// Get free value definition metadata for this command.
        /// </summary>
        /// <returns>The free value definition metadata collection.</returns>
        IEnumerable<IFreeValueDefinitionMetadata> GetRegisteredFreeValuesMetadata();
    }
}