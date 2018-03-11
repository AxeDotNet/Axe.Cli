using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Axe.Cli.Parser
{
    abstract class CommandDefinitionBase : ICommandDefinition
    {
        readonly List<ICliOptionDefinition> options = new List<ICliOptionDefinition>();
        readonly List<IFreeValueDefinition> freeValues = new List<IFreeValueDefinition>();
        public bool AllowFreeValue { get; set; }

        public abstract string Symbol { get; }

        public abstract string Description { get; }

        public abstract bool IsConflict(ICommandDefinition commandDefinition);

        public abstract bool IsMatch(string argument);

        public IEnumerable<ICliOptionDefinition> GetRegisteredOptionsMetadata()
        {
            return GetRegisteredOptions();
        }

        public IEnumerable<IFreeValueDefinitionMetadata> GetRegisteredFreeValuesMetadata()
        {
            return GetRegisteredFreeValues();
        }

        public IEnumerable<ICliOptionDefinition> GetRegisteredOptions()
        {
            return options;
        }

        public IEnumerable<IFreeValueDefinition> GetRegisteredFreeValues()
        {
            return freeValues;
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

        public void RegisterFreeValue(IFreeValueDefinition freeValue)
        {
            Debug.Assert(freeValue != null);

            IFreeValueDefinition conflictFreeValueDefinition =
                freeValues.FirstOrDefault(f => f.IsConflict(freeValue));
            if (conflictFreeValueDefinition != null)
            {
                throw new ArgumentException(
                    $"The free value definition '{freeValue}' conflicts with '{conflictFreeValueDefinition}'");
            }
            
            freeValues.Add(freeValue);
        }
    }
}