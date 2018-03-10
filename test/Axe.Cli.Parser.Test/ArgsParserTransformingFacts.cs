using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Axe.Cli.Parser.Test
{
    public class ArgsParserTransformingFacts
    {
        [Fact]
        public void should_transform_integer_values()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue("integer", 'i', string.Empty, true, ArgsTransformers.IntegerTransformer)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new [] {"-i", "12"});

            Assert.True(result.IsSuccess);
            Assert.Equal(12, result.GetOptionValue("-i").Single());
            Assert.Equal(12, result.GetOptionValue<int>("--integer"));
        }

        [Fact]
        public void should_transform_multiple_integer_values()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue("integer", 'i', string.Empty, true, ArgsTransformers.IntegerTransformer)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new [] {"-i", "12", "--integer", "13"});

            Assert.True(result.IsSuccess);
            Assert.Equal(new [] {12, 13}, result.GetOptionValues<int>("-i"));
        }

        [Fact]
        public void should_fail_if_transforming_to_integer_failed()
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
        public void should_fail_first_value_if_transforming_to_integer_failed_for_multiple_value()
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
        public void should_be_ok_if_transforming_empty_option_value()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue("integer", 'i', string.Empty, false, ArgsTransformers.IntegerTransformer)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new string[0]);

            Assert.True(result.IsSuccess);
            Assert.Empty(result.GetOptionValues<int>("--integer"));
        }
    }
}