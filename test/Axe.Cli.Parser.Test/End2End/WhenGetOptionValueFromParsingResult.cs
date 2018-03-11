using System;
using System.Linq;
using Axe.Cli.Parser.Test.Helpers;
using Xunit;

namespace Axe.Cli.Parser.Test.End2End
{
    public class WhenGetOptionValueFromParsingResult
    {
        [Fact]
        public void should_throw_if_option_is_null_when_getting_option_value()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue("option", 'o', string.Empty, true)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new[]{"-o", "value"});

            result.AssertSuccess();

            Assert.Throws<ArgumentNullException>(() => result.GetOptionRawValue(null));
            Assert.Throws<ArgumentNullException>(() => result.GetOptionValue(null));
        }

        [Fact]
        public void should_throw_if_option_is_not_defined_when_getting_option_value()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue("option", 'o', string.Empty, true)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new[]{"-o", "value"});

            result.AssertSuccess();

            Assert.Throws<ArgumentException>(() => result.GetOptionRawValue("--not-defined"));
            Assert.Throws<ArgumentException>(() => result.GetOptionValue("--not-defined"));
        }

        [Fact]
        public void should_throw_if_getting_option_from_a_failure_result()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue("option", 'o', string.Empty, true)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new string[0]);

            Assert.False(result.IsSuccess);

            Assert.Throws<InvalidOperationException>(() => result.GetOptionRawValue("-o"));
            Assert.Throws<InvalidOperationException>(() => result.GetOptionValue("-o"));
        }

        [Fact]
        public void should_get_cannot_find_value_if_no_value_is_provided_for_key_value_option_for_specified_command()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .AddOptionWithValue("key", 'k', string.Empty)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new [] {"command", "-k"});
            
            result.AssertError(ArgsParsingErrorCode.CannotFindValueForOption, "-k");
        }

        [Fact]
        public void should_get_cannot_find_value_if_no_value_is_provided_for_multiple_option_for_specified_command()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .AddOptionWithValue("key-a", 'a', string.Empty)
                .AddOptionWithValue("key-b", 'b', string.Empty)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new[] {"command", "--key-a", "value-a", "-b"});

            result.AssertError(ArgsParsingErrorCode.CannotFindValueForOption, "-b");
        }

        [Fact]
        public void should_get_cannot_find_value_if_no_value_is_provided_for_multiple_key_values_for_default_command()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue("key-a", 'a', string.Empty)
                .AddOptionWithValue("key-b", 'b', string.Empty)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new[] {"--key-a", "value-a", "-b"});

            result.AssertError(ArgsParsingErrorCode.CannotFindValueForOption, "-b");
        }

        [Theory]
        [InlineData("--option")]
        [InlineData("-o")]
        public void should_get_cannot_find_value_if_default_command_kv_option_contains_no_value(
            string argumentExpression)
        {
            // start-(kv-optin)-(EoA)->error

            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue("option", 'o', string.Empty)
                .EndCommand()
                .Build();

            string[] args = { argumentExpression };
            ArgsParsingResult result = parser.Parse(args);

            result.AssertError(
                ArgsParsingErrorCode.CannotFindValueForOption,
                argumentExpression);
        }

        [Fact]
        public void should_get_required_option_not_present_if_required_value_not_present()
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

        [Fact]
        public void should_get_transforming_to_integer_failed()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue("integer", 'i', string.Empty, true, ArgsTransformers.IntegerTransformer)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new [] {"-i", "not_an_integer"});

            Assert.False(result.IsSuccess);
            Assert.Equal(ArgsParsingErrorCode.TransformIntegerValueFailed, result.Error.Code);
            Assert.Equal("not_an_integer", result.Error.Trigger);
        }

        [Fact]
        public void should_get_transforming_to_integer_on_first_value_failed_for_multiple_value()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue("integer", 'i', string.Empty, true, ArgsTransformers.IntegerTransformer)
                .EndCommand()
                .Build();

            ArgsParsingResult result =
                parser.Parse(new[] {"-i", "20", "-i", "not_an_integer", "-i", "another_failure"});

            Assert.False(result.IsSuccess);
            Assert.Equal(ArgsParsingErrorCode.TransformIntegerValueFailed, result.Error.Code);
            Assert.Equal("not_an_integer", result.Error.Trigger);
        }

        [Fact]
        public void should_get_option_raw_value_with_default_command()
        {
            // waiting-value -(any)-> ok

            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue("key", 'k', string.Empty)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new[] {"--key", "value"});

            result.AssertSuccess();
            Assert.Equal("value", result.GetOptionRawValue("--key").First());
            Assert.Equal("value", result.GetOptionRawValue("-k").First());
        }
        
        [Theory]
        [InlineData("--value")]
        [InlineData("-value")]
        public void should_get_option_raw_value_even_if_value_looks_like_options_keys(string optionLikeValue)
        {
            // waiting-value -(any)-> ok

            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue("key", 'k', string.Empty)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new[] { "--key", optionLikeValue });

            result.AssertSuccess();
            Assert.Equal(optionLikeValue, result.GetOptionRawValue("--key").First());
            Assert.Equal(optionLikeValue, result.GetOptionRawValue("-k").First());

            Assert.Throws<ArgumentException>(() => result.GetOptionRawValue("--value"));
            Assert.Throws<ArgumentException>(() => result.GetOptionRawValue("-v"));
        }

        [Fact]
        public void should_get_non_required_optional_raw_value()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue("key", 'k', string.Empty)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(Array.Empty<string>());

            result.AssertSuccess();
            Assert.Empty(result.GetOptionRawValue("--key"));
        }

        [Fact]
        public void should_get_option_raw_value_for_specified_command()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .AddOptionWithValue("key", 'k', string.Empty)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new [] {"command", "-k", "value"});

            result.AssertSuccess();
            Assert.Equal("value", result.GetOptionRawValue("--key").Single());
        }

        [Fact]
        public void should_get_option_raw_value_for_multiple_def_in_specified_command()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .AddOptionWithValue("key-a", 'a', string.Empty)
                .AddOptionWithValue("key-b", 'b', string.Empty)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new[] {"command", "--key-a", "value-a", "-b", "value-b"});

            result.AssertSuccess();
            Assert.Equal("value-a", result.GetOptionRawValue("-a").Single());
            Assert.Equal("value-b", result.GetOptionRawValue("--key-b").Single());
        }

        [Fact]
        public void should_get_option_raw_value_for_multiple_def_in_default_command()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue("key-a", 'a', string.Empty)
                .AddOptionWithValue("key-b", 'b', string.Empty)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new[] {"--key-a", "value-a", "-b", "value-b"});

            result.AssertSuccess();
            Assert.Equal("value-a", result.GetOptionRawValue("-a").Single());
            Assert.Equal("value-b", result.GetOptionRawValue("--key-b").Single());
        }
    }
}