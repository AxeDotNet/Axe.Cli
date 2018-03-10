using System.Collections.Generic;

namespace Axe.Cli.Parser
{
    class FreeValue : TransformedValue<string, IList<object>>
    {
        public FreeValue(string raw, IList<object> transformed) : base(raw, transformed)
        {
        }
    }
}