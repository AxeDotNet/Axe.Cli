﻿using System;
using System.Text.RegularExpressions;

namespace Axe.Cli.Parser
{
    class CommandDefinition : CommandDefinitionBase
    {
        static readonly Regex Pattern = new Regex(
            "^[A-Z0-9_][A-Z0-9_\\-]{0,}$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline | RegexOptions.IgnoreCase);
        
        public CommandDefinition(string symbol, string description)
        {
            ValidateSymbol(symbol);

            Symbol = symbol;
            Description = description ?? string.Empty;
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

        public override bool IsConflict(ICommandDefinition commandDefinition)
        {
            if (!(commandDefinition is CommandDefinition c)) { return true; }
            return Symbol.Equals(c.Symbol, StringComparison.OrdinalIgnoreCase);
        }

        public override string ToString()
        {
            return $"command {Symbol}";
        }

        public override bool IsMatch(string argument)
        {
            return Symbol.Equals(argument, StringComparison.OrdinalIgnoreCase);
        }

        public static bool CanBeCommand(string argument)
        {
            return Pattern.IsMatch(argument);
        }
    }
}