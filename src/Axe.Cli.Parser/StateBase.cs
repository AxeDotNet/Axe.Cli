using System.Collections.Generic;
using System.Linq;

namespace Axe.Cli.Parser
{
    abstract class StateBase : IParsingState
    {
        public IParsingState HandleInput(ParseResultBuilder builder, string input)
        {
            if (SymbolDefinition.IsEndOfArgument(input)) return HandleEoAInput(builder);
            if (SymbolDefinition.IsAbbrFlags(input)) return HandleAbbrFlagsInput(builder, GetAbbrLabels(input));
            if (SymbolDefinition.IsFullLabel(input)) return HandleFullLabelInput(builder, input);
            if (SymbolDefinition.IsAbbrLabel(input)) return HandleAbbrLabelInput(builder, input);
            return HandleOtherInput(builder, input);
        }

        static IEnumerable<string> GetAbbrLabels(string input)
        {
            return input.Skip(1).Select(c => $"-{c}");
        }

        protected abstract IParsingState HandleEoAInput(ParseResultBuilder builder);

        protected abstract IParsingState HandleAbbrFlagsInput(
            ParseResultBuilder builder,
            IEnumerable<string> abbrFlags);

        protected abstract IParsingState HandleFullLabelInput(
            ParseResultBuilder builder, string fullLabel);

        protected abstract IParsingState HandleAbbrLabelInput(
            ParseResultBuilder builder, string abbrLabel);

        protected abstract IParsingState HandleOtherInput(
            ParseResultBuilder builder, string input);
    }
}