using System.Globalization;

namespace Axe.Cli.Parser.Transformers
{
    class IntegerTransformer : SingleValueTransformer
    {
        protected override object TransformSingleArgument(string argument)
        {
            if (!int.TryParse(argument, NumberStyles.Integer, CultureInfo.InvariantCulture, out int result))
            {
                throw new CliArgParsingException(
                    CliArgsParsingErrorCode.TransformIntegerValueFailed, argument);
            }

            return result;
        }
    }
}