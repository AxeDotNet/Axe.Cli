using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Axe.Cli.Parser.Tokenizer
{
    class TokenizedResultBuilder
    {
        ICliCommandDefinition command;
        readonly IList<ICliOptionToken> tokens = new List<ICliOptionToken>();

        public void SetCommand(ICliCommandDefinition commandDefinition)
        {
            Debug.Assert(commandDefinition != null);

            if (command != null)
            {
                throw new InvalidOperationException("The command has been set.");
            }
            
            command = commandDefinition;
        }

        public void AppendOptionToken(ICliOptionToken optionToken)
        {
            Debug.Assert(optionToken != null);
            tokens.Add(optionToken);
        }

        public TokenizedResult Build()
        {
            return new TokenizedResult(command, tokens);
        }
    }
}