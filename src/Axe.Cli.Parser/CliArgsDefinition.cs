using System;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Cli.Parser
{
    class CliArgsDefinition
    {
        readonly List<CliCommandDefinition> commands = new List<CliCommandDefinition>();
        CliDefaultCommandDefinition defaultCommand;

        public ICliCommandDefinition DefaultCommand => defaultCommand;

        public void RegisterCommand(CliCommandDefinition command)
        {
            CliCommandDefinition conflict = commands.FirstOrDefault(c => c.IsConflict(command));
            if (conflict != null)
            {
                throw new ArgumentException(
                    $"The command '{command}' conflicts with command '{conflict}'");
            }

            commands.Add(command);
        }

        public IReadOnlyList<CliCommandDefinition> GetRegisteredCommands()
        {
            return commands.AsReadOnly();
        }

        public void SetDefaultCommand(CliDefaultCommandDefinition command)
        {
            if (defaultCommand != null)
            {
                throw new InvalidOperationException("The default command has been set.");
            }

            defaultCommand = command ?? throw new ArgumentNullException(nameof(command));
        }
    }
}