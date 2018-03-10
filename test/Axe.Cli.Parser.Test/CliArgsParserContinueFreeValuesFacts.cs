using System.Linq;
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

            CliArgsParsingResult result = parser.Parse(new[] {"free-value"});

            Assert.True(result.IsSuccess);
            Assert.Equal(new []{"free-value"}, result.GetUndefinedFreeValues());
        }

        [Fact]
        public void should_get_free_value_continuously_for_default_command()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginDefaultCommand()
                .ConfigFreeValue(true)
                .EndCommand()
                .Build();

            CliArgsParsingResult result = parser.Parse(new[] {"free-value1", "free-value2"});

            Assert.True(result.IsSuccess);
            Assert.Equal(new []{"free-value1", "free-value2"}, result.GetUndefinedFreeValues());
        }

        [Fact]
        public void should_get_free_value_for_specified_command()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .ConfigFreeValue(true)
                .EndCommand()
                .Build();

            CliArgsParsingResult result = parser.Parse(new[] {"command", "free-value"});

            Assert.True(result.IsSuccess);
            Assert.Equal(new [] {"free-value"}, result.GetUndefinedFreeValues());
        }
        
        [Fact]
        public void should_get_free_value_continuously_for_specified_command()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .ConfigFreeValue(true)
                .EndCommand()
                .Build();

            CliArgsParsingResult result = parser.Parse(new[] {"command", "free-value1", "free-value2"});

            Assert.True(result.IsSuccess);
            Assert.Equal(new [] {"free-value1", "free-value2"}, result.GetUndefinedFreeValues());
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

            CliArgsParsingResult result = parser.Parse(new [] {"command", "-f", "-a"});
            
            Assert.True(result.IsSuccess);
            Assert.Equal(new [] {"-a"}, result.GetUndefinedFreeValues());
            Assert.True(result.GetFlagValues("--flag"));
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

            CliArgsParsingResult result = parser.Parse(new [] {"command", "-a", "-f"});
            
            Assert.True(result.IsSuccess);
            Assert.Equal(new [] {"-a", "-f"}, result.GetUndefinedFreeValues());
            Assert.False(result.GetFlagValues("--flag"));
        }

        [Fact]
        public void should_capture_single_free_variable()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .AddFreeValue("name", "description")
                .EndCommand()
                .Build();

            CliArgsParsingResult result = parser.Parse(new[] {"command", "free_value"});

            Assert.True(result.IsSuccess);
            Assert.Equal("free_value", result.GetFreeRawValue("name"));
        }

        [Fact]
        public void should_capture_multiple_free_values()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .AddFreeValue("name", "description")
                .AddFreeValue("age", "description")
                .EndCommand()
                .Build();

            CliArgsParsingResult result = parser.Parse(new[] {"command", "name_value", "age_value"});

            Assert.True(result.IsSuccess);
            Assert.Equal("name_value", result.GetFreeRawValue("name"));
            Assert.Equal("age_value", result.GetFreeRawValue("age"));
        }

        [Fact]
        public void should_capture_multiple_free_values_with_undefined_ones()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .AddFreeValue("name", "description")
                .AddFreeValue("age", "description")
                .EndCommand()
                .Build();

            CliArgsParsingResult result = parser.Parse(
                new[] {"command", "name_value", "age_value", "undefined_value1", "undefined_value2"});

            Assert.True(result.IsSuccess);
            Assert.Equal("name_value", result.GetFreeRawValue("name"));
            Assert.Equal("age_value", result.GetFreeRawValue("age"));
            Assert.Equal(new object[] {"undefined_value1", "undefined_value2"}, result.GetUndefinedFreeValues());
        }

        [Fact]
        public void should_ignore_uncaptured_definitions()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .AddFreeValue("name", string.Empty)
                .AddFreeValue("uncaptured_definition", string.Empty)
                .EndCommand()
                .Build();

            CliArgsParsingResult result = parser.Parse(new[] {"command", "name_value"});

            Assert.True(result.IsSuccess);
            Assert.Equal("name_value", result.GetFreeRawValue("name"));
            Assert.Empty(result.GetFreeValue("uncaptured_definition"));
            Assert.Equal(string.Empty, result.GetFreeRawValue("uncaptured_definition"));
        }
    }
}