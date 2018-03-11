﻿using System;
using System.Linq;
using Axe.Cli.Parser.Test.Helpers;
using Xunit;

namespace Axe.Cli.Parser.Test.End2End
{
    public class WhenGetBindedFreeValueDefinitionFromParsingResult
    {
        [Fact]
        public void should_get_the_named_definition_basic_information()
        {
            const string name = "free_value_name";
            const string description = "description";

            ArgsParser parser = new ArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .AddFreeValue(name, description, ArgsTransformers.IntegerTransformer)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new[] {"command", "123"});

            result.AssertSuccess();

            Assert.True(result.Command.AllowFreeValue);
            IFreeValueDefinitionMetadata definition = result.Command.GetRegisteredFreeValuesMetadata().Single();

            Assert.Equal(name, definition.Name);
            Assert.Equal(description, definition.Description);
            Assert.Same(ArgsTransformers.IntegerTransformer, definition.Transformer);
        }

        [Fact]
        public void should_remove_the_line_breaks_in_description()
        {
            const string name = "free_value_name";
            const string description = "description\r\nanother line";

            ArgsParser parser = new ArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .AddFreeValue(name, description, ArgsTransformers.IntegerTransformer)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new[] {"command", "123"});

            string actualDescription = result.Command.GetRegisteredFreeValuesMetadata().Single().Description;
            Assert.Equal("description another line", actualDescription);
        }

        [Fact]
        public void should_turn_null_to_empty_description()
        {
            const string name = "free_value_name";

            ArgsParser parser = new ArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .AddFreeValue(name, null, ArgsTransformers.IntegerTransformer)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new[] {"command", "123"});

            string actualDescription = result.Command.GetRegisteredFreeValuesMetadata().Single().Description;
            Assert.Equal(string.Empty, actualDescription);
        }

        [Theory]
        [InlineData("name")]
        [InlineData("namE")]
        [InlineData("NamE")]
        public void should_determine_conflict_free_value(string conflictName)
        {
            CommandBuilder builder = new ArgsParserBuilder().BeginCommand("command", string.Empty);
            builder.AddFreeValue("name", "original");
            Assert.Throws<ArgumentException>(() => builder.AddFreeValue(conflictName, "conflict"));
        }
    }
}