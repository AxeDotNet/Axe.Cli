﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Axe.Cli.Parser.Tokenizer
{
    class TokenizedResultBuilder
    {
        readonly IList<ICliOptionToken> tokens = new List<ICliOptionToken>();
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

        public CliArgsParsingResult Build()
        {
            if (hasBeenBuilt) { throw new InvalidOperationException("The builder has been built."); }
            if (command == null) { throw new InvalidOperationException("The command has not been set."); }

            MergeFlags();

            var result = new CliArgsParsingResult(command, keyValues, flags);
            hasBeenBuilt = true;

            return result;
        }

        bool AppendKeyValueOption(ICliOptionToken optionToken)
        {
            if (optionToken.Definition.Type != OptionType.KeyValue) { return false; }
            if (keyValues.ContainsKey(optionToken.Definition))
            {
                keyValues[optionToken.Definition].Add((string) optionToken.Value);
            }
            else
            {
                keyValues[optionToken.Definition] =
                    new List<string> {(string) optionToken.Value};
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
    }
}