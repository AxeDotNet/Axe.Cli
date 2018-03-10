using System.Collections.Generic;

namespace Axe.Cli.Parser
{
    class FreeValue
    {
        public FreeValue(string rawValue, IList<object> transformedValues)
        {
            RawValue = rawValue;
            TransformedValues = transformedValues;
        }

        public string RawValue { get; }
        public IList<object> TransformedValues { get; }
    }
}