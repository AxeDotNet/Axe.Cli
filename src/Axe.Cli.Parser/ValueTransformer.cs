using System;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Cli.Parser
{
    public abstract class ValueTransformer
    {
        protected abstract IList<string> SplitArgument(string argument);
        protected abstract object TransformSingleArgument(string argument);

        public IList<object> Transform(IList<string> arguments)
        {
            if (arguments == null) { return Array.Empty<object>(); }
            return arguments
                .SelectMany(SplitArgument)
                .Select(TransformSingleArgument)
                .ToArray();
        }

        public IList<object> Transform(string argument)
        {
            if (string.IsNullOrEmpty(argument)) { return Array.Empty<object>(); }
            return SplitArgument(argument)
                .Select(TransformSingleArgument)
                .ToArray();
        }
    }
}