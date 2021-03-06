﻿using System.Linq;
using Axe.Cli.Parser.Test.Helpers;
using Axe.Cli.Parser.Transformers;
using Xunit;

namespace Axe.Cli.Parser.Test.End2End
{
    public class WhenGettingTranslatedValues
    {
        [Fact]
        public void should_transform_integer_values()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue("integer", 'i', string.Empty, true, IntegerTransformer.Instance)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new [] {"-i", "12"});

            result.AssertSuccess();
            Assert.Equal(12, result.GetOptionValue("-i").Single());
            Assert.Equal(12, result.GetFirstOptionValue<int>("--integer"));
        }

        [Fact]
        public void should_transform_multiple_integer_values()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue("integer", 'i', string.Empty, true, IntegerTransformer.Instance)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new [] {"-i", "12", "--integer", "13"});

            result.AssertSuccess();
            Assert.Equal(new [] {12, 13}, result.GetOptionValue<int>("-i"));
        }

        [Fact]
        public void should_be_ok_if_transforming_empty_option_value()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue("integer", 'i', string.Empty, false, IntegerTransformer.Instance)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new string[0]);

            result.AssertSuccess();
            Assert.Empty(result.GetOptionValue<int>("--integer"));
        }
    }
}