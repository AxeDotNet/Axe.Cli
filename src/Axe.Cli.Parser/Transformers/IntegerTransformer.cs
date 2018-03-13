using System.Globalization;

namespace Axe.Cli.Parser.Transformers
{
    /// <inheritdoc />
    /// <summary>
    /// Represents an 32-bit signed integer value transformer.
    /// </summary>
    public sealed class IntegerTransformer : SingleValueTransformer
    {
        /// <inheritdoc />
        /// <summary>
        /// Translate argument as a 32-bit signed integer.
        /// </summary>
        /// <param name="argument">The user input string.</param>
        /// <returns>The translated integer.</returns>
        /// <exception cref="T:Axe.Cli.Parser.ArgParsingException">
        /// Fail to convert string argument to integer.
        /// </exception>
        protected override object TransformSingleArgument(string argument)
        {
            if (!int.TryParse(argument, NumberStyles.Integer, CultureInfo.InvariantCulture, out int result))
            {
                throw new ArgParsingException(
                    ArgsParsingErrorCode.TransformIntegerValueFailed, argument);
            }

            return result;
        }
        
        /// <summary>
        /// Get the default instance of <see cref="IntegerTransformer"/> since it has no state.
        /// </summary>
        public static ValueTransformer Instance { get; } = new IntegerTransformer();
    }
}