using System;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Cli.Parser
{
    abstract class CliCommandDefinitionBase : ICliOptionDefinitionContainer, ICliCommandSymbolDefinition
    {
        readonly List<CliOptionDefinition> options = new List<CliOptionDefinition>();

        public IReadOnlyList<CliOptionDefinition> GetRegisteredOptions()
        {
            return options.AsReadOnly();
        }

        public void RegisterOption(CliOptionDefinition option)
        {
            if (option == null) { throw new ArgumentNullException(nameof(option)); }
            CliOptionDefinition conflictOptionDefinition = options.FirstOrDefault(o => o.IsConflict(option));
            if (conflictOptionDefinition != null)
            {
                throw new ArgumentException(
                    $"The option definition '{option}' conflicts with definition '{conflictOptionDefinition}'");
            }

            options.Add(option);
        }

        public abstract string Symbol { get; }
        public abstract string Description { get; }
        public abstract bool IsConflict(ICliCommandSymbolDefinition commandDefinition);
        public abstract bool IsMatch(string argument);
    }
}