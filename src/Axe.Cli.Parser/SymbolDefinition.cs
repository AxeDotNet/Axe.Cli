using System.Text.RegularExpressions;

namespace Axe.Cli.Parser
{
    static class SymbolDefinition
    {
        const RegexOptions MatchingOptions =
            RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase;
        static readonly Regex AbbrLabelGroup = new Regex("^-[A-Z]{2,}$", MatchingOptions);
        static readonly Regex PossibleArea = new Regex("^[A-Z0-9_][A-Z0-9_\\-]+$", MatchingOptions);
        static readonly Regex AbbrLabel = new Regex("^-[A-Z]$", MatchingOptions);
        static readonly Regex FullLabel = new Regex("^--[A-Z0-9_][A-Z0-9_\\-]+$", MatchingOptions);

        public static bool IsAbbrLabel(string input)
        {
            return AbbrLabel.IsMatch(input);
        }

        public static bool IsFullLabel(string input)
        {
            return FullLabel.IsMatch(input);
        }

        public static bool IsAbbrFlags(string input)
        {
            return AbbrLabelGroup.IsMatch(input);
        }

        public static bool IsEndOfArgument(string input)
        {
            return input == null;
        }

        public static bool IsPossibleArea(string input)
        {
            return PossibleArea.IsMatch(input);
        }
    }
}