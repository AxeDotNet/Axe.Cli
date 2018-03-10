using System;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Cli.Parser
{
    public static class ParsingResultExtensions
    {
        public static IList<T> GetOptionValues<T>(this CliArgsParsingResult result, string option)
        {
            if (result == null) {throw new ArgumentNullException(nameof(result));}

            IList<object> values = result.GetOptionValue(option);
            if (typeof(IList<T>) == typeof(IList<object>)) { return (IList<T>)values; }

            return values.Cast<T>().ToArray();
        }

        public static T GetOptionValue<T>(this CliArgsParsingResult result, string option, Func<T> defaultFunc = null)
        {
            if (result == null) {throw new ArgumentNullException(nameof(result));}
            Func<T> createDefaultValue = defaultFunc ?? (() => default(T));

            IList<object> values = result.GetOptionValue(option);
            if (values.Count == 0) { return createDefaultValue(); }

            return (T) values.First();
        }
    }
}