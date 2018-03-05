﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Axe.Cli.Parser.Tokenizer
{
    class PreParserResultBuilder
    {
        readonly IList<string> freeValues = new List<string>();
        readonly IDictionary<ICliOptionDefinition, bool> flags = new Dictionary<ICliOptionDefinition, bool>();
        readonly IDictionary<ICliOptionDefinition, IList<string>> keyValues = new Dictionary<ICliOptionDefinition, IList<string>>();

        bool hasBeenBuilt;
        ICliCommandDefinition command;

        public void SetCommand(ICliCommandDefinition commandDefinition)
        {
            Debug.Assert(commandDefinition != null);

            if (command != null)
            {
                throw new InvalidOperationException("The command has been set.");
            }
            
            command = commandDefinition;
        }

        public void AppendOptionToken(ICliOptionToken optionToken, string argument)
        {
            bool handled = AppendFlag(optionToken, argument) || AppendKeyValueOption(optionToken);
            if (!handled) { throw new CliArgParsingException(CliArgsParsingErrorCode.UnknownOptionType, argument); }
        }

        public void AppendFreeValue(string freeValue)
        {
            if (string.IsNullOrEmpty(freeValue))
            {
                throw new ArgumentException("The free value cannot be null or empty");
            }
            
            freeValues.Add(freeValue);
        }

        public CliArgsPreParsingResult Build()
        {
            if (hasBeenBuilt) { throw new InvalidOperationException("The builder has been built."); }
            if (command == null) { throw new InvalidOperationException("The command has not been set."); }

            ValidateRequiredKeyValues();
            MergeFlags();
            MergeNotRequiredKeyValues();

            var result = new CliArgsPreParsingResult(command, keyValues, flags, freeValues);
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
                throw new CliArgParsingException(
                    CliArgsParsingErrorCode.RequiredOptionNotPresent,
                    notPresentedRequiredOption.Symbol.ToString());
            }
        }

        void MergeNotRequiredKeyValues()
        {
            IEnumerable<ICliOptionDefinition> nonRequiredButNotPresent = command.GetRegisteredOptions()
                .Where(o => o.Type == OptionType.KeyValue && !o.IsRequired)
                .Where(o => !keyValues.ContainsKey(o));
            foreach (ICliOptionDefinition option in nonRequiredButNotPresent)
            {
                keyValues.Add(option, Array.Empty<string>());
            }
        }

        void MergeFlags()
        {
            IEnumerable<ICliOptionDefinition> notSetFlags = command.GetRegisteredOptions()
                .Where(o => o.Type == OptionType.Flag)
                .Where(o => !flags.ContainsKey(o));
            foreach (ICliOptionDefinition notSetFlag in notSetFlags)
            {
                flags.Add(notSetFlag, false);
            }
        }

        bool AppendKeyValueOption(ICliOptionToken optionToken)
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

        bool AppendFlag(ICliOptionToken optionToken, string argument)
        {
            if (optionToken.Definition.Type != OptionType.Flag) { return false; }

            if (flags.ContainsKey(optionToken.Definition))
            {
                throw new CliArgParsingException(
                    CliArgsParsingErrorCode.DuplicateFlagsInArgs,
                    argument);
            }

            flags.Add(optionToken.Definition, true);
            return true;
        }
    }
}