using System.Collections.Generic;
using System.Diagnostics;

namespace Axe.Cli.Parser.Tokenizer
{
    class TokenizedResult
    {
        public TokenizedResult(ICliCommandDefinition command, IList<ICliOptionToken> tokens)
        {
            Debug.Assert(command != null);
            Debug.Assert(tokens != null);

            Command = command;
            Tokens = tokens;
        }

        public ICliCommandDefinition Command { get; }
        public IList<ICliOptionToken> Tokens { get; }
    }
}