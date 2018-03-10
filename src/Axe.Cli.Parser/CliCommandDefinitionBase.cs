using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Axe.Cli.Parser
{
    abstract class CliCommandDefinitionBase : ICliCommandDefinition
    {
        readonly List<ICliOptionDefinition> options = new List<ICliOptionDefinition>();
        public bool AllowFreeValue { get; set; }
        public abstract string Symbol { get; }
        public abstract string Description { get; }
        public abstract bool IsConflict(ICliCommandDefinition commandDefinition);
        public abstract bool IsMatch(string argument);
        
        public IReadOnlyList<ICliOptionDefinition> GetRegisteredOptions()
        {
            return options.AsReadOnly();
        }

        public void RegisterOption(ICliOptionDefinition option)
        {
            Debug.Assert(option != null);

            ICliOptionDefinition conflictOptionDefinition = options.FirstOrDefault(o => o.IsConflict(option));
            if (conflictOptionDefinition != null)
            {
                throw new ArgumentException(
                    $"The option definition '{option}' conflicts with definition '{conflictOptionDefinition}'");
            }

            options.Add(option);
        }
    }
}