using System;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Cli.Parser
{
    public class CliArgsParsingResult
    {
        readonly IList<KeyValuePair<ICliOptionDefinition, bool>> optionFlags;
        readonly IList<KeyValuePair<ICliOptionDefinition, IList<string>>> optionValues;

        public ICliCommandDefinition Command { get; }
        public bool IsSuccess { get; }
        public CliArgsParsingError Error { get; }

        public CliArgsParsingResult(CliArgsParsingError error)
        {
            Error = error ?? throw new ArgumentNullException(nameof(error));
            IsSuccess = false;
        }

        public CliArgsParsingResult(
            ICliCommandDefinition command,
            IEnumerable<KeyValuePair<ICliOptionDefinition, IList<string>>> optionValues,
            IEnumerable<KeyValuePair<ICliOptionDefinition, bool>> optionFlags)
        {
            Command = command ?? throw new ArgumentNullException(nameof(command));
            this.optionValues = optionValues?.ToArray() ?? Array.Empty<KeyValuePair<ICliOptionDefinition, IList<string>>>();
            this.optionFlags = optionFlags?.ToArray() ?? Array.Empty<KeyValuePair<ICliOptionDefinition, bool>>();
            IsSuccess = true;
        }

        public bool GetFlagValue(string flag)
        {
            KeyValuePair<ICliOptionDefinition, bool> matchedFlag = optionFlags.FirstOrDefault(o => o.Key.IsMatch(flag));
            if (matchedFlag.Key == null) { throw new ArgumentException($"The flag you specified is not defined: '{flag}'");}
            return matchedFlag.Value;
        }

        public IList<string> GetOptionValue(string option)
        {
            KeyValuePair<ICliOptionDefinition, IList<string>> matchedKeyValue = 
                optionValues.FirstOrDefault(o => o.Key.IsMatch(option));
            if (matchedKeyValue.Key == null)
            {
                throw new ArgumentException($"The option you specified is not defined: '{option}'");
            }

            return matchedKeyValue.Value;
        }
    }
}