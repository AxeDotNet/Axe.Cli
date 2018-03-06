using System.Collections.Generic;

namespace Axe.Cli.Parser
{
    class OptionValue
    {
        public OptionValue(IList<string> rawValues, IList<object> transformedValues)
        {
            RawValues = rawValues;
            TransformedValues = transformedValues;
        }

        public IList<string> RawValues { get; }
        public IList<object> TransformedValues { get; }
    }
}