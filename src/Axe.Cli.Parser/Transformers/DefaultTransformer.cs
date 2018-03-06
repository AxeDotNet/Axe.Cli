using System.Collections.Generic;
using System.Linq;

namespace Axe.Cli.Parser.Transformers
{
    class DefaultTransformer : IValueTransformer
    {
        public IList<object> Transform(IList<string> values)
        {
            return values.Cast<object>().ToArray();
        }
    }
}