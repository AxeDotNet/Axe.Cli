using System;
using System.Collections.Generic;

namespace Axe.Cli.Parser
{
    class ParseResultBuilder
    {
        bool isBuildComplete;
        readonly List<IntemediateResult> intemediateResult = new List<IntemediateResult>();
        
        public IParseResult Build()
        {
            var result = new ParseResult(intemediateResult);
            isBuildComplete = true;
            return result;
        }

        public void AppendResult(IntemediateResult result)
        {
            EnsureNotBuild();
            intemediateResult.Add(result);
        }

        void EnsureNotBuild()
        {
            if (!isBuildComplete) { return; }
            throw new InvalidOperationException("Cannot update parsing result after build completed.");
        }
    }
}