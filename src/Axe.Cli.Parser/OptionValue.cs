using System.Collections.Generic;

namespace Axe.Cli.Parser
{
    class OptionValue : TransformedValue<IList<string>, IList<object>>
    {
        public OptionValue(IList<string> raw, IList<object> transformed) : base(raw, transformed)
        {
        }
    }
}