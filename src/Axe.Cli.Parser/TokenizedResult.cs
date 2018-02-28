using System.Collections.Generic;
using System.Diagnostics;

namespace Axe.Cli.Parser
{
    class TokenizedResult
    {
        public TokenizedResult(ICliCommandSymbolDefinition command, IList<ICliOptionToken> tokens)
        {
            Debug.Assert(command != null);
            Debug.Assert(tokens != null);

            Command = command;
            Tokens = tokens;
        }

        public ICliCommandSymbolDefinition Command { get; }
        public IList<ICliOptionToken> Tokens { get; }
    }
}