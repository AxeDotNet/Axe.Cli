using System;
using Axe.Cli.Parser.Test.Helpers;
using Xunit;

namespace Axe.Cli.Parser.Test
{
    public class ArgsParserIsRequiredFacts
    {
        [Fact]
        public void should_get_optional_value()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue("key", 'k', string.Empty)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(Array.Empty<string>());

            Assert.True(result.IsSuccess);
            Assert.Empty(result.GetOptionRawValue("--key"));
        }

        [Fact]
        public void should_be_error_if_required_value_not_present()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue("key", 'k', string.Empty, true)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(Array.Empty<string>());

            result.AssertError(
                ArgsParsingErrorCode.RequiredOptionNotPresent,
                "full form: --key; abbr. form: -k");
        }
    }
}