using System;
using System.Collections.Generic;

namespace Axe.Cli.Parser.Transformers
{
    public abstract class SingleValueTransformer : ValueTransformer
    {
        protected sealed override IList<string> SplitArgument(string argument)
        {
            return argument == null ? Array.Empty<string>() : new[] {argument};
        }
    }
}