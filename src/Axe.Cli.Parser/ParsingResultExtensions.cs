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
        /// <exception cref="ArgumentException">
        /// The <paramref name="option"/> specified is not defined.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The <paramref name="result"/> is not a successful one.
        /// </exception>
        public static IList<T> GetOptionValue<T>(this ArgsParsingResult result, string option)
        {
            if (result == null) {throw new ArgumentNullException(nameof(result));}

            IList<object> values = result.GetOptionValue(option);
            return values.Cast<T>().ToArray();
        }

        /// <summary>
        /// Get first translated option value in the option list. And try to cast it to specified
        /// type.
        /// </summary>
        /// <typeparam name="T">The destination type that you want the value to cast to.</typeparam>
        /// <param name="result">The argument parsing result.</param>
        /// <param name="option">The full form or the abbreviation form of the option.</param>
        /// <returns>The first value to the option.</returns>
        /// <exception cref="ArgumentNullException">
        /// <para>The <paramref name="result"/> is <c>null</c>.</para>
        /// <para>-- Or --</para>
        /// <para>The <paramref name="option"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The <paramref name="option"/> specified is not defined.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <para>The <paramref name="result"/> is not a successful one.</para>
        /// <para>-- Or --</para>
        /// <para>There is no value for specified <paramref name="option"/>.</para>
        /// </exception>
        public static T GetFirstOptionValue<T>(this ArgsParsingResult result, string option)
        {
            if (result == null) {throw new ArgumentNullException(nameof(result));}
            IList<object> values = result.GetOptionValue(option);
            return (T) values.First();
        }

        /// <summary>
        /// Get translated free values and try to cast them to specified type.
        /// </summary>
        /// <typeparam name="T">The destination type that you want each value to cast to.</typeparam>
        /// <param name="result">The argument parsing result.</param>
        /// <param name="name">The name of the free value.</param>
        /// <returns>The translated free values.</returns>
        /// <exception cref="ArgumentNullException">
        /// <para>The <paramref name="result"/> is <c>null</c>.</para>
        /// <para>-- Or --</para>
        /// <para>The <paramref name="name"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The <paramref name="name"/> specified is not defined.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The <paramref name="result"/> is not a successful one.
        /// </exception>
        public static IList<T> GetFreeValue<T>(this ArgsParsingResult result, string name)
        {
            if (result == null) { throw new ArgumentNullException(nameof(result)); }
            IList<object> values = result.GetFreeValue(name);

            return values.Cast<T>().ToArray();
        }

        /// <summary>
        /// Get first element of translated free values and try to cast it to specified type.
        /// </summary>
        /// <typeparam name="T">The destination type that you want the value to cast to.</typeparam>
        /// <param name="result">The argument parsing result.</param>
        /// <param name="name">The name of the free value.</param>
        /// <returns>The translated free value.</returns>
        /// <exception cref="ArgumentNullException">
        /// <para>The <paramref name="result"/> is <c>null</c>.</para>
        /// <para>-- Or --</para>
        /// <para>The <paramref name="name"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The <paramref name="name"/> specified is not defined.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <para>The <paramref name="result"/> is not a successful one.</para>
        /// <para>-- Or --</para>
        /// <para>There is no value for specified <paramref name="name"/>.</para>
        /// </exception>
        public static T GetFirstFreeValue<T>(this ArgsParsingResult result, string name)
        {
            if (result == null) { throw new ArgumentNullException(nameof(result)); }
            IList<object> values = result.GetFreeValue(name);
            return (T) values.First();
        }
    }
}