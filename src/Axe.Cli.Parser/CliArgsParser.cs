using System.Collections.Generic;

namespace Axe.Cli.Parser
{
    public class CliArgsParser
    {
        /*
         * The parser uses the following symbol definitions (all patterns are case insensitive):
         * 
         * - Full Label: ^--[A-Z0-9_][A-Z0-9_\-]+$
         * - Abbr. Label: ^-[A-Z]$
         * - Abbr. Label Group: ^-[A-Z]{2,}$ (should not contains char duplication)
         * - Area: ^[A-Z0-9_][A-Z0-9_\-]+$ (must be the first element if presented)
         * 
         * And the state machine for parser is:
         * 
         * - (start) --Full Label--> (label received|waiting)
         * - (start) --Abbr. Label--> (abbr. label received|waiting)
         * - (start) --Abbr. Label Group--> (continue|store abbr. flags)
         * - (start) --[EOA]--> (stop)
         * - (start) --[?]--> (continue|store value)
         * - (label received|waiting) --Full Lable--> (label received|1. store last label as flag 2. waiting)
         * - (label received|waiting) --Abbr. Label--> (abbr. label received|1. store last label as flag 2. waiting)
         * - (label received|waiting) --Abbr. Label Group--> (continue|1. store last label as flag 2. store abbr. flags)
         * - (label received|waiting) --[EOA]-->(stop|store last label as flag)
         * - (label received|waiting) --[?]--> (continue|store label/value pair)
         * - (abbr. label received|waiting) --Full Lable--> (label received|1. store last abbr. flag 2. waiting)
         * - (abbr. label received|waiting) --Abbr. Label--> (abbr. label received|1. store last abbr. flag 2. waiting)
         * - (abbr. label received|waiting) --Abbr. Label Group--> (continue|1. store last abbr. flag 2. store abbr. flags)
         * - (abbr. label received|waiting) --[EOA]--> (stop|store last abbr. flag)
         * - (abbr. label received|waiting) --[?]--> (continue|store last abbr. label/value pair)
         * - (continue) --Full Label--> (label received|waiting)
         * - (continue) --Abbr. Label--> (abbr. label received|waiting)
         * - (continue) --Abbr. Label Group--> (continue|store abbr. flags)
         * - (continue) --[EOA]--> (stop)
         * - (continue) --[?]--> (continue|store value)
         */
        public IParseResult Parse(IEnumerable<string> args)
        {
            var resultBuilder = new ParseResultBuilder();
            IParsingState nextState = new StartState();
            foreach (string arg in args)
            {
                nextState = nextState.HandleInput(resultBuilder, arg);
            }

            // End-of-Argument input
            nextState.HandleInput(resultBuilder, null);
            return resultBuilder.Build();
        }
    }
}