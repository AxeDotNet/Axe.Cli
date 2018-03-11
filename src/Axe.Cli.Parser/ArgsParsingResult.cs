using System;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Cli.Parser
{
    /// <summary>
    /// Represent the parsing result of command line arguments.
    /// </summary>
    public class ArgsParsingResult
    {
        readonly IList<KeyValuePair<ICliOptionDefinition, bool>> optionFlags;
        readonly IList<KeyValuePair<ICliOptionDefinition, OptionValue>> optionValues;
        readonly IList<KeyValuePair<IFreeValueDefinition, FreeValue>> freeValues;

        /// <summary>
        /// Get the matched command definitions. It can be either a named command or a default
        /// command (if supported). This value is <c>null</c> if <see cref="IsSuccess"/> is
        /// <c>false</c>.
        /// </summary>
        public ICommandDefinitionMetadata Command { get; }

        /// <summary>
        /// Get if the parsing result represents a success. You should always check this value
        /// before processing the parsing result.
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Get detailed parsing error if <see cref="IsSuccess"/> is <c>false</c>. This value is
        /// <c>null</c> if <see cref="IsSuccess"/> is <c>true</c>.
        /// </summary>
        public ArgsParsingError Error { get; }

        internal ArgsParsingResult(ArgsParsingError error)
        {
            Error = error ?? throw new ArgumentNullException(nameof(error));
            IsSuccess = false;
        }

        internal ArgsParsingResult(
            ICommandDefinition command,
            IEnumerable<KeyValuePair<ICliOptionDefinition, IList<string>>> optionValues,
            IEnumerable<KeyValuePair<ICliOptionDefinition, bool>> optionFlags,
            IList<KeyValuePair<IFreeValueDefinition, string>> freeValues)
        {
            Command = command ?? throw new ArgumentNullException(nameof(command));
            this.optionValues = TransformService.TransformOptionValues(optionValues);
            this.optionFlags = optionFlags?.ToArray() ?? Array.Empty<KeyValuePair<ICliOptionDefinition, bool>>();
            this.freeValues = TransformService.TransformFreeValues(freeValues);
            IsSuccess = true;
        }

        OptionValue GetOptionValueObject(string option)
        {
            KeyValuePair<ICliOptionDefinition, OptionValue> matchedKeyValue = 
                optionValues.FirstOrDefault(o => o.Key.IsMatch(option));
            if (matchedKeyValue.Key == null)
            {
                throw new ArgumentException($"The option you specified is not defined: '{option}'");
            }

            return matchedKeyValue.Value;
        }

        FreeValue GetFreeValueObject(string name)
        {
            KeyValuePair<IFreeValueDefinition, FreeValue> matchedFreeValue =
                freeValues.FirstOrDefault(f => f.Key.IsMatch(name));
            if (matchedFreeValue.Key == null) 
            {
                throw new ArgumentException($"The free value name you specified is not defined: '{name}");
            }

            return matchedFreeValue.Value;
        }

        void EnsureSuccess()
        {
            if (!IsSuccess)
            {
                throw new InvalidOperationException(
                    "This operation is not supported because current parsing result is a failure.");
            }
        }

        /// <summary>
        /// Get the value of a flag.
        /// </summary>
        /// <param name="flag">The full form or the abbreviation form of the flag.</param>
        /// <returns>
        /// If the flag appears in the command line argument, then this value is <c>true</c>,
        /// otherwise <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="flag"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The <paramref name="flag"/> specified is not defined.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The <see cref="IsSuccess"/> is <c>false</c>.
        /// </exception>
        public bool GetFlagValue(string flag)
        {
            if (flag == null) { throw new ArgumentNullException(nameof(flag)); }

            EnsureSuccess();
            KeyValuePair<ICliOptionDefinition, bool> matchedFlag = optionFlags.FirstOrDefault(o => o.Key.IsMatch(flag));
            if (matchedFlag.Key == null) { throw new ArgumentException($"The flag you specified is not defined: '{flag}'");}
            return matchedFlag.Value;
        }

        /// <summary>
        /// <para>
        /// Get a list of raw string values of an key-value definition. Note that it may
        /// contains multiple values if the option appears more than once in the command
        /// line arguments. Such as:
        /// </para>
        /// <code><![CDATA[--key value1 --key value2]]>
        /// </code>
        /// </summary>
        /// <param name="option">The full form or the abbreviation form of an option key.</param>
        /// <returns>
        /// A list of raw string values to that option key. If the option is not mandatory, and if
        /// is not specified in the command line arguments. An empty array will be returned.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="option"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The <paramref name="option"/> specified is not defined.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The <see cref="IsSuccess"/> is <c>false</c>.
        /// </exception>
        public IList<string> GetOptionRawValue(string option)
        {
            if (option == null) { throw new ArgumentNullException(nameof(option)); }

            EnsureSuccess();
            return GetOptionValueObject(option).Raw;
        }

        /// <summary>
        /// Get a list of translated values of an key-value definition. Note that it may
        /// contains multiple values if the option appears more than once in the command
        /// line arguments, or some argument can be translated to multiple values.
        /// </summary>
        /// <param name="option">The full form or the abbreviation form of an option key.</param>
        /// <returns>
        /// A list of raw translated values to that option key. If the option is not mandatory, and if
        /// is not specified in the command line arguments. An empty array will be returned.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="option"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The <paramref name="option"/> specified is not defined.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The <see cref="IsSuccess"/> is <c>false</c>.
        /// </exception>
        public IList<object> GetOptionValue(string option)
        {
            if (option == null) { throw new ArgumentNullException(nameof(option)); }

            EnsureSuccess();
            return GetOptionValueObject(option).Transformed;
        }

        /// <summary>
        /// Get a list of translated values for a named free-value. Note that it may contains
        /// multiple values if the argument can be translated to multiple values.
        /// </summary>
        /// <param name="name">The name of the free value.</param>
        /// <returns>
        /// The translated values to the name. If the free value is optional and it is not
        /// specified in the command line arguments. An empty array will be returned.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="name"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The <paramref name="name"/> specified is not defined.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The <see cref="IsSuccess"/> is <c>false</c>.
        /// </exception>
        public IList<object> GetFreeValue(string name)
        {
            if (name == null) { throw new ArgumentNullException(nameof(name)); }

            EnsureSuccess();
            return GetFreeValueObject(name).Transformed;
        }

        /// <summary>
        /// Get a raw value for a named free-value.
        /// </summary>
        /// <param name="name">The name of the free value.</param>
        /// <returns>
        /// The raw value to the name. If the free value is optional and it is not specified in
        /// the command line arguments. An empty string will be returned.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="name"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The <paramref name="name"/> specified is not defined.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The <see cref="IsSuccess"/> is <c>false</c>.
        /// </exception>
        public string GetFreeRawValue(string name)
        {
            if (name == null) { throw new ArgumentNullException(nameof(name)); }

            EnsureSuccess();
            return GetFreeValueObject(name).Raw;
        }

        /// <summary>
        /// Get a list of undefined but captured free values.
        /// </summary>
        /// <returns>
        /// The undefined but captured free values. If there is none, returns empty array.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// The <see cref="IsSuccess"/> is <c>false</c>.
        /// </exception>
        public IList<string> GetUndefinedFreeValues()
        {
            EnsureSuccess();

            return freeValues
                .Where(f => f.Key is NullFreeValueDefinition)
                .Select(f => f.Value.Raw)
                .ToArray();
        }
    }
}