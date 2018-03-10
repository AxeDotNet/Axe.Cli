using System;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Cli.Parser
{
    public static class ParsingResultExtensions
    {
        public static IList<T> GetOptionValues<T>(this ArgsParsingResult result, string option)
        {
            if (result == null) {throw new ArgumentNullException(nameof(result));}

            IList<object> values = result.GetOptionValue(option);
            return values.Cast<T>().ToArray();
        }

        public static T GetFirstOptionValue<T>(this ArgsParsingResult result, string option)
        {
            if (result == null) {throw new ArgumentNullException(nameof(result));}
            IList<object> values = result.GetOptionValue(option);
            return (T) values.First();
        }

        public static IList<T> GetFreeValue<T>(this ArgsParsingResult result, string name)
        {
            if (result == null) { throw new ArgumentNullException(nameof(result)); }
            IList<object> values = result.GetFreeValue(name);

            return values.Cast<T>().ToArray();
        }

        public static T GetFirstFreeValue<T>(this ArgsParsingResult result, string name)
        {
            if (result == null) { throw new ArgumentNullException(nameof(result)); }
            IList<object> values = result.GetFreeValue(name);
            return (T) values.First();
        }
    }
}