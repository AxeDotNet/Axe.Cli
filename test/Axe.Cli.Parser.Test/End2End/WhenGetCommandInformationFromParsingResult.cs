using System;
using Axe.Cli.Parser.Test.Helpers;
using Xunit;

namespace Axe.Cli.Parser.Test.End2End
{
    public class WhenGetCommandInformationFromParsingResult
    {
        [Fact]
        public void should_get_not_match_any_command_error_if_command_does_not_match_without_default_command()
        {
            // start-(unresolved command)->no default command ? error

            const string commandName = "command";
            ArgsParser parser = new ArgsParserBuilder()
                .BeginCommand(commandName, string.Empty)
                .EndCommand()
                .Build();

            string[] args = {"not_matched_command"};

            ArgsParsingResult result = parser.Parse(args);

            result.AssertError(
                ArgsParsingErrorCode.DoesNotMatchAnyCommand,
                "not_matched_command");
        }

        [Fact]
        public void should_get_not_match_any_command_error_for_non_argument_if_default_command_is_not_set()
        {
            // start-(EoA)->no default command ? error

            var parser = new ArgsParserBuilder().Build();

            ArgsParsingResult result = parser.Parse(Array.Empty<string>());

            result.AssertError(
                ArgsParsingErrorCode.DoesNotMatchAnyCommand,
                "Unexpected end of arguments.");
        }

        [Theory]
        [InlineData("c")]
        [InlineData("word")]
        [InlineData("multi-word")]
        [InlineData("multi_word")]
        [InlineData("underscore_tail_")]
        [InlineData("dash_tail-")]
        public void should_get_command_symbol(string validSymbol)
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginCommand(validSymbol, string.Empty).EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new [] {validSymbol});

            Assert.Equal(validSymbol, result.Command.Symbol);
        }

        [Fact]
        public void should_get_default_command_symbol()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand().EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(Array.Empty<string>());

            result.AssertSuccess();
            Assert.True(result.Command.IsDefaultCommand());
            Assert.Null(result.Command.Symbol);
        }

        [Theory]
        [InlineData("line1\nline2", "line1 line2")]
        [InlineData("line1\r\nline2", "line1 line2")]
        public void should_single_lined_the_description(string multiLined, string expected)
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginCommand("command", multiLined).EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new [] {"command"});
            Assert.Equal(expected, result.Command.Description);
        }

        [Fact]
        public void should_accept_null_description()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginCommand("valid_symbol", null).EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new []{"valid_symbol"});

            Assert.Equal(string.Empty, result.Command.Description);
        }
    }
}