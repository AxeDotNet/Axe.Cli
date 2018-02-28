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
            if (command == null) { throw new ArgumentNullException(nameof(command)); }
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

        public void SetDefaultCommand(CliDefaultCommandDefinition defaultCommandDefinition, bool overwriteIfExist = false)
        {
            if (defaultCommand != null && !overwriteIfExist)
            {
                throw new InvalidOperationException("The default command has been set.");
            }

            defaultCommand = defaultCommandDefinition ?? throw new ArgumentNullException(nameof(defaultCommandDefinition));
        }
    }
}