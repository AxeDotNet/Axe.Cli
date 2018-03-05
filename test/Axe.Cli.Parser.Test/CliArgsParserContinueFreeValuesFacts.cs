using Xunit;

namespace Axe.Cli.Parser.Test
{
    public class CliArgsParserContinueFreeValuesFacts
    {
        [Fact]
        public void should_get_free_value_for_default_command()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginDefaultCommand()
                .ConfigFreeValue(true)
                .EndCommand()
                .Build();

            CliArgsPreParsingResult result = parser.Parse(new[] {"free-value"});

            Assert.True(result.IsSuccess);
            Assert.Equal(new []{"free-value"}, result.GetFreeValues());
        }

        [Fact]
        public void should_get_free_value_continuously_for_default_command()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginDefaultCommand()
                .ConfigFreeValue(true)
                .EndCommand()
                .Build();

            CliArgsPreParsingResult result = parser.Parse(new[] {"free-value1", "free-value2"});

            Assert.True(result.IsSuccess);
            Assert.Equal(new []{"free-value1", "free-value2"}, result.GetFreeValues());
        }

        [Fact]
        public void should_get_free_value_for_specified_command()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .ConfigFreeValue(true)
                .EndCommand()
                .Build();

            CliArgsPreParsingResult result = parser.Parse(new[] {"command", "free-value"});

            Assert.True(result.IsSuccess);
            Assert.Equal(new [] {"free-value"}, result.GetFreeValues());
        }
        
        [Fact]
        public void should_get_free_value_continuously_for_specified_command()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .ConfigFreeValue(true)
                .EndCommand()
                .Build();

            CliArgsPreParsingResult result = parser.Parse(new[] {"command", "free-value1", "free-value2"});

            Assert.True(result.IsSuccess);
            Assert.Equal(new [] {"free-value1", "free-value2"}, result.GetFreeValues());
        }

        [Fact]
        public void should_get_as_free_value_even_if_it_looks_like_option()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .ConfigFreeValue(true)
                .AddFlagOption("flag", 'f', string.Empty)
                .EndCommand()
                .Build();

            CliArgsPreParsingResult result = parser.Parse(new [] {"command", "-f", "-a"});
            
            Assert.True(result.IsSuccess);
            Assert.Equal(new [] {"-a"}, result.GetFreeValues());
            Assert.True(result.GetFlagValue("--flag"));
        }

        [Fact]
        public void should_get_as_free_value_after_one_free_value()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .ConfigFreeValue(true)
                .AddFlagOption("flag", 'f', string.Empty)
                .EndCommand()
                .Build();

            CliArgsPreParsingResult result = parser.Parse(new [] {"command", "-a", "-f"});
            
            Assert.True(result.IsSuccess);
            Assert.Equal(new [] {"-a", "-f"}, result.GetFreeValues());
            Assert.False(result.GetFlagValue("--flag"));
        }
    }
}