using System.Linq;
using Axe.Cli.Parser.Test.Helpers;
using Xunit;

namespace Axe.Cli.Parser.Test.End2End
{
    public class WhenGetMixedInformationFromParsingResult
    {
        [Fact]
        public void should_get_mixed_options_for_default_command()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue("key-a", 'a', string.Empty)
                .AddFlagOption("flag-b", 'b', string.Empty)
                .AddFlagOption("flag-c", 'c', string.Empty)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new[] { "--key-a", "value", "-b" });

            result.AssertSuccess();
            Assert.Equal("value", result.GetOptionRawValue("-a").Single());
            Assert.True(result.GetFlagValue("--flag-b"));
            Assert.False(result.GetFlagValue("--flag-c"));
        }
    }
}