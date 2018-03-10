using System.Linq;
using Axe.Cli.Parser.Test.Helpers;
using Xunit;

namespace Axe.Cli.Parser.Test
{
    public class CliArgsParserContinueStateFacts
    {
        [Fact]
        public void should_get_key_value_for_specified_command()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .AddOptionWithValue("key", 'k', string.Empty)
                .EndCommand()
                .Build();

            CliArgsParsingResult result = parser.Parse(new [] {"command", "-k", "value"});

            Assert.True(result.IsSuccess);
            Assert.Equal("value", result.GetOptionRawValue("--key").Single());
        }

        [Fact]
        public void should_get_error_if_no_value_is_provided_for_key_value_option_for_specified_command()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .AddOptionWithValue("key", 'k', string.Empty)
                .EndCommand()
                .Build();

            CliArgsParsingResult result = parser.Parse(new [] {"command", "-k"});
            
            result.AssertError(CliArgsParsingErrorCode.CannotFindValueForOption, "-k");
        }

        [Fact]
        public void should_get_multiple_key_values_for_specified_command()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .AddOptionWithValue("key-a", 'a', string.Empty)
                .AddOptionWithValue("key-b", 'b', string.Empty)
                .EndCommand()
                .Build();

            CliArgsParsingResult result = parser.Parse(new[] {"command", "--key-a", "value-a", "-b", "value-b"});

            Assert.True(result.IsSuccess);
            Assert.Equal("value-a", result.GetOptionRawValue("-a").Single());
            Assert.Equal("value-b", result.GetOptionRawValue("--key-b").Single());
        }

        
        [Fact]
        public void should_get_error_if_no_value_is_provided_for_multiple_key_value_option_for_specified_command()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .AddOptionWithValue("key-a", 'a', string.Empty)
                .AddOptionWithValue("key-b", 'b', string.Empty)
                .EndCommand()
                .Build();

            CliArgsParsingResult result = parser.Parse(new[] {"command", "--key-a", "value-a", "-b"});

            result.AssertError(CliArgsParsingErrorCode.CannotFindValueForOption, "-b");
        }

        [Fact]
        public void should_get_multiple_key_values_for_default_command()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue("key-a", 'a', string.Empty)
                .AddOptionWithValue("key-b", 'b', string.Empty)
                .EndCommand()
                .Build();

            CliArgsParsingResult result = parser.Parse(new[] {"--key-a", "value-a", "-b", "value-b"});

            Assert.True(result.IsSuccess);
            Assert.Equal("value-a", result.GetOptionRawValue("-a").Single());
            Assert.Equal("value-b", result.GetOptionRawValue("--key-b").Single());
        }
        
        [Fact]
        public void should_get_error_if_no_value_is_provided_for_multiple_key_values_for_default_command()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue("key-a", 'a', string.Empty)
                .AddOptionWithValue("key-b", 'b', string.Empty)
                .EndCommand()
                .Build();

            CliArgsParsingResult result = parser.Parse(new[] {"--key-a", "value-a", "-b"});

            result.AssertError(CliArgsParsingErrorCode.CannotFindValueForOption, "-b");
        }

        [Fact]
        public void should_get_flag_for_specified_command()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .AddFlagOption("flag", 'f', string.Empty)
                .EndCommand()
                .Build();

            CliArgsParsingResult result = parser.Parse(new [] {"command", "-f"});

            Assert.True(result.IsSuccess);
            Assert.True(result.GetFlagValues("--flag"));
        }

        [Fact]
        public void should_get_multiple_flag_for_specified_command()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .AddFlagOption("flag-a", 'a', string.Empty)
                .AddFlagOption("flag-b", 'b', string.Empty)
                .AddFlagOption("flag-c", 'c', string.Empty)
                .EndCommand()
                .Build();

            CliArgsParsingResult result = parser.Parse(new [] {"command", "-a", "-b"});

            Assert.True(result.IsSuccess);
            Assert.True(result.GetFlagValues("--flag-a"));
            Assert.True(result.GetFlagValues("--flag-b"));
            Assert.False(result.GetFlagValues("--flag-c"));
        }

        [Fact]
        public void should_get_multiple_flag_for_default_command()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption("flag-a", 'a', string.Empty)
                .AddFlagOption("flag-b", 'b', string.Empty)
                .AddFlagOption("flag-c", 'c', string.Empty)
                .EndCommand()
                .Build();

            CliArgsParsingResult result = parser.Parse(new [] {"-a", "-b"});

            Assert.True(result.IsSuccess);
            Assert.True(result.GetFlagValues("--flag-a"));
            Assert.True(result.GetFlagValues("--flag-b"));
            Assert.False(result.GetFlagValues("--flag-c"));
        }

        [Fact]
        public void should_get_mixed_options_for_default_command()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue("key-a", 'a', string.Empty)
                .AddFlagOption("flag-b", 'b', string.Empty)
                .AddFlagOption("flag-c", 'c', string.Empty)
                .EndCommand()
                .Build();

            CliArgsParsingResult result = parser.Parse(new[] { "--key-a", "value", "-b" });

            Assert.True(result.IsSuccess);
            Assert.Equal("value", result.GetOptionRawValue("-a").Single());
            Assert.True(result.GetFlagValues("--flag-b"));
            Assert.False(result.GetFlagValues("--flag-c"));
        }

        [Fact]
        public void should_not_support_free_value()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue("key-a", 'a', string.Empty)
                .AddFlagOption("flag-b", 'b', string.Empty)
                .AddFlagOption("flag-c", 'c', string.Empty)
                .EndCommand()
                .Build();

            CliArgsParsingResult result = parser.Parse(new[] { "--key-a", "value", "-b", "free-value" });

            result.AssertError(CliArgsParsingErrorCode.FreeValueNotSupported, "free-value");
        }
    }
}