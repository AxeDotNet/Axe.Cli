using System;
using Xunit;

namespace Axe.Cli.Parser.Test.Unit
{
    public class ParsingResultExtensionsFacts
    {
        [Fact]
        public void should_throw_if_result_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => ((ArgsParsingResult) null).GetOptionValue<object>("-i"));
            Assert.Throws<ArgumentNullException>(() => ((ArgsParsingResult) null).GetFirstOptionValue<object>("-i"));
        }

        [Fact]
        public void should_throw_if_option_is_null()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new [] {"command"});

            Assert.Throws<ArgumentNullException>(() => result.GetOptionValue<object>(null));
        }
    }
}