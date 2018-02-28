using System;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Cli.Parser
{
    public class CliArgsParsingResult
    {
        readonly IList<string> freeValues;
        readonly IList<KeyValuePair<ICliOptionDefinition, bool>> optionFlags;
        readonly IList<KeyValuePair<ICliOptionDefinition, object[]>> optionValues;

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
            IEnumerable<KeyValuePair<ICliOptionDefinition, object[]>> optionValues,
            IEnumerable<KeyValuePair<ICliOptionDefinition, bool>> optionFlags,
            IEnumerable<string> freeValues)
        {
            Command = command ?? throw new ArgumentNullException(nameof(command));
            this.optionValues = optionValues?.ToArray() ?? Array.Empty<KeyValuePair<ICliOptionDefinition, object[]>>();
            this.optionFlags = optionFlags?.ToArray() ?? Array.Empty<KeyValuePair<ICliOptionDefinition, bool>>();
            this.freeValues = freeValues?.ToArray() ?? Array.Empty<string>();
            IsSuccess = true;
        }

        public bool GetFlagValue(string flag)
        {
            KeyValuePair<ICliOptionDefinition, bool> matchedFlag = optionFlags.FirstOrDefault(o => o.Key.IsMatch(flag));
            if (matchedFlag.Key == null) { throw new ArgumentException($"The flag you specified is not defined: '{flag}'");}
            return matchedFlag.Value;
        }
    }
}