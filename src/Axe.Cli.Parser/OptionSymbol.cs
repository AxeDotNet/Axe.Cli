using System;
using System.Diagnostics.CodeAnalysis;
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
        
        public string FullForm { get; }

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

            if (abbreviation != null && !IsEnglishAlphabet(abbreviation.Value) )
            {
                throw new ArgumentException("The abbreviation cannot be a dash sign.");
            }

            FullForm = symbol;
            Abbreviation = abbreviation;
        }

        public override string ToString()
        {
            string fullFormOutput = FullForm == null ? null : $"--{FullForm}";
            string abbrOutput = Abbreviation == null ? null : $"-{Abbreviation}";
            if (fullFormOutput != null && abbrOutput != null)
            {
                return $"{fullFormOutput} / {abbrOutput}";
            }

            return fullFormOutput ?? abbrOutput;
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
            return SymbolEqual(other.FullForm) || AbbreviationEqual(other.Abbreviation);
        }

        [SuppressMessage(
            "ReSharper", "ArrangeRedundantParentheses", Justification = "We humans like parentheses.")]
        static bool IsEnglishAlphabet(char value)
        {
            return (value >= 'a' && value <= 'z') || (value >= 'A' && value <= 'Z');
        }

        bool AbbreviationEqual(char? otherAbbreviation)
        {
            return Abbreviation != null && otherAbbreviation != null &&
                char.ToUpperInvariant(Abbreviation.Value) == char.ToUpperInvariant(otherAbbreviation.Value);
        }

        bool SymbolEqual(string otherSymbol)
        {
            return FullForm != null && otherSymbol != null &&
                FullForm.Equals(otherSymbol, StringComparison.OrdinalIgnoreCase);
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