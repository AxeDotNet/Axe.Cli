using System;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Cli.Parser
{
    class ArgsDefinition
    {
        readonly List<ICommandDefinition> commands = new List<ICommandDefinition>();
        DefaultCommandDefinition defaultCommand;

        public ICommandDefinition DefaultCommand => defaultCommand;

        public void RegisterCommand(ICommandDefinition command)
        {
            ICommandDefinition conflict = commands.FirstOrDefault(c => c.IsConflict(command));
            if (conflict != null)
            {
                throw new ArgumentException(
                    $"The command '{command}' conflicts with command '{conflict}'");
            }

            commands.Add(command);
        }

        public IEnumerable<ICommandDefinition> GetRegisteredCommands()
        {
            return commands;
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