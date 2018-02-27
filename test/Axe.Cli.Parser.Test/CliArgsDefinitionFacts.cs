using System;
using System.Linq;
using Xunit;

namespace Axe.Cli.Parser.Test
{
    public class CliArgsDefinitionFacts
    {
        [Fact]
        public void should_register_command()
        {
            var argsDefinition = new CliArgsDefinition();
            var command = new CliCommandDefinition("command", "awesome description");
            argsDefinition.RegisterCommand(command);

            Assert.True(
                argsDefinition.GetRegisteredCommands().Any(c => ReferenceEquals(c, command)));
        }

        [Fact]
        public void should_throw_if_command_conflicts()
        {
            var argsDefinition = new CliArgsDefinition();
            argsDefinition.RegisterCommand(
                new CliCommandDefinition("command", "awesome description"));

            Assert.Throws<ArgumentException>(() => 
                argsDefinition.RegisterCommand(new CliCommandDefinition("command", string.Empty)));
        }

        [Fact]
        public void should_set_default_command()
        {
            var argsDefinition = new CliArgsDefinition();
            var defaultCommand = new CliDefaultCommandDefinition();

            argsDefinition.SetDefaultCommand(defaultCommand);
            Assert.Same(defaultCommand, argsDefinition.DefaultCommand);
        }

        [Fact]
        public void should_throw_if_default_command_has_been_set()
        {
            var argsDefinition = new CliArgsDefinition();
            var defaultCommand = new CliDefaultCommandDefinition();

            argsDefinition.SetDefaultCommand(defaultCommand);

            Assert.Throws<InvalidOperationException>(() => argsDefinition.SetDefaultCommand(defaultCommand));
        }

        [Fact]
        public void should_set_default_command_overwritten()
        {
            var argsDefinition = new CliArgsDefinition();
            var defaultCommand = new CliDefaultCommandDefinition();
            var anotherDefaultCommand = new CliDefaultCommandDefinition();

            argsDefinition.SetDefaultCommand(defaultCommand);
            argsDefinition.SetDefaultCommand(anotherDefaultCommand, true);

            Assert.Same(anotherDefaultCommand, argsDefinition.DefaultCommand);
        }
    }
}