using System;
using System.Linq;
using Xunit;

namespace Axe.Cli.Parser.Test
{
    public class CliArgsParserWaitingValueStateFacts
    {
        /// <summary>
        /// waiting-value -(any)-> ok
        /// </summary>
        [Fact]
        public void should_store_key_value_with_default_command()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue("key", 'k', string.Empty)
                .EndCommand()
                .Build();

            CliArgsParsingResult result = parser.Parse(new[] {"--key", "value"});

            Assert.True(result.IsSuccess);
            Assert.Equal("value", result.GetOptionRawValues("--key").First());
            Assert.Equal("value", result.GetOptionRawValues("-k").First());
        }

        /// <summary>
        /// waiting-value -(any)-> ok
        /// </summary>
        [Theory]
        [InlineData("--value")]
        [InlineData("-value")]
        public void should_store_key_value_even_if_value_looks_like_options(string optionLikeValue)
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue("key", 'k', string.Empty)
                .EndCommand()
                .Build();

            CliArgsParsingResult result = parser.Parse(new[] { "--key", optionLikeValue });

            Assert.True(result.IsSuccess);
            Assert.Equal(optionLikeValue, result.GetOptionRawValues("--key").First());
            Assert.Equal(optionLikeValue, result.GetOptionRawValues("-k").First());

            Assert.Throws<ArgumentException>(() => result.GetOptionRawValues("--value"));
            Assert.Throws<ArgumentException>(() => result.GetOptionRawValues("-v"));
        }
    }
}