using System;
using System.Text.RegularExpressions;

namespace Axe.Cli.Parser
{
    class OptionSymbol : IOptionSymbol
    {
        static readonly Regex Pattern = new Regex(
            "^[A-Z0-9_][A-Z0-9_\\-]{0,}$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline |
            RegexOptions.IgnoreCase);

        static readonly Regex FullFormPattern = new Regex(
            "^--[A-Z0-9_][A-Z0-9_\\-]{0,}",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline |
            RegexOptions.IgnoreCase);
        
        static readonly Regex AbbrFormPattern = new Regex(
            "^-[A-Z]{1,}$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline |
            RegexOptions.IgnoreCase);

        static readonly Regex AbbrSingleFormPattern = new Regex(
            "^-[A-Z]$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline |
            RegexOptions.IgnoreCase);
        
        public string Symbol { get; }

        public char? Abbreviation { get; }

        public OptionSymbol(string symbol, char? abbreviation)
        {
            if (symbol == null && abbreviation == null)
            {
                throw new ArgumentException("The symbol and abbreviation cannot be null at the same time."); 
            }

            if (symbol != null && !Pattern.IsMatch(symbol))
            {
                throw new ArgumentException($"The symbol '{symbol}' is not in a valid format."); 
            }

            if (abbreviation != null && abbreviation == '-')
            {
                throw new ArgumentException("The abbreviation cannot be a dash sign.");
            }

            Symbol = symbol;
            Abbreviation = abbreviation;
        }

        public override string ToString()
        {
            string symbolOutput = Symbol == null ? "(null)" : $"--{Symbol}";
            string abbrOutput = Abbreviation == null ? "(null)" : $"-{Abbreviation}";
            return $"full form: {symbolOutput}; abbr. form: {abbrOutput}";
        }

        public bool IsMatch(string argument)
        {
            if (argument.StartsWith("--", StringComparison.Ordinal))
            {
                return SymbolEqual(argument.Substring(2));
            }

            if (argument.StartsWith("-", StringComparison.Ordinal) && argument.Length == 2)
            {
                return AbbreviationEqual(argument[1]);
            }

            return false;
        }

        public bool IsConflict(IOptionSymbol other)
        {
            return SymbolEqual(other.Symbol) || AbbreviationEqual(other.Abbreviation);
        }

        bool AbbreviationEqual(char? otherAbbreviation)
        {
            return Abbreviation != null && otherAbbreviation != null &&
                char.ToUpperInvariant(Abbreviation.Value) == char.ToUpperInvariant(otherAbbreviation.Value);
        }

        bool SymbolEqual(string otherSymbol)
        {
            return Symbol != null && otherSymbol != null &&
                Symbol.Equals(otherSymbol, StringComparison.OrdinalIgnoreCase);
        }

        public static bool CanBeFullForm(string argument)
        {
            return argument != null && FullFormPattern.IsMatch(argument);
        }

        public static bool CanBeAbbreviationForm(string argument)
        {
            return argument != null && AbbrFormPattern.IsMatch(argument);
        }

        public static bool CanBeAbbreviationSingleForm(string argument)
        {
            return argument != null && AbbrSingleFormPattern.IsMatch(argument);
        }
    }
}