using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Axe.Cli.Parser.Tokenizer
{
    class PreParserResultBuilder
    {
        readonly IList<KeyValuePair<IFreeValueDefinition, string>> freeValues =
            new List<KeyValuePair<IFreeValueDefinition, string>>();
        readonly IDictionary<ICliOptionDefinition, bool> flags = new Dictionary<ICliOptionDefinition, bool>();
        readonly IDictionary<ICliOptionDefinition, IList<string>> keyValues =
            new Dictionary<ICliOptionDefinition, IList<string>>();

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
            
            AppendUnspecifiedFlags();
            AppendOptionalKeyValues();
            AppendOptionalFreeValues();
            
            var result = new ArgsParsingResult(command, keyValues, flags, freeValues);
            hasBeenBuilt = true;

            return result;
        }

        void ValidateRequiredKeyValues()
        {
            ICliOptionDefinition notPresentedRequiredOption = command.GetRegisteredOptions()
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
            IEnumerable<ICliOptionDefinition> nonRequiredButNotPresent = command.GetRegisteredOptions()
                .Where(o => o.Type == OptionType.KeyValue && !o.IsRequired)
                .Where(o => !keyValues.ContainsKey(o));
            foreach (ICliOptionDefinition option in nonRequiredButNotPresent)
            {
                keyValues.Add(option, Array.Empty<string>());
            }
        }

        void AppendUnspecifiedFlags()
        {
            IEnumerable<ICliOptionDefinition> notSetFlags = command.GetRegisteredOptions()
                .Where(o => o.Type == OptionType.Flag)
                .Where(o => !flags.ContainsKey(o));
            foreach (ICliOptionDefinition notSetFlag in notSetFlags)
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