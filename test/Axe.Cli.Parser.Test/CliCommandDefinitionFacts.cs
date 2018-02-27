using System;
using System.Linq;
using Xunit;

namespace Axe.Cli.Parser.Test
{
    public class CliCommandDefinitionFacts
    {
        [Fact]
        public void should_not_be_null_symbol()
        {
            Assert.Throws<ArgumentNullException>(
                () => new CliCommandDefinition(null, string.Empty));
        }

        [Theory]
        [InlineData("")]
        [InlineData("-o")]
        [InlineData("with space")]
        [InlineData("multi\nline")]
        [InlineData("with_other_symbol_@")]
        [InlineData("--this-is-an-option")]
        public void should_not_be_incorrect_format(string incorrectCommandSymbol)
        {
            Assert.Throws<ArgumentException>(
                () => new CliCommandDefinition("--this-is-an-option", string.Empty));
        }

        [Theory]
        [InlineData("line1\nline2", "line1 line2")]
        [InlineData("line1\r\nline2", "line1 line2")]
        public void should_single_lined_the_description(string multiLined, string expected)
        {
            var definition = new CliCommandDefinition("command", multiLined);

            Assert.Equal(expected, definition.Description);
        }

        [Theory]
        [InlineData("c")]
        [InlineData("word")]
        [InlineData("multi-word")]
        [InlineData("multi_word")]
        [InlineData("underscore_tail_")]
        [InlineData("dash_tail-")]
        public void should_accept_valid_command_symbol(string validSymbol)
        {
            var definition = new CliCommandDefinition(validSymbol, string.Empty);

            Assert.Equal(validSymbol, definition.Symbol);
        }

        [Fact]
        public void should_register_option_if_not_conflict()
        {
            var commandDefinition = new CliCommandDefinition("commit", "commit something");
            var optionDefinition = new CliOptionDefinition("message", 'm', "Specify the message for the commit.");
            commandDefinition.RegisterOption(optionDefinition);

            Assert.True(commandDefinition.GetRegisteredOptions().Any(o => ReferenceEquals(o, optionDefinition)));
        }

        [Fact]
        public void should_throw_if_register_option_conflicts()
        {
            var commandDefinition = new CliCommandDefinition("commit", "commit something");
            commandDefinition.RegisterOption(
                new CliOptionDefinition("message", 'm', "Specify the message for the commit."));

            Assert.Throws<ArgumentException>(() => commandDefinition.RegisterOption(
                new CliOptionDefinition("message", 'p', "This is a conflict definition")));
        }

        [Theory]
        [InlineData("command", "command")]
        [InlineData("Command", "commAnd")]
        public void should_conflict_if_command_symbol_matches(string symbol1, string symbol2)
        {
            Assert.True(new CliCommandDefinition(symbol1, string.Empty)
                .IsConflict(new CliCommandDefinition(symbol2, string.Empty)));
        }

        [Fact]
        public void should_conflict_with_any_other_type_of_command()
        {
            Assert.True(new CliCommandDefinition("command", "description")
                .IsConflict(new CliDefaultCommandDefinition()));
        }
    }
}