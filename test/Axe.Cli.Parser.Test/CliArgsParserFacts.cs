using System;
using Xunit;

namespace Axe.Cli.Parser.Test
{
    public class CliArgsParserFacts
    {
        [Fact]
        public void should_accept_empty_arguments()
        {
            IParseResult result = new CliArgsParser().Parse(Array.Empty<string>());

            Assert.True(result.IsSuccess);
            Assert.False(result.HasPossibleArea);
            Assert.Equal(string.Empty, result.PossibleArea);
            Assert.Empty(result.Pairs);
            Assert.Empty(result.Flags);
            Assert.Empty(result.FreeValues);
        }

        [Fact]
        public void should_get_abbr_flags_group()
        {
            IParseResult result = new CliArgsParser().Parse(new [] {"-rf"});

            Assert.True(result.IsSuccess);
            Assert.False(result.HasPossibleArea);
            Assert.Equal(string.Empty, result.PossibleArea);
            Assert.Empty(result.Pairs);
            Assert.Equal(new [] {"-r", "-f"}, result.Flags);
            Assert.Empty(result.FreeValues);
        }

        [Fact]
        public void should_get_possible_area_value()
        {
            IParseResult result = new CliArgsParser().Parse(new[] { "area-value" });

            Assert.True(result.IsSuccess);
            Assert.True(result.HasPossibleArea);
            Assert.Equal("area-value", result.PossibleArea);
            Assert.Empty(result.Pairs);
            Assert.Empty(result.Flags);
            Assert.Empty(result.FreeValues);
        }

        [Fact]
        public void should_get_free_value_if_the_first_arg_doesnt_match_area_requirement()
        {
            IParseResult result = new CliArgsParser().Parse(new[] { "not-area-%-value" });

            Assert.True(result.IsSuccess);
            Assert.False(result.HasPossibleArea);
            Assert.Equal(string.Empty, result.PossibleArea);
            Assert.Empty(result.Pairs);
            Assert.Empty(result.Flags);
            Assert.Equal(new [] { "not-area-%-value" }, result.FreeValues);
        }
    }
}