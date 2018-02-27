using System;
using Xunit;

namespace Axe.Cli.Parser.Test
{
    public class OptionSymbolFacts
    {
        [Fact]
        public void should_not_be_null_for_both_fullname_and_abbreviation()
        {
            Assert.Throws<ArgumentException>(() => new OptionSymbol(null, null));
        }

        [Fact]
        public void should_not_contains_dash_in_full_symbol()
        {
            Assert.Throws<ArgumentException>(() => new OptionSymbol("-name", 'c'));
        }

        [Fact]
        public void should_not_be_dash_for_abbreviation()
        {
            Assert.Throws<ArgumentException>(() => new OptionSymbol("name", '-'));
        }

        [Theory]
        [InlineData("o", 'o')]
        [InlineData("word", 'w')]
        [InlineData("word-with-dash", 'w')]
        [InlineData("word-with-tail-dash-", 'w')]
        [InlineData("word_with_lodash", 'w')]
        [InlineData("word_with_tail_lodash_", 'w')]
        [InlineData("_word_with_lodash_prefix", 'w')]
        [InlineData("not_null", null)]
        [InlineData(null, 'n')]
        public void should_create_valid_symbol(string symbol, char? abbr)
        {
            var optionSymbol = new OptionSymbol(symbol, abbr);
            Assert.Equal(symbol, optionSymbol.Symbol);
            Assert.Equal(abbr, optionSymbol.Abbreviation);
        }

        [Theory]
        [InlineData("o", 'o', "o", 'o')]                // totally equal
        [InlineData("o", 'o', "O", 'O')]                // upper case
        [InlineData("o", 'o', "v", 'o')]                // abbr equal
        [InlineData(null, 'o', "v", 'o')]               // abbr equal
        [InlineData("o", 'o', "o", 'v')]                // symbol equal
        [InlineData("o", null, "o", 'v')]               // symbol equal
        public void should_determine_conflict(string s1, char? a1, string s2, char? a2)
        {
            Assert.True(
                new OptionSymbol(s1, a1).IsConflict(new OptionSymbol(s2, a2)));
        }

        [Theory]
        [InlineData("o", null, "v", null)]
        [InlineData("o", null, "v", 'o')]
        [InlineData("o", 'p', "v", 'o')]
        [InlineData("o", 'p', "v", 'q')]
        public void should_be_no_conflict(string s1, char? a1, string s2, char? a2)
        {
            Assert.False(
                new OptionSymbol(s1, a1).IsConflict(new OptionSymbol(s2, a2)));
        }
    }
}