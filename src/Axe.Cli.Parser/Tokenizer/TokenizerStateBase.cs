using System;
using System.Collections.Generic;
using System.Linq;
using Axe.Cli.Parser.Extensions;

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
            if (!OptionSymbol.CanBeFullForm(argument) && !OptionSymbol.CanBeAbbreviationSingleForm(argument))
            {
                return null;
            }

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
                string[] flagArguments = SplitAbbrArgument(argument);
                if (flagArguments.HasDuplication(StringComparer.OrdinalIgnoreCase))
                {
                    throw new CliArgParsingException(CliArgsParsingErrorCode.DuplicateFlagsInArgs, argument);
                }

                return selectedCommand.GetRegisteredOptions()
                    .Where(o => o.Type == OptionType.Flag && flagArguments.Any(o.IsMatch))
                    .ToArray();
            }

            return Array.Empty<ICliOptionDefinition>();
        }

        protected static ITokenizerState HandleKeyValueOptionArgument(
            ICliCommandDefinition command,
            TokenizedResultBuilder resultBuilder,
            string argument)
        {
            ICliOptionDefinition kvOption = ResolveKeyValueOptionLabel(
                command,
                argument);
            return kvOption != null
                ? new WaitingValueWithCommandState(command, kvOption, argument, resultBuilder)
                : null;
        }

        protected static ITokenizerState HandleFlagOptionArgument(
            ICliCommandDefinition command,
            TokenizedResultBuilder resultBuilder,
            string argument)
        {
            IList<ICliOptionDefinition> flagOptions = ResolveFlagOptionLabels(
                command,
                argument);
            if (flagOptions.Count > 0)
            {
                foreach (ICliOptionDefinition flagOption in flagOptions)
                {
                    resultBuilder.AppendOptionToken(new CliOptionToken(flagOption), argument);
                }
                return new ContinueWithCommandState(command, resultBuilder);
            }

            return null;
        }

        static string[] SplitAbbrArgument(string argument)
        {
            return argument.Skip(1).Select(c => $"-{c}").ToArray();
        }

        public abstract ITokenizerState MoveToNext(string argument);
    }
}