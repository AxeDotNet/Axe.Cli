using System;
using Axe.Cli.Parser.Test.Helpers;
using Xunit;

namespace Axe.Cli.Parser.Test
{
    public class CliArgsParserIsRequiredFacts
    {
        [Fact]
        public void should_get_optional_value()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue("key", 'k', string.Empty)
                .EndCommand()
                .Build();

            CliArgsPreParsingResult result = parser.Parse(Array.Empty<string>());

            Assert.True(result.IsSuccess);
            Assert.Empty(result.GetOptionValue("--key"));
        }

        [Fact]
        public void should_be_error_if_required_value_not_present()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue("key", 'k', string.Empty, true)
                .EndCommand()
                .Build();

            CliArgsPreParsingResult result = parser.Parse(Array.Empty<string>());

            result.AssertError(
                CliArgsParsingErrorCode.RequiredOptionNotPresent,
                "full form: --key; abbr. form: -k");
        }
    }
}