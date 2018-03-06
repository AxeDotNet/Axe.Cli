using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Axe.Cli.Parser.Transformers
{
    class IntegerTransformer : IValueTransformer
    {
        public IList<object> Transform(IList<string> values)
        {
            if (values.Count == 0) { return Array.Empty<object>(); }

            return values.Select(
                v =>
                {
                    if (!int.TryParse(v, NumberStyles.Integer, CultureInfo.InvariantCulture, out int result))
                    {
                        throw new CliArgParsingException(
                            CliArgsParsingErrorCode.TransformIntegerValueFailed, v);
                    }

                    return result;
                })
                .Cast<object>()
                .ToArray();
        }
    }
}