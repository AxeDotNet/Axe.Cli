using System;
using System.Collections.Generic;

namespace Axe.Cli.Parser.Transformers
{
    /// <inheritdoc />
    /// <summary>
    /// Represnets a <see cref="T:Axe.Cli.Parser.ValueTransformer" /> that treat each string argument as one element.
    /// </summary>
    public abstract class SingleValueTransformer : ValueTransformer
    {
        /// <summary>
        /// Do nothing in the split pass. Just returns the one element collection.
        /// </summary>
        /// <param name="argument">The string argument the user inputs.</param>
        /// <returns>A single element collection that contains the user input argument.</returns>
        protected sealed override IList<string> SplitArgument(string argument)
        {
            return argument == null ? Array.Empty<string>() : new[] {argument};
        }
    }
}