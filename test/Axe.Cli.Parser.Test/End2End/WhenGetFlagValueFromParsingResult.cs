using System;
using Axe.Cli.Parser.Test.Helpers;
using Xunit;

namespace Axe.Cli.Parser.Test.End2End
{
    public class WhenGetFlagValueFromParsingResult
    {
        [Fact]
        public void should_throw_if_flag_is_null_when_getting_flag_value()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .ConfigFreeValue(true)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new string[0]);

            result.AssertSuccess();

            Assert.Throws<ArgumentNullException>(() => result.GetFlagValue(null));
        }

        [Fact]
        public void should_throw_if_flag_is_not_defined_when_getting_flag_value()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .ConfigFreeValue(true)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new string[0]);

            result.AssertSuccess();

            Assert.Throws<ArgumentException>(() => result.GetFlagValue("--not-defined"));
        }

        [Fact]
        public void should_throw_when_getting_flag_in_a_failure_result()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption("flag", 'f', string.Empty)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new [] {"free_value"});

            Assert.False(result.IsSuccess);
            Assert.Throws<InvalidOperationException>(() => result.GetFlagValue("--flag"));
        }

        [Fact]
        public void should_get_dup_flags_for_duplicated_flags_with_default_command()
        {
            // start-(dup-flag-options)->error

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

        [Theory]
        [InlineData("--flag")]
        [InlineData("-f")]
        public void should_get_flag_option_with_default_command(string argumentExpression)
        {
            // start-(flag-option)-(EoA)->ok

            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption("flag", 'f', string.Empty)
                .EndCommand()
                .Build();
             
            string[] args = { argumentExpression };
            ArgsParsingResult result = parser.Parse(args);

            result.AssertSuccess();
            Assert.True(result.GetFlagValue(argumentExpression));
        }
        
        [Fact]
        public void should_get_flag_using_full_form_but_parsed_as_abbr_form_with_default_command()
        {
            // start-(abbr-flag-options)-(EoA)->ok

            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption("recursive", 'r', string.Empty)
                .AddFlagOption("force", 'f', string.Empty)
                .EndCommand()
                .Build();

            string[] args = {"-rf"};
            ArgsParsingResult result = parser.Parse(args);

            result.AssertSuccess();
            Assert.True(result.GetFlagValue("--recursive"));
            Assert.True(result.GetFlagValue("--force"));
        }
        
        [Fact]
        public void should_get_flag_for_non_specified_flags_with_default_command()
        {
            // start-(flag-option)->(EoA)->other flag option should be false.

            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption("flag", 'f', string.Empty)
                .AddFlagOption("other-flag", 'o', string.Empty)
                .EndCommand()
                .Build();
             
            string[] args = { "-f" };
            ArgsParsingResult result = parser.Parse(args);

            result.AssertSuccess();
            Assert.True(result.GetFlagValue("-f"));
            Assert.False(result.GetFlagValue("-o"));
        }

        [Fact]
        public void should_get_multiple_flag_for_default_command()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption("flag-a", 'a', string.Empty)
                .AddFlagOption("flag-b", 'b', string.Empty)
                .AddFlagOption("flag-c", 'c', string.Empty)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new [] {"-a", "-b"});

            result.AssertSuccess();
            Assert.True(result.GetFlagValue("--flag-a"));
            Assert.True(result.GetFlagValue("--flag-b"));
            Assert.False(result.GetFlagValue("--flag-c"));
        }

        [Fact]
        public void should_get_flag_for_specified_command()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .AddFlagOption("flag", 'f', string.Empty)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new [] {"command", "-f"});

            result.AssertSuccess();
            Assert.True(result.GetFlagValue("--flag"));
        }

        [Fact]
        public void should_get_multiple_flag_for_specified_command()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .AddFlagOption("flag-a", 'a', string.Empty)
                .AddFlagOption("flag-b", 'b', string.Empty)
                .AddFlagOption("flag-c", 'c', string.Empty)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new [] {"command", "-a", "-b"});

            result.AssertSuccess();
            Assert.True(result.GetFlagValue("--flag-a"));
            Assert.True(result.GetFlagValue("--flag-b"));
            Assert.False(result.GetFlagValue("--flag-c"));
        }

        [Fact]
        public void should_recognize_some_odd_flag_with_number()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .AddFlagOption("1", 'o', string.Empty)
                .EndCommand()
                .Build();
            
            ArgsParsingResult result = parser.Parse(new [] {"command", "--1"});
            
            result.AssertSuccess();
            Assert.True(result.GetFlagValue("--1"));
            Assert.True(result.GetFlagValue("-o"));
        }
    }
}