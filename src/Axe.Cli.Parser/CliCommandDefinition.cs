using System;
using System.Text.RegularExpressions;
using Axe.Cli.Parser.Text;

namespace Axe.Cli.Parser
{
    class CliCommandDefinition : CliCommandDefinitionBase
    {
        static readonly Regex Pattern = new Regex(
            "^[A-Z0-9_][A-Z0-9_\\-]{0,}$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline | RegexOptions.IgnoreCase);
        
        public CliCommandDefinition(string symbol, string description)
        {
            ValidateSymbol(symbol);

            Symbol = symbol;
            Description = description.MakeSingleLine();
        }

        public override string Symbol { get; }

        public override string Description { get; }

        static void ValidateSymbol(string symbol)
        {
            if (symbol == null) { throw new ArgumentNullException(nameof(symbol)); }
            if (!Pattern.IsMatch(symbol))
            {
                throw new ArgumentException($"The symbol '{symbol}' is an invalid command.");
            }
        }

        public override bool IsConflict(ICliCommandSymbolDefinition commandDefinition)
        {
            if (!(commandDefinition is CliCommandDefinition c)) { return true; }
            return Symbol.Equals(c.Symbol, StringComparison.OrdinalIgnoreCase);
        }

        public override string ToString()
        {
            return $"{Symbol}: {Description}";
        }

        public override bool IsMatch(string argument)
        {
            return Symbol.Equals(argument, StringComparison.OrdinalIgnoreCase);
        }
    }
}