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
    }
}