using System;
using Xunit;

namespace Axe.Cli.Parser.Test.End2End
{
    public class WhenDefineCommand
    {
        [Fact]
        public void should_throw_if_command_name_is_null()
        {
            Assert.Throws<ArgumentNullException>(
                () => new ArgsParserBuilder().BeginCommand(null, string.Empty));
        }

        [Theory]
        [InlineData("")]
        [InlineData("-o")]
        [InlineData("with space")]
        [InlineData("multi\nline")]
        [InlineData("with_other_symbol_@")]
        [InlineData("--this-is-an-option")]
        public void should_throw_if_command_name_is_of_invalid_format(string incorrectCommandSymbol)
        {
            Assert.Throws<ArgumentException>(
                () => new ArgsParserBuilder().BeginCommand(incorrectCommandSymbol, string.Empty));
        }

        [Fact]
        public void should_throw_if_register_options_conflicts()
        {
            CommandBuilder builder = new ArgsParserBuilder()
                .BeginCommand("valid_symbol", null)
                .AddOptionWithValue("message", 'm', string.Empty);

            Assert.Throws<ArgumentException>(() => builder.AddOptionWithValue("message", 'p', string.Empty));
        }

        [Fact]
        public void should_throw_if_register_flag_option_conflicts()
        {
            CommandBuilder builder = new ArgsParserBuilder()
                .BeginCommand("valid_symbol", null)
                .AddFlagOption("message", 'm', string.Empty);

            Assert.Throws<ArgumentException>(() => builder.AddOptionWithValue("message", 'p', string.Empty));
        }

        [Fact]
        public void should_throw_if_register_flag_option_conflicts2()
        {
            CommandBuilder builder = new ArgsParserBuilder()
                .BeginCommand("valid_symbol", null)
                .AddOptionWithValue("message", 'm', string.Empty);

            Assert.Throws<ArgumentException>(() => builder.AddFlagOption("message", 'p', string.Empty));
        }

        [Theory]
        [InlineData("o", null, "v", null, "--o", "--v")]
        [InlineData("o", null, "v", 'o', "--o", "-o")]
        [InlineData("o", 'p', "v", 'o', "--o", "-o")]
        [InlineData("o", 'p', "v", 'q', "--o", "-q")]
        public void should_be_no_conflict(string s1, char? a1, string s2, char? a2, string argument1, string argument2)
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue(s1, a1, string.Empty)
                .AddOptionWithValue(s2, a2, string.Empty)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new [] {argument1, "value1", argument2, "value2"});

            Assert.Equal("value1", result.GetFirstOptionValue<string>(argument1));
            Assert.Equal("value2", result.GetFirstOptionValue<string>(argument2));
        }

        [Fact]
        public void should_throw_if_both_full_form_and_abbreviation_form_are_null()
        {
            CommandBuilder builder = new ArgsParserBuilder()
                .BeginDefaultCommand();

            Assert.Throws<ArgumentException>(() => builder.AddOptionWithValue(null, null, string.Empty));
        }

        [Theory]
        [InlineData("")]
        [InlineData("-o")]
        [InlineData("with space")]
        [InlineData("multi\nline")]
        [InlineData("with_other_symbol_@")]
        [InlineData("--this-is-an-option")]
        public void should_throw_if_full_form_is_of_invalid_pattern(string invalidFullForm)
        {
            CommandBuilder builder = new ArgsParserBuilder()
                .BeginDefaultCommand();

            Assert.Throws<ArgumentException>(() => builder.AddOptionWithValue(invalidFullForm, null, string.Empty));
        }

        [Theory]
        [InlineData('!')]
        [InlineData('-')]
        [InlineData('0')]
        public void should_throw_if_abbr_form_is_of_invalid_pattern(char abbrForm)
        {
            CommandBuilder builder = new ArgsParserBuilder()
                .BeginDefaultCommand();

            Assert.Throws<ArgumentException>(() => builder.AddOptionWithValue(null, abbrForm, string.Empty));
        }

        [Fact]
        public void should_throw_if_register_flags_conflicts()
        {
            CommandBuilder builder = new ArgsParserBuilder()
                .BeginCommand("valid_symbol", null)
                .AddFlagOption("message", 'm', string.Empty);

            Assert.Throws<ArgumentException>(() => builder.AddFlagOption("message", 'p', string.Empty));
        }

        [Fact]
        public void should_not_be_null_for_both_fullname_and_abbreviation_for_flag()
        {
            CommandBuilder builder = new ArgsParserBuilder().BeginDefaultCommand();
            Assert.Throws<ArgumentException>(() => builder.AddFlagOption(null, null, string.Empty));
        }

        [Theory]
        [InlineData("")]
        [InlineData("-o")]
        [InlineData("with space")]
        [InlineData("multi\nline")]
        [InlineData("with_other_symbol_@")]
        [InlineData("--this-is-an-option")]
        public void should_throw_if_full_form_is_invalid_for_flag(string invalidFullForm)
        {
            CommandBuilder builder = new ArgsParserBuilder().BeginDefaultCommand();
            Assert.Throws<ArgumentException>(() => builder.AddFlagOption(invalidFullForm, null, string.Empty));
        }

        [Theory]
        [InlineData('!')]
        [InlineData('-')]
        [InlineData('0')]
        public void should_throw_if_abbr_form_is_invalid_for_flag(char invalidAbbrForm)
        {
            CommandBuilder builder = new ArgsParserBuilder().BeginDefaultCommand();
            Assert.Throws<ArgumentException>(() => builder.AddFlagOption("name", invalidAbbrForm, string.Empty));
        }

        [Fact]
        public void should_throw_if_free_value_definition_is_added_while_disabling_free_values()
        {
            CommandBuilder builder = new ArgsParserBuilder().BeginDefaultCommand()
                .AddFreeValue("free_value_name", string.Empty);

            Assert.Throws<InvalidOperationException>(() => builder.ConfigFreeValue());
        }

        [Fact]
        public void should_throw_if_free_value_name_is_null()
        {
            CommandBuilder builder = new ArgsParserBuilder().BeginDefaultCommand();

            Assert.Throws<ArgumentNullException>(() => builder.AddFreeValue(null, string.Empty));
        }

        [Theory]
        [InlineData("name")]
        [InlineData("namE")]
        [InlineData("NamE")]
        public void should_throw_if_free_value_conflict(string conflictName)
        {
            CommandBuilder builder = new ArgsParserBuilder().BeginCommand("command", string.Empty);
            builder.AddFreeValue("name", "original");
            Assert.Throws<ArgumentException>(() => builder.AddFreeValue(conflictName, "conflict"));
        }

        [Fact]
        public void should_throw_if_command_conflicts()
        {
            ArgsParserBuilder builder = new ArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .EndCommand();

            Assert.Throws<ArgumentException>(() => builder.BeginCommand("command", string.Empty).EndCommand());
        }

        [Fact]
        public void should_throw_if_default_command_has_been_set()
        {
            ArgsParserBuilder builder = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .EndCommand();

            Assert.Throws<InvalidOperationException>(() => builder.BeginDefaultCommand().EndCommand());
        }
    }
}