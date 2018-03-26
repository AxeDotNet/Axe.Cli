using System;
using System.Collections.Generic;
using System.Linq;
using Axe.Cli.Parser.Extensions;

namespace Axe.Cli.Parser.Tokenizer
{
    abstract class PreParsingStateBase : IPreParsingState
    {
        protected static bool IsEndOfArguments(string argument)
        {
            return argument == null;
        }

        static IOptionDefinition ResolveKeyValueOptionLabel(
            ICommandDefinition selectedCommand,
            string argument)
        {
            if (!OptionSymbol.CanBeFullForm(argument) && !OptionSymbol.CanBeAbbreviationSingleForm(argument))
            {
                return null;
            }

            return selectedCommand.GetRegisteredOptions()
                .FirstOrDefault(o => o.Type == OptionType.KeyValue && o.IsMatch(argument));
        }
        
        static IList<IOptionDefinition> ResolveFlagOptionLabels(
            ICommandDefinition selectedCommand, 
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
                    throw new ArgParsingException(ArgsParsingErrorCode.DuplicateFlagsInArgs, argument);
                }

                IOptionDefinition[] resolvedDefinitions = selectedCommand.GetRegisteredOptions()
                    .Where(o => o.Type == OptionType.Flag && flagArguments.Any(o.IsMatch))
                    .ToArray();

                return resolvedDefinitions.Length != flagArguments.Length
                    ? Array.Empty<IOptionDefinition>()
                    : resolvedDefinitions;
            }

            return Array.Empty<IOptionDefinition>();
        }

        protected static IPreParsingState HandleKeyValueOptionArgument(
            ICommandDefinition command,
            PreParserResultBuilder resultBuilder,
            string argument)
        {
            IOptionDefinition kvOption = ResolveKeyValueOptionLabel(
                command,
                argument);
            return kvOption != null
                ? new WaitingValueState(command, kvOption, argument, resultBuilder)
                : null;
        }

        protected static IPreParsingState HandleFlagOptionArgument(
            ICommandDefinition command,
            PreParserResultBuilder resultBuilder,
            string argument)
        {
            IList<IOptionDefinition> flagOptions = ResolveFlagOptionLabels(
                command,
                argument);
            if (flagOptions.Count > 0)
            {
                foreach (IOptionDefinition flagOption in flagOptions)
                {
                    resultBuilder.AppendOptionToken(new OptionToken(flagOption), argument);
                }
                return new ContinueState(command, resultBuilder);
            }

            return null;
        }

        static string[] SplitAbbrArgument(string argument)
        {
            return argument.Skip(1).Select(c => $"-{c}").ToArray();
        }

        public abstract IPreParsingState MoveToNext(string argument);

        protected static IPreParsingState HandleFreeValueArgument(ICommandDefinition selectedCommand,
            PreParserResultBuilder resultBuilder, string argument)
        {
            if (selectedCommand.AllowFreeValue)
            {
                resultBuilder.AppendFreeValue(argument);
                return new ContinueFreeValueState(selectedCommand, resultBuilder);
            }

            throw new ArgParsingException(
                ArgsParsingErrorCode.FreeValueNotSupported,
                argument);
        }
    }
}