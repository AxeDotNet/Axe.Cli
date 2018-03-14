using System;
using System.Text.RegularExpressions;
using Axe.Cli.Parser.Extensions;
using Axe.Cli.Parser.Transformers;

namespace Axe.Cli.Parser
{
    class FreeValueDefinition : IFreeValueDefinition
    {
        static readonly Regex NamePattern = new Regex(
            "^[A-Z_][A-Z0-9_\\-]{0,}$", 
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline |
            RegexOptions.IgnoreCase);
        
        public FreeValueDefinition(string name, string description, bool isRequired = false, ValueTransformer transformer = null)
        {
            ValidateName(name);
            
            Name = name;
            IsRequired = isRequired;
            Description = description.MakeSingleLine();
            Transformer = transformer ?? DefaultTransformer.Instance;
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
        public bool IsRequired { get; }
        public ValueTransformer Transformer { get; }
        public bool IsConflict(IFreeValueDefinition freeValueDefinition)
        {
            if (freeValueDefinition == null) { throw new ArgumentNullException(nameof(freeValueDefinition)); }
            return Name.Equals(freeValueDefinition.Name, StringComparison.OrdinalIgnoreCase);
        }

        public bool IsMatch(string name)
        {
            return Name.Equals(name, StringComparison.OrdinalIgnoreCase);
        }

        public bool Equals(IFreeValueDefinition other)
        {
            if (other == null) { return false; }
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) { return false;}
            if (GetType() != obj.GetType()) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            return Equals((IFreeValueDefinition) obj);
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