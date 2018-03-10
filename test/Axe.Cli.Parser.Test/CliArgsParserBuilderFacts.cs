﻿using System;
using Xunit;

namespace Axe.Cli.Parser.Test
{
    public class CliArgsParserBuilderFacts
    {
        [Fact]
        public void should_throw_if_command_conflicts()
        {
            CliArgsParserBuilder builder = new CliArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .EndCommand();

            Assert.Throws<ArgumentException>(() => builder.BeginCommand("command", string.Empty).EndCommand());
        }

        [Fact]
        public void should_throw_if_default_command_has_been_set()
        {
            CliArgsParserBuilder builder = new CliArgsParserBuilder()
                .BeginDefaultCommand()
                .EndCommand();

            Assert.Throws<InvalidOperationException>(() => builder.BeginDefaultCommand().EndCommand());
        }

        [Fact]
        public void should_not_be_null_symbol()
        {
            Assert.Throws<ArgumentNullException>(() => new CliArgsParserBuilder().BeginCommand(null, string.Empty));
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
                () => new CliArgsParserBuilder().BeginCommand(incorrectCommandSymbol, string.Empty));
        }

        [Theory]
        [InlineData("line1\nline2", "line1 line2")]
        [InlineData("line1\r\nline2", "line1 line2")]
        public void should_single_lined_the_description(string multiLined, string expected)
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginCommand("command", multiLined).EndCommand()
                .Build();

            CliArgsParsingResult result = parser.Parse(new [] {"command"});
            Assert.Equal(expected, result.Command.Description);
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
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginCommand(validSymbol, string.Empty).EndCommand()
                .Build();

            CliArgsParsingResult result = parser.Parse(new [] {validSymbol});

            Assert.Equal(validSymbol, result.Command.Symbol);
        }

        [Fact]
        public void should_accept_null_description()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginCommand("valid_symbol", null).EndCommand()
                .Build();

            CliArgsParsingResult result = parser.Parse(new []{"valid_symbol"});

            Assert.Equal(string.Empty, result.Command.Description);
        }

        [Fact]
        public void should_throw_if_register_options_conflicts()
        {
            CliCommandBuilder builder = new CliArgsParserBuilder()
                .BeginCommand("valid_symbol", null)
                .AddOptionWithValue("message", 'm', string.Empty);

            Assert.Throws<ArgumentException>(() => builder.AddOptionWithValue("message", 'p', string.Empty));
        }

        [Fact]
        public void should_throw_if_register_flags_conflicts()
        {
            CliCommandBuilder builder = new CliArgsParserBuilder()
                .BeginCommand("valid_symbol", null)
                .AddFlagOption("message", 'm', string.Empty);

            Assert.Throws<ArgumentException>(() => builder.AddFlagOption("message", 'p', string.Empty));
        }

        [Fact]
        public void should_throw_if_register_flag_option_conflicts()
        {
            CliCommandBuilder builder = new CliArgsParserBuilder()
                .BeginCommand("valid_symbol", null)
                .AddFlagOption("message", 'm', string.Empty);

            Assert.Throws<ArgumentException>(() => builder.AddOptionWithValue("message", 'p', string.Empty));
        }

        [Fact]
        public void should_throw_if_register_flag_option_conflicts2()
        {
            CliCommandBuilder builder = new CliArgsParserBuilder()
                .BeginCommand("valid_symbol", null)
                .AddOptionWithValue("message", 'm', string.Empty);

            Assert.Throws<ArgumentException>(() => builder.AddFlagOption("message", 'p', string.Empty));
        }

        [Fact]
        public void should_not_be_null_for_both_fullname_and_abbreviation()
        {
            CliCommandBuilder builder = new CliArgsParserBuilder().BeginDefaultCommand();
            Assert.Throws<ArgumentException>(() => builder.AddFlagOption(null, null, string.Empty));
        }

        [Fact]
        public void should_not_contains_dash_in_full_symbol()
        {
            CliCommandBuilder builder = new CliArgsParserBuilder().BeginDefaultCommand();
            Assert.Throws<ArgumentException>(() => builder.AddFlagOption("-name", 'c', string.Empty));
        }

        [Fact]
        public void should_not_be_dash_for_abbreviation()
        {
            CliCommandBuilder builder = new CliArgsParserBuilder().BeginDefaultCommand();
            Assert.Throws<ArgumentException>(() => builder.AddFlagOption("name", '-', string.Empty));
        }

        [Theory]
        [InlineData("o", 'o', "-o")]
        [InlineData("word", 'w', "-w")]
        [InlineData("word-with-dash", 'w', "-w")]
        [InlineData("word-with-tail-dash-", 'w', "-w")]
        [InlineData("word_with_lodash", 'w', "-w")]
        [InlineData("word_with_tail_lodash_", 'w', "-w")]
        [InlineData("_word_with_lodash_prefix", 'w', "-w")]
        [InlineData("not_null", null, "--not_null")]
        [InlineData(null, 'n', "-n")]
        public void should_create_valid_symbol(string symbol, char? abbr, string argument)
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption(symbol, abbr, null)
                .EndCommand()
                .Build();
            
            CliArgsParsingResult result = parser.Parse(new[] {argument});

            Assert.True(result.GetFlagValues(argument));
        }

        [Theory]
        [InlineData("o", 'o', "o", 'o')]                // totally equal
        [InlineData("o", 'o', "O", 'O')]                // upper case
        [InlineData("o", 'o', "v", 'o')]                // abbr equal
        [InlineData(null, 'o', "v", 'o')]               // abbr equal
        [InlineData("o", 'o', "o", 'v')]                // symbol equal
        [InlineData("o", null, "o", 'v')]               // symbol equal
        public void should_determine_conflict(string s1, char? a1, string s2, char? a2)
        {
            CliCommandBuilder builder = new CliArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption(s1, a1, string.Empty);

            Assert.Throws<ArgumentException>(() => builder.AddFlagOption(s2, a2, string.Empty));
        }

        [Theory]
        [InlineData("o", null, "v", null, "--o", "--v")]
        [InlineData("o", null, "v", 'o', "--o", "-o")]
        [InlineData("o", 'p', "v", 'o', "--o", "-o")]
        [InlineData("o", 'p', "v", 'q', "--o", "-q")]
        public void should_be_no_conflict(string s1, char? a1, string s2, char? a2, string argument1, string argument2)
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue(s1, a1, string.Empty)
                .AddOptionWithValue(s2, a2, string.Empty)
                .EndCommand()
                .Build();

            CliArgsParsingResult result = parser.Parse(new [] {argument1, "value1", argument2, "value2"});

            Assert.Equal("value1", result.GetOptionValue<string>(argument1));
            Assert.Equal("value2", result.GetOptionValue<string>(argument2));
        }
    }
}