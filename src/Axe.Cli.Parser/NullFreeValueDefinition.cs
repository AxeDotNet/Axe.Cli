using System;

namespace Axe.Cli.Parser
{
    class NullFreeValueDefinition : IFreeValueDefinition
    {
        NullFreeValueDefinition() {}
        
        public static IFreeValueDefinition Instance { get; } = new NullFreeValueDefinition();
        
        public Guid Id { get; } = Guid.Empty;
        public string Name { get; } = string.Empty;
        public string Description { get; } = string.Empty;
        public ValueTransformer Transformer { get; } = ArgsTransformers.Default;
        public bool IsConflict(IFreeValueDefinition freeValueDefinition)
        {
            // always return true to avoid registration.
            return true;
        }

        public bool IsMatch(string name)
        {
            return false;
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
    }
}