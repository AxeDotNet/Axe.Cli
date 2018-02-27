using Xunit;

namespace Axe.Cli.Parser.Test
{
    public class CliDefaultCommandDefinitionFacts
    {
        [Fact]
        public void should_conflict_with_any_other_command_type()
        {
            Assert.True(new CliDefaultCommandDefinition()
                .IsConflict(new CliCommandDefinition("command", string.Empty)));
        }
    }
}