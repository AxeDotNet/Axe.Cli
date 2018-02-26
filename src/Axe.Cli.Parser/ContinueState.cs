using System;
using System.Collections.Generic;

namespace Axe.Cli.Parser
{
    class ContinueState : StateBase
    {
        protected override IParsingState HandleEoAInput(ParseResultBuilder builder)
        {
            return new StopState();
        }

        protected override IParsingState HandleAbbrFlagsInput(ParseResultBuilder builder, IEnumerable<string> abbrFlags)
        {
            throw new NotImplementedException();
        }

        protected override IParsingState HandleFullLabelInput(ParseResultBuilder builder, string fullLabel)
        {
            throw new NotImplementedException();
        }

        protected override IParsingState HandleAbbrLabelInput(ParseResultBuilder builder, string abbrLabel)
        {
            throw new NotImplementedException();
        }

        protected override IParsingState HandleOtherInput(ParseResultBuilder builder, string input)
        {
            throw new NotImplementedException();
        }
    }
}