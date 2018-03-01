using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Axe.Cli.Parser.Test
{
    public class CliArgsParserStartStateFacts
    {
        /// <summary>
        /// start-(command)->ok
        /// </summary>
        public void should_get_command_from_parsing_result()
        {
            const string commandName = "command";
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginCommand(commandName, string.Empty)
                .EndCommand()
                .Build();

            string[] args = { commandName };

            CliArgsParsingResult result = parser.Parse(args);

            Assert.True(result.IsSuccess);
            Assert.Same(commandName, result.Command.Symbol);
        }

        /// <summary>
        /// start-(unresolved command)->no default command ? error
        /// </summary>
        public void should_be_error_if_command_does_not_match_without_default_command()
        {
            const string commandName = "command";
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginCommand(commandName, string.Empty)
                .EndCommand()
                .Build();

            string[] args = {"not_matched_command"};

            CliArgsParsingResult result = parser.Parse(args);

            AssertError(
                result,
                CliArgsParsingErrorCode.DoesNotMatchAnyCommand,
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
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginDefaultCommand()
                .AddOptionWithValue("option", 'o', string.Empty)
                .EndCommand()
                .Build();

            string[] args = { argumentExpression };
            CliArgsParsingResult result = parser.Parse(args);

            AssertError(
                result,
                CliArgsParsingErrorCode.CannotFindValueForOption,
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
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption("flag", 'f', string.Empty)
                .EndCommand()
                .Build();
             
            string[] args = { argumentExpression };
            CliArgsParsingResult result = parser.Parse(args);

            Assert.True(result.IsSuccess);
            Assert.True(result.GetFlagValue(argumentExpression));
        }

        /// <summary>
        /// start-(abbr-flag-options)-(EoA)->ok
        /// </summary>
        [Fact]
        public void should_be_ok_for_multiple_abbr_form_flags_with_default_command()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption("recursive", 'r', string.Empty)
                .AddFlagOption("force", 'f', string.Empty)
                .EndCommand()
                .Build();

            string[] args = {"-rf"};
            CliArgsParsingResult result = parser.Parse(args);

            Assert.True(result.IsSuccess);
            Assert.True(result.GetFlagValue("--recursive"));
            Assert.True(result.GetFlagValue("--force"));
        }
        
        /// <summary>
        /// start-(dup-flag-options)->error
        /// </summary>
        [Fact]
        public void should_be_error_for_duplicated_flags_with_default_command()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption("flag", 'f', string.Empty)
                .EndCommand()
                .Build();
             
            string[] args = { "-ff" };
            CliArgsParsingResult result = parser.Parse(args);

            Assert.False(result.IsSuccess);
            AssertError(result, CliArgsParsingErrorCode.DuplicateFlagsInArgs, "-ff");
        }
        
        /// <summary>
        /// start-(flag-option)->(EoA)->other flag option should be false.
        /// </summary>
        [Fact]
        public void should_be_ok_for_non_specified_flags_with_default_command()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption("flag", 'f', string.Empty)
                .AddFlagOption("other-flag", 'o', string.Empty)
                .EndCommand()
                .Build();
             
            string[] args = { "-f" };
            CliArgsParsingResult result = parser.Parse(args);

            Assert.True(result.IsSuccess);
            Assert.True(result.GetFlagValue("-f"));
            Assert.False(result.GetFlagValue("-o"));
        }

        /// <summary>
        /// start-(EoA)->ok
        /// </summary>
        [Fact]
        public void should_be_ok_for_non_argument_if_default_command_is_set()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginDefaultCommand().EndCommand().Build();

            CliArgsParsingResult result = parser.Parse(Array.Empty<string>());

            Assert.True(result.IsSuccess);
            Assert.Equal("DEFAULT_COMMAND", result.Command.ToString());
        }

        /// <summary>
        /// start-(EoA)->no default command ? error
        /// </summary>
        [Fact]
        public void should_be_error_for_non_argument_if_default_command_is_not_set()
        {
            var parser = new CliArgsParserBuilder().Build();

            CliArgsParsingResult result = parser.Parse(Array.Empty<string>());

            AssertError(
                result,
                CliArgsParsingErrorCode.DoesNotMatchAnyCommand,
                "Unexpected end of arguments.");
        }

        [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
        static void AssertError(
            CliArgsParsingResult result,
            CliArgsParsingErrorCode code,
            string trigger)
        {
            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
            Assert.Equal(code, result.Error.Code);
            Assert.Equal(trigger, result.Error.Trigger);
        }
    }
}