using System;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Cli.Parser
{
    class ArgsDefinition
    {
        readonly List<CommandDefinition> commands = new List<CommandDefinition>();
        DefaultCommandDefinition defaultCommand;

        public ICommandDefinition DefaultCommand => defaultCommand;

        public void RegisterCommand(CommandDefinition command)
        {
            CommandDefinition conflict = commands.FirstOrDefault(c => c.IsConflict(command));
            if (conflict != null)
            {
                throw new ArgumentException(
                    $"The command '{command}' conflicts with command '{conflict}'");
            }

            commands.Add(command);
        }

        public IReadOnlyList<CommandDefinition> GetRegisteredCommands()
        {
            return commands.AsReadOnly();
        }

        public void SetDefaultCommand(DefaultCommandDefinition command)
        {
            if (defaultCommand != null)
            {
                throw new InvalidOperationException("The default command has been set.");
            }

            defaultCommand = command ?? throw new ArgumentNullException(nameof(command));
        }
    }
}