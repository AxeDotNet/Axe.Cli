using System;
using System.Linq;
using Axe.Cli.Parser.Test.Helpers;
using Axe.Cli.Parser.Transformers;
using Xunit;

namespace Axe.Cli.Parser.Test.End2End
{
    public class WhenGetFreeValueFromParsingResult
    {
        [Fact]
        public void should_throw_if_name_is_null_when_getting_free_value()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFreeValue("free_value_name", string.Empty)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new [] {"value"});

            result.AssertSuccess();

            Assert.Throws<ArgumentNullException>(() => result.GetFreeValue(null));
            Assert.Throws<ArgumentNullException>(() => result.GetFreeRawValue(null));
        }

        [Fact]
        public void should_throw_if_name_is_not_defined_when_getting_free_value()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFreeValue("free_value_name", string.Empty)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new [] {"value"});

            result.AssertSuccess();

            Assert.Throws<ArgumentException>(() => result.GetFreeValue("undefined"));
            Assert.Throws<ArgumentException>(() => result.GetFreeRawValue("undefined"));
        }

        [Fact]
        public void should_throw_if_getting_free_value_in_a_failure_result()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFreeValue("free_value_name", string.Empty)
                .AddOptionWithValue("option", 'o', string.Empty, true)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new [] {"value"});

            Assert.False(result.IsSuccess);

            Assert.Throws<InvalidOperationException>(() => result.GetFreeValue("free_value_name"));
            Assert.Throws<InvalidOperationException>(() => result.GetFreeRawValue("free_value_name"));
        }

        [Fact]
        public void should_throw_if_getting_undefined_free_values_in_a_failure_result()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .ConfigFreeValue(true)
                .AddOptionWithValue("option", 'o', string.Empty, true)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new [] {"value"});

            Assert.False(result.IsSuccess);

            Assert.Throws<InvalidOperationException>(() => result.GetUndefinedFreeValues());
        }

        [Fact]
        public void should_get_free_value_not_supported_if_free_value_is_not_configured()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue("key-a", 'a', string.Empty)
                .AddFlagOption("flag-b", 'b', string.Empty)
                .AddFlagOption("flag-c", 'c', string.Empty)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new[] { "--key-a", "value", "-b", "free-value" });

            result.AssertError(ArgsParsingErrorCode.FreeValueNotSupported, "free-value");
        }

        [Fact]
        public void should_get_free_value_not_supported_for_specified_command()
        {
            // start-(unresolved command)->default command ? error

            const string commandName = "command";
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .EndCommand()
                .BeginCommand(commandName, string.Empty)
                .EndCommand()
                .Build();

            string[] args = { "not_matched_command" };

            ArgsParsingResult result = parser.Parse(args);

            result.AssertError(
                ArgsParsingErrorCode.FreeValueNotSupported,
                "not_matched_command");
        }

        [Fact]
        public void should_get_free_value_for_default_command()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .ConfigFreeValue(true)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new[] {"free-value"});

            result.AssertSuccess();
            Assert.Equal(new []{"free-value"}, result.GetUndefinedFreeValues());
        }

        [Fact]
        public void should_get_free_value_continuously_for_default_command()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .ConfigFreeValue(true)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new[] {"free-value1", "free-value2"});

            result.AssertSuccess();
            Assert.Equal(new []{"free-value1", "free-value2"}, result.GetUndefinedFreeValues());
        }

        [Fact]
        public void should_get_free_value_for_specified_command()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .ConfigFreeValue(true)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new[] {"command", "free-value"});

            result.AssertSuccess();
            Assert.Equal(new [] {"free-value"}, result.GetUndefinedFreeValues());
        }
        
        [Fact]
        public void should_get_free_value_continuously_for_specified_command()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .ConfigFreeValue(true)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new[] {"command", "free-value1", "free-value2"});

            result.AssertSuccess();
            Assert.Equal(new [] {"free-value1", "free-value2"}, result.GetUndefinedFreeValues());
        }

        [Fact]
        public void should_get_as_free_value_even_if_it_looks_like_option()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .ConfigFreeValue(true)
                .AddFlagOption("flag", 'f', string.Empty)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new [] {"command", "-f", "-a"});
            
            result.AssertSuccess();
            Assert.Equal(new [] {"-a"}, result.GetUndefinedFreeValues());
            Assert.True(result.GetFlagValue("--flag"));
        }

        [Fact]
        public void should_get_as_free_value_after_one_free_value()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .ConfigFreeValue(true)
                .AddFlagOption("flag", 'f', string.Empty)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new [] {"command", "-a", "-f"});
            
            result.AssertSuccess();
            Assert.Equal(new [] {"-a", "-f"}, result.GetUndefinedFreeValues());
            Assert.False(result.GetFlagValue("--flag"));
        }

        [Fact]
        public void should_capture_single_free_variable()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .AddFreeValue("name", "description")
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new[] {"command", "free_value"});

            result.AssertSuccess();
            Assert.Equal("free_value", result.GetFreeRawValue("name"));
        }

        [Fact]
        public void should_capture_multiple_free_values()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .AddFreeValue("name", "description")
                .AddFreeValue("age", "description")
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new[] {"command", "name_value", "age_value"});

            result.AssertSuccess();
            Assert.Equal("name_value", result.GetFreeRawValue("name"));
            Assert.Equal("age_value", result.GetFreeRawValue("age"));
        }

        [Fact]
        public void should_capture_multiple_free_values_with_undefined_ones()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .AddFreeValue("name", "description")
                .AddFreeValue("age", "description")
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(
                new[] {"command", "name_value", "age_value", "undefined_value1", "undefined_value2"});

            result.AssertSuccess();
            Assert.Equal("name_value", result.GetFreeRawValue("name"));
            Assert.Equal("age_value", result.GetFreeRawValue("age"));
            Assert.Equal(new object[] {"undefined_value1", "undefined_value2"}, result.GetUndefinedFreeValues());
        }

        [Fact]
        public void should_ignore_uncaptured_definitions()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .AddFreeValue("name", string.Empty)
                .AddFreeValue("uncaptured_definition", string.Empty)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new[] {"command", "name_value"});

            result.AssertSuccess();
            Assert.Equal("name_value", result.GetFreeRawValue("name"));
            Assert.Empty(result.GetFreeValue("uncaptured_definition"));
            Assert.Equal(string.Empty, result.GetFreeRawValue("uncaptured_definition"));
        }

        [Fact]
        public void should_throw_if_get_a_undefined_free_value()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .ConfigFreeValue(true)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new [] {"command", "free_value"});

            result.AssertSuccess();
            Assert.Throws<ArgumentException>(() => result.GetFreeValue("undefined_name"));
            Assert.Throws<ArgumentException>(() => result.GetFreeRawValue("undefined_name"));
        }

        [Fact]
        public void should_support_transformer_on_free_values()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .AddFreeValue("name", string.Empty, false, IntegerTransformer.Instance)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new [] {"command", "123"});

            result.AssertSuccess();
            Assert.Equal(123, result.GetFreeValue<int>("name").Single());
            Assert.Equal(123, result.GetFirstFreeValue<int>("name"));
        }

        [Fact]
        public void should_support_mandatory_free_values_for_default_command()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFreeValue("name", string.Empty, true, DefaultTransformer.Instance)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new [] {"free_value"});
            
            result.AssertSuccess();
            Assert.Equal("free_value", result.GetFirstFreeValue<string>("name"));
        }

        [Fact]
        public void should_throw_if_mandatory_free_value_is_not_present()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFreeValue("name", string.Empty, true, DefaultTransformer.Instance)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new string[0]);
            
            result.AssertError(ArgsParsingErrorCode.RequiredFreeValueNotPresent, "<name>");
        }

        [Fact]
        public void should_recognize_free_value_for_a_flag_like_number()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .AddOptionWithValue("option", 'o', string.Empty, true)
                .AddFreeValue("number", string.Empty)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new[] {"command", "--option", "option_value", "-1"});
            
            result.AssertSuccess();
            Assert.Equal("option_value", result.GetFirstOptionValue<string>("-o"));
            Assert.Equal("-1", result.GetFreeRawValue("number"));
        }
    }
}