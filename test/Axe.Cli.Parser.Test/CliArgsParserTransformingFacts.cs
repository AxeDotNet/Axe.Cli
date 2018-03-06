using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Axe.Cli.Parser.Test
{
    public class CliArgsParserTransformingFacts
    {
        [Fact]
        public void should_transform_integer_values()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue("integer", 'i', string.Empty, true, CliArgsTransformers.IntegerTransformer)
                .EndCommand()
                .Build();

            CliArgsParsingResult result = parser.Parse(new [] {"-i", "12"});

            Assert.True(result.IsSuccess);
            Assert.Equal(12, result.GetOptionValues("-i").Single());
        }

        [Fact]
        public void should_transform_multiple_integer_values()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue("integer", 'i', string.Empty, true, CliArgsTransformers.IntegerTransformer)
                .EndCommand()
                .Build();

            CliArgsParsingResult result = parser.Parse(new [] {"-i", "12", "--integer", "13"});

            Assert.True(result.IsSuccess);
            Assert.Equal(new object[] {12, 13}, result.GetOptionValues("-i"));
        }

        [Fact]
        public void should_fail_if_transforming_to_integer_failed()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue("integer", 'i', string.Empty, true, CliArgsTransformers.IntegerTransformer)
                .EndCommand()
                .Build();

            CliArgsParsingResult result = parser.Parse(new [] {"-i", "not_an_integer"});

            Assert.False(result.IsSuccess);
            Assert.Equal(CliArgsParsingErrorCode.TransformIntegerValueFailed, result.Error.Code);
            Assert.Equal("not_an_integer", result.Error.Trigger);
        }

        [Fact]
        public void should_fail_first_value_if_transforming_to_integer_failed_for_multiple_value()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue("integer", 'i', string.Empty, true, CliArgsTransformers.IntegerTransformer)
                .EndCommand()
                .Build();

            CliArgsParsingResult result =
                parser.Parse(new[] {"-i", "20", "-i", "not_an_integer", "-i", "another_failure"});

            Assert.False(result.IsSuccess);
            Assert.Equal(CliArgsParsingErrorCode.TransformIntegerValueFailed, result.Error.Code);
            Assert.Equal("not_an_integer", result.Error.Trigger);
        }

        [Fact]
        public void should_be_ok_if_transforming_empty_option_value()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue("integer", 'i', string.Empty, false, CliArgsTransformers.IntegerTransformer)
                .EndCommand()
                .Build();

            CliArgsParsingResult result = parser.Parse(new string[0]);

            Assert.True(result.IsSuccess);
            Assert.Empty(result.GetOptionValues<int>("--integer"));
        }
    }
}