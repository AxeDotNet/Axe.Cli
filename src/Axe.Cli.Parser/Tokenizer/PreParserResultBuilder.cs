﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Axe.Cli.Parser.Tokenizer
{
    class PreParserResultBuilder
    {
        readonly IList<KeyValuePair<IFreeValueDefinition, string>> freeValues =
            new List<KeyValuePair<IFreeValueDefinition, string>>();
        readonly IDictionary<IOptionDefinition, bool> flags = new Dictionary<IOptionDefinition, bool>();
        readonly IDictionary<IOptionDefinition, IList<string>> keyValues =
            new Dictionary<IOptionDefinition, IList<string>>();

        bool hasBeenBuilt;
        ICommandDefinition command;

        public void SetCommand(ICommandDefinition commandDefinition)
        {
            Debug.Assert(commandDefinition != null);

            if (command != null)
            {
                throw new InvalidOperationException("The command has been set.");
            }
            
            command = commandDefinition;
        }

        public void AppendOptionToken(IOptionToken optionToken, string argument)
        {
            bool handled = AppendFlag(optionToken, argument) || AppendKeyValueOption(optionToken);
            if (!handled) { throw new ArgParsingException(ArgsParsingErrorCode.UnknownOptionType, argument); }
        }

        public void AppendFreeValue(string freeValue)
        {
            if (string.IsNullOrEmpty(freeValue))
            {
                throw new ArgumentException("The free value cannot be null or empty");
            }

            int nextIndex = freeValues.Count;
            IFreeValueDefinition[] freeValueDefinitions = command.GetRegisteredFreeValues().ToArray();
            freeValues.Add(
                freeValueDefinitions.Length <= nextIndex
                    ? new KeyValuePair<IFreeValueDefinition, string>(NullFreeValueDefinition.Instance, freeValue)
                    : new KeyValuePair<IFreeValueDefinition, string>(freeValueDefinitions[nextIndex], freeValue));
        }

        public ArgsParsingResult Build()
        {
            if (hasBeenBuilt) { throw new InvalidOperationException("The builder has been built."); }
            if (command == null) { throw new InvalidOperationException("The command has not been set."); }

            ValidateRequiredKeyValues();
            ValidateRequiredFreeValues();
            
            AppendUnspecifiedFlags();
            AppendOptionalKeyValues();
            AppendOptionalFreeValues();
            
            var result = new ArgsParsingResult(command, keyValues, flags, freeValues);
            hasBeenBuilt = true;

            return result;
        }

        void ValidateRequiredFreeValues()
        {
            IFreeValueDefinition notPresentedRequiredFreeValue = command.GetRegisteredFreeValues()
                .Where(fv => fv.IsRequired)
                .FirstOrDefault(fv => !freeValues.Any(f => f.Key.Equals(fv)));

            if (notPresentedRequiredFreeValue != null)
            {
                throw new ArgParsingException(
                    ArgsParsingErrorCode.RequiredFreeValueNotPresent,
                    $"<{notPresentedRequiredFreeValue.Name}>");
            }
        }

        void ValidateRequiredKeyValues()
        {
            IOptionDefinition notPresentedRequiredOption = command.GetRegisteredOptions()
                .Where(o => o.Type == OptionType.KeyValue && o.IsRequired)
                .FirstOrDefault(o => !keyValues.ContainsKey(o));
            if (notPresentedRequiredOption != null)
            {
                throw new ArgParsingException(
                    ArgsParsingErrorCode.RequiredOptionNotPresent,
                    notPresentedRequiredOption.Symbol.ToString());
            }
        }

        void AppendOptionalFreeValues()
        {
            IEnumerable<IFreeValueDefinition> unspecifiedFreeValueDefinitions = command
                .GetRegisteredFreeValues()
                .Where(fvDef => !freeValues.Any(fv => fv.Key.Equals(fvDef)));
            foreach (IFreeValueDefinition definition in unspecifiedFreeValueDefinitions)
            {
                freeValues.Add(new KeyValuePair<IFreeValueDefinition, string>(definition, string.Empty));
            }
        }

        void AppendOptionalKeyValues()
        {
            IEnumerable<IOptionDefinition> nonRequiredButNotPresent = command.GetRegisteredOptions()
                .Where(o => o.Type == OptionType.KeyValue && !o.IsRequired)
                .Where(o => !keyValues.ContainsKey(o));
            foreach (IOptionDefinition option in nonRequiredButNotPresent)
            {
                keyValues.Add(option, Array.Empty<string>());
            }
        }

        void AppendUnspecifiedFlags()
        {
            IEnumerable<IOptionDefinition> notSetFlags = command.GetRegisteredOptions()
                .Where(o => o.Type == OptionType.Flag)
                .Where(o => !flags.ContainsKey(o));
            foreach (IOptionDefinition notSetFlag in notSetFlags)
            {
                flags.Add(notSetFlag, false);
            }
        }

        bool AppendKeyValueOption(IOptionToken optionToken)
        {
            if (optionToken.Definition.Type != OptionType.KeyValue) { return false; }
            if (keyValues.ContainsKey(optionToken.Definition))
            {
                keyValues[optionToken.Definition].Add(optionToken.Value);
            }
            else
            {
                keyValues[optionToken.Definition] =
                    new List<string> {optionToken.Value};
            }

            return true;

        }

        bool AppendFlag(IOptionToken optionToken, string argument)
        {
            if (optionToken.Definition.Type != OptionType.Flag) { return false; }

            if (flags.ContainsKey(optionToken.Definition))
            {
                throw new ArgParsingException(
                    ArgsParsingErrorCode.DuplicateFlagsInArgs,
                    argument);
            }

            flags.Add(optionToken.Definition, true);
            return true;
        }
    }
}