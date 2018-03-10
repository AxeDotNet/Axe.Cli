using System;
using Xunit;

namespace Axe.Cli.Parser.Test
{
    public class WeirdParsingFacts
    {
        [Fact]
        public void should_be_error_when_parsing_null()
        {
            CliArgsParser parser = new CliArgsParserBuilder()
                .BeginCommand("command", string.Empty).EndCommand()
                .Build();

            Assert.Throws<ArgumentNullException>(() => parser.Parse(null));
        }

        [Fact]
        public void should_be_error_when_parsing_null_command()
        {

        }
    }
}