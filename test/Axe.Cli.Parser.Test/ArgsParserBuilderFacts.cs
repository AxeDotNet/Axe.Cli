using System;
using Xunit;

namespace Axe.Cli.Parser.Test
{
    public class ArgsParserBuilderFacts
    {
        [Theory]
        [InlineData("o", 'o', "-o")]
        [InlineData("word", 'w', "-w")]
        [InlineData("word-with-dash", 'w', "-w")]
        [InlineData("word-with-tail-dash-", 'w', "-w")]
        [InlineData("word_with_lodash", 'w', "-w")]
        [InlineData("word_with_tail_lodash_", 'w', "-w")]
        [InlineData("_word_with_lodash_prefix", 'w', "-w")]
        [InlineData("not_null", null, "--not_null")]
        [InlineData(null, 'n', "-n")]
        public void should_create_valid_symbol(string symbol, char? abbr, string argument)
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption(symbol, abbr, null)
                .EndCommand()
                .Build();
            
            ArgsParsingResult result = parser.Parse(new[] {argument});

            Assert.True(result.GetFlagValue(argument));
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
            CommandBuilder builder = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption(s1, a1, string.Empty);

            Assert.Throws<ArgumentException>(() => builder.AddFlagOption(s2, a2, string.Empty));
        }
    }
}