using System;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Cli.Parser.Tokenizer
{
    abstract class TokenizerStateBase : ITokenizerState
    {
        protected static bool IsEndOfArguments(string argument)
        {
            return argument == null;
        }

        protected static ICliOptionDefinition ResolveKeyValueOptionLabel(
            ICliCommandDefinition selectedCommand,
            string argument)
        {
            return selectedCommand.GetRegisteredOptions()
                .FirstOrDefault(o => o.Type == OptionType.KeyValue && o.IsMatch(argument));
        }
        
        protected static IList<ICliOptionDefinition> ResolveFlagOptionLabels(
            ICliCommandDefinition selectedCommand, 
            string argument)
        {
            if (OptionSymbol.CanBeFullForm(argument))
            {
                return selectedCommand.GetRegisteredOptions()
                    .Where(o => o.Type == OptionType.Flag && o.IsMatch(argument))
                    .ToArray();
            }
            
            if (OptionSymbol.CanBeAbbreviationForm(argument))
            {
                IEnumerable<string> flagArguments = SplitAbbrArgument(argument);
                return selectedCommand.GetRegisteredOptions()
                    .Where(o => o.Type == OptionType.Flag && flagArguments.Any(o.IsMatch))
                    .ToArray();
            }

            return Array.Empty<ICliOptionDefinition>();
        }

        static IEnumerable<string> SplitAbbrArgument(string argument)
        {
            return argument.Skip(1).Select(c => $"-{c}").ToArray();
        }

        public abstract ITokenizerState MoveToNext(string argument);
    }
}