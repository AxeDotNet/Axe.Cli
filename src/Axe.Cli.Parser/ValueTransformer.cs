using System;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Cli.Parser
{
    /// <summary>
    /// Representing the base class for a value tranformer. A value transformer is used to
    /// translate command line string value to a specified type.
    /// </summary>
    public abstract class ValueTransformer
    {
        /// <summary>
        /// The first pass of the translation is the split process. It will split the argument
        /// to multiple ones if needed. If this method fails it should throw an exception.
        /// If you want to provide additional information, you can throw a
        /// <see cref="ArgParsingException"/>.
        /// </summary>
        /// <param name="argument">The argument to split.</param>
        /// <returns>The splitted argument collection.</returns>
        protected abstract IList<string> SplitArgument(string argument);

        /// <summary>
        /// The second pass of the translation is the transform process. It will translate
        /// each splitted element into specified type. If this method fails it should throw an
        /// exception. If you want to provide additional information, you can throw a
        /// <see cref="ArgParsingException"/>.
        /// </summary>
        /// <param name="argument">A single splitted argument.</param>
        /// <returns>The translated value.</returns>
        protected abstract object TransformSingleArgument(string argument);

        /// <summary>
        /// Split and translate input values into specified type.
        /// </summary>
        /// <param name="arguments">The input values</param>
        /// <returns>The splitted and translated result.</returns>
        public IList<object> Transform(IList<string> arguments)
        {
            if (arguments == null) { return Array.Empty<object>(); }
            return arguments
                .SelectMany(SplitArgument)
                .Select(TransformSingleArgument)
                .ToArray();
        }

        /// <summary>
        /// Split and translate input value into specified type.
        /// </summary>
        /// <param name="argument">The input values</param>
        /// <returns>The splitted and translated result.</returns>
        public IList<object> Transform(string argument)
        {
            if (string.IsNullOrEmpty(argument)) { return Array.Empty<object>(); }
            return SplitArgument(argument)
                .Select(TransformSingleArgument)
                .ToArray();
        }
    }
}