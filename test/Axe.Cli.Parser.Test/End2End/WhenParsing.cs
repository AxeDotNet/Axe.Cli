using System;
using Xunit;

namespace Axe.Cli.Parser.Test.End2End
{
    public class WhenParsing
    {
        [Fact]
        public void should_be_error_when_parsing_null()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginCommand("command", string.Empty).EndCommand()
                .Build();

            Assert.Throws<ArgumentNullException>(() => parser.Parse(null));
        }

        [Fact]
        public void should_be_error_when_parsing_null_command()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginCommand("command", string.Empty).EndCommand()
                .Build();

            Assert.Throws<ArgumentException>(() => parser.Parse(new string[] {null}));
        }

        [Fact]
        public void should_get_parsing_result()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .EndCommand()
                .Build();

            Assert.NotNull(parser.Parse(new[] {"command"}));
            Assert.NotNull(parser.Parse(new string[0]));
            Assert.NotNull(parser.Parse(new[] {"-t"}));
        }
    }
}