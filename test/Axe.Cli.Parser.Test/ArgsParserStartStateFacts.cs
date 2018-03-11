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

            result.AssertSuccess();
            Assert.Equal(commandName, result.Command.Symbol);
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

            result.AssertSuccess();
            Assert.Equal("DEFAULT_COMMAND", result.Command.ToString());
        }
    }
}