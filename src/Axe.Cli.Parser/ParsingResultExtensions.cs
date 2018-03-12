using System;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Cli.Parser
{
    /// <summary>
    /// The extension to ease the usage of parsing result.
    /// </summary>
    public static class ParsingResultExtensions
    {
        
        /// <summary>
        /// Get translated option values and try to cast them to specified type.
        /// </summary>
        /// <param name="result">The argument parsing result.</param>
        /// <param name="option">The full form or the abbreviation form of the option.</param>
        /// <typeparam name="T">The destination type that you want each element to cast to</typeparam>
        /// <returns>The values to the option.</returns>
        /// <exception cref="ArgumentNullException">
        /// <para>The <paramref name="result"/> is <c>null</c>.</para>
        /// <para>-- Or --</para>
        /// <para>The <paramref name="option"/> is <c>null</c>.</para>
        /// </exception>
        public static IList<T> GetOptionValue<T>(this ArgsParsingResult result, string option)
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