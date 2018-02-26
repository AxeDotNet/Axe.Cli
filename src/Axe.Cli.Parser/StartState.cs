using System;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Cli.Parser
{
    class StartState : StateBase
    {
        protected override IParsingState HandleEoAInput(ParseResultBuilder builder)
        {
            return new StopState();
        }

        protected override IParsingState HandleAbbrFlagsInput(
            ParseResultBuilder builder, IEnumerable<string> abbrFlags)
        {
            foreach (IntemediateResult result in abbrFlags.Select(IntemediateResult.CreateFlag))
            {
                builder.AppendResult(result);
            }

            return new ContinueState();
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
            builder.AppendResult(
                SymbolDefinition.IsPossibleArea(input)
                    ? IntemediateResult.CreatePossibleArea(input)
                    : IntemediateResult.CreateFreeValue(input));
            return new ContinueState();
        }
    }
}