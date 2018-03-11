using System;
using Xunit;

namespace Axe.Cli.Parser.Test.End2End
{
    public class WhenDefineCommand
    {
        [Fact]
        public void should_throw_if_command_name_is_null()
        {
            Assert.Throws<ArgumentNullException>(
                () => new ArgsParserBuilder().BeginCommand(null, string.Empty));
        }

        [Theory]
        [InlineData("")]
        [InlineData("-o")]
        [InlineData("with space")]
        [InlineData("multi\nline")]
        [InlineData("with_other_symbol_@")]
        [InlineData("--this-is-an-option")]
        public void should_throw_if_command_name_is_of_invalid_format(string incorrectCommandSymbol)
        {
            Assert.Throws<ArgumentException>(
                () => new ArgsParserBuilder().BeginCommand(incorrectCommandSymbol, string.Empty));
        }
    }
}