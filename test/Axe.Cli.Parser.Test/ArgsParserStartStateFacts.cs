using System;
using Axe.Cli.Parser.Test.Helpers;
using Xunit;

namespace Axe.Cli.Parser.Test
{
    public class ArgsParserStartStateFacts
    {
        /// <summary>
        /// start-(command)->ok
        /// </summary>
        [Fact]
        public void should_get_command_from_parsing_result()
        {
            const string commandName = "command";
            ArgsParser parser = new ArgsParserBuilder()
                .BeginCommand(commandName, string.Empty)
                .EndCommand()
                .Build();

            string[] args = { commandName };

            ArgsParsingResult result = parser.Parse(args);

            Assert.True(result.IsSuccess);
            Assert.Equal(commandName, result.Command.Symbol);
        }

        /// <summary>
        /// start-(unresolved command)->no default command ? error
        /// </summary>
        [Fact]
        public void should_be_error_if_command_does_not_match_without_default_command()
        {
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

        /// <summary>
        /// start-(unresolved command)->default command ? error
        /// </summary>
        [Fact]
        public void should_be_error_for_free_values()
        {
            const string commandName = "command";
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .EndCommand()
                .BeginCommand(commandName, string.Empty)
                .EndCommand()
                .Build();

            string[] args = { "not_matched_command" };

            ArgsParsingResult result = parser.Parse(args);

            result.AssertError(
                ArgsParsingErrorCode.FreeValueNotSupported,
                "not_matched_command");
        }

        /// <summary>
        /// start-(kv-optin)-(EoA)->error
        /// </summary>
        [Theory]
        [InlineData("--option")]
        [InlineData("-o")]
        public void should_be_error_if_default_command_kv_option_contains_no_value(
            string argumentExpression)
        {
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

        /// <summary>
        /// start-(flag-option)-(EoA)->ok
        /// </summary>
        [Theory]
        [InlineData("--flag")]
        [InlineData("-f")]
        public void should_be_ok_for_flag_with_default_command(string argumentExpression)
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption("flag", 'f', string.Empty)
                .EndCommand()
                .Build();
             
            string[] args = { argumentExpression };
            ArgsParsingResult result = parser.Parse(args);

            Assert.True(result.IsSuccess);
            Assert.True(result.GetFlagValues(argumentExpression));
        }

        /// <summary>
        /// start-(abbr-flag-options)-(EoA)->ok
        /// </summary>
        [Fact]
        public void should_be_ok_for_multiple_abbr_form_flags_with_default_command()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption("recursive", 'r', string.Empty)
                .AddFlagOption("force", 'f', string.Empty)
                .EndCommand()
                .Build();

            string[] args = {"-rf"};
            ArgsParsingResult result = parser.Parse(args);

            Assert.True(result.IsSuccess);
            Assert.True(result.GetFlagValues("--recursive"));
            Assert.True(result.GetFlagValues("--force"));
        }
        
        /// <summary>
        /// start-(dup-flag-options)->error
        /// </summary>
        [Fact]
        public void should_be_error_for_duplicated_flags_with_default_command()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption("flag", 'f', string.Empty)
                .EndCommand()
                .Build();
             
            string[] args = { "-ff" };
            ArgsParsingResult result = parser.Parse(args);

            Assert.False(result.IsSuccess);
            result.AssertError(ArgsParsingErrorCode.DuplicateFlagsInArgs, "-ff");
        }
        
        /// <summary>
        /// start-(flag-option)->(EoA)->other flag option should be false.
        /// </summary>
        [Fact]
        public void should_be_ok_for_non_specified_flags_with_default_command()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption("flag", 'f', string.Empty)
                .AddFlagOption("other-flag", 'o', string.Empty)
                .EndCommand()
                .Build();
             
            string[] args = { "-f" };
            ArgsParsingResult result = parser.Parse(args);

            Assert.True(result.IsSuccess);
            Assert.True(result.GetFlagValues("-f"));
            Assert.False(result.GetFlagValues("-o"));
        }

        /// <summary>
        /// start-(EoA)->ok
        /// </summary>
        [Fact]
        public void should_be_ok_for_non_argument_if_default_command_is_set()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand().EndCommand().Build();

            ArgsParsingResult result = parser.Parse(Array.Empty<string>());

            Assert.True(result.IsSuccess);
            Assert.Equal("DEFAULT_COMMAND", result.Command.ToString());
        }

        /// <summary>
        /// start-(EoA)->no default command ? error
        /// </summary>
        [Fact]
        public void should_be_error_for_non_argument_if_default_command_is_not_set()
        {
            var parser = new ArgsParserBuilder().Build();

            ArgsParsingResult result = parser.Parse(Array.Empty<string>());

            result.AssertError(
                ArgsParsingErrorCode.DoesNotMatchAnyCommand,
                "Unexpected end of arguments.");
        }
    }
}