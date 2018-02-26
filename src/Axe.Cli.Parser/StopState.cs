using System;

namespace Axe.Cli.Parser
{
    class StopState : IParsingState
    {
        public IParsingState HandleInput(ParseResultBuilder builder, string input)
        {
            throw new NotSupportedException("The parsing process has ended.");
        }
    }
}