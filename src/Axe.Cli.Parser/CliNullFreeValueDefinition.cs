using System;

namespace Axe.Cli.Parser
{
    class CliNullFreeValueDefinition : ICliFreeValueDefinition
    {
        CliNullFreeValueDefinition() {}
        
        public static ICliFreeValueDefinition Instance { get; } = new CliNullFreeValueDefinition();
        
        public Guid Id { get; } = Guid.Empty;
        public string Name { get; } = string.Empty;
        public string Description { get; } = string.Empty;
        public ValueTransformer Transformer { get; } = ArgsTransformers.Default;
        public bool IsConflict(ICliFreeValueDefinition freeValueDefinition)
        {
            // always return true to avoid registration.
            return true;
        }

        public bool IsMatch(string name)
        {
            return false;
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
    }
}