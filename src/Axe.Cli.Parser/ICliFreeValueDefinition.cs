using System;

namespace Axe.Cli.Parser
{
    public interface ICliFreeValueDefinition : IEquatable<ICliFreeValueDefinition>
    {
        Guid Id { get; }
        
        string Name { get; }
        string Description { get; }
        IValueTransformer Transformer { get; }

        bool IsConflict(ICliFreeValueDefinition freeValueDefinition);
        bool IsMatch(string name);
        string ToString();
    }
}