using System;
using System.Text.RegularExpressions;

namespace Axe.Cli.Parser
{
    class CliFreeValueDefinition : ICliFreeValueDefinition
    {
        static readonly Regex NamePattern = new Regex(
            "^[A-Z_][A-Z0-9_\\-]{0,}$", 
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline |
            RegexOptions.IgnoreCase);
        
        public CliFreeValueDefinition(string name, string description, ValueTransformer transformer = null)
        {
            ValidateName(name);
            
            Name = name;
            Description = description ?? string.Empty;
            Transformer = transformer ?? CliArgsTransformers.Default;
        }

        static void ValidateName(string name)
        {
            if (name == null) { throw new ArgumentNullException(nameof(name)); }
            if (NamePattern.IsMatch(name)) return;
            string message = $"The name is in an invalid format: {name}";
            throw new ArgumentException(message);
        }

        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; }
        public string Description { get; }
        public ValueTransformer Transformer { get; }
        public bool IsConflict(ICliFreeValueDefinition freeValueDefinition)
        {
            return Name.Equals(freeValueDefinition.Name, StringComparison.OrdinalIgnoreCase);
        }

        public bool IsMatch(string name)
        {
            return Name.Equals(name, StringComparison.OrdinalIgnoreCase);
        }

        public bool Equals(ICliFreeValueDefinition other)
        {
            if (other == null) { return false; }
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) { return false;}
            if (GetType() != obj.GetType()) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            return Equals((ICliFreeValueDefinition) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}