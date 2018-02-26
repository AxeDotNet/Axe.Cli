using System.Collections.Generic;

namespace Axe.Cli.Parser
{
    static class ParsingErrorCodeDescription
    {
        static readonly Dictionary<ParsingErrorCode, string> descriptions = new Dictionary<ParsingErrorCode, string>
        {
            { ParsingErrorCode.DuplicationOfFlagInGroup, "The flag group cannot contains duplicated flag abbreviations." }
        };

        public static string GetDescription(this ParsingErrorCode code)
        {
            return descriptions[code];
        }
    }
}