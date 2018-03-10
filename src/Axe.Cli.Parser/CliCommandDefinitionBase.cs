using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Axe.Cli.Parser
{
    abstract class CliCommandDefinitionBase : ICliCommandDefinition
    {
        readonly List<ICliOptionDefinition> options = new List<ICliOptionDefinition>();
        readonly List<ICliFreeValueDefinition> freeValues = new List<ICliFreeValueDefinition>();
        public bool AllowFreeValue { get; set; }
        public abstract string Symbol { get; }
        public abstract string Description { get; }
        public abstract bool IsConflict(ICliCommandDefinition commandDefinition);
        public abstract bool IsMatch(string argument);
        
        public IEnumerable<ICliOptionDefinition> GetRegisteredOptions()
        {
            return options;
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
        
        public void RegisterFreeValue(ICliFreeValueDefinition freeValue)
        {
            Debug.Assert(freeValue != null);

            ICliFreeValueDefinition conflictFreeValueDefinition =
                freeValues.FirstOrDefault(f => f.IsConflict(freeValue));
            if (conflictFreeValueDefinition != null)
            {
                throw new ArgumentException(
                    $"The free value definition '{freeValue}' conflicts with '{conflictFreeValueDefinition}'");
            }
            
            freeValues.Add(freeValue);
        }

        public IEnumerable<ICliFreeValueDefinition> GetRegisteredFreeValues()
        {
            return freeValues;
        }
    }
}