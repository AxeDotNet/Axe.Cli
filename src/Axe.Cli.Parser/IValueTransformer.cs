using System.Collections.Generic;

namespace Axe.Cli.Parser
{
    public interface IValueTransformer
    {
        IList<object> Transform(IList<string> values);
    }
}