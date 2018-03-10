using System;
using Xunit;

namespace Axe.Cli.Parser.Test
{
    public class ParsingResultExtensionsFacts
    {
        [Fact]
        public void should_throw_if_result_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => ((CliArgsParsingResult) null).GetOptionValues<object>("-i"));
            Assert.Throws<ArgumentNullException>(() => ((CliArgsParsingResult) null).GetOptionValue<object>("-i"));
        }
    }
}