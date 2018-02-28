using System;
using System.Linq;

namespace Axe.Cli.Parser
{
    public class CliOptionValueResult
    {
        readonly object[] values;

        public CliOptionValueResult(ICliOptionDefinition definition, object[] values)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            this.values = values ?? Array.Empty<object>();
            HasValue = true;
        }

        public CliOptionValueResult()
        {
            values = null;
            Definition = null;
            HasValue = false;
        }

        public ICliOptionDefinition Definition { get; }
        public bool HasValue { get; }


        public object[] GetValues()
        {
            EnsureHasValue();
            return values;
        }

        public object GetValue()
        {
            EnsureHasValue();
            return values.FirstOrDefault();
        }

        public T GetValue<T>()
        {
            EnsureHasValue();
            object firstValue = values.FirstOrDefault();
            
            if (firstValue == null && typeof(T).IsValueType)
            {
                throw new InvalidOperationException($"The result is null while '{typeof(T).Name}' is a value type.");
            }

            return (T) firstValue;
        }

        void EnsureHasValue()
        {
            if (!HasValue)
            {
                throw new InvalidOperationException("The result does not contain any value.");
            }
        }
    }
}