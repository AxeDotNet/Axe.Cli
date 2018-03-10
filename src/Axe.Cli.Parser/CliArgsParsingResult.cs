using System;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Cli.Parser
{
    public class CliArgsParsingResult
    {
        readonly IList<KeyValuePair<ICliOptionDefinition, bool>> optionFlags;
        readonly IList<KeyValuePair<ICliOptionDefinition, OptionValue>> optionValues;
        readonly IList<KeyValuePair<ICliFreeValueDefinition, string>> freeValues;

        public ICliCommandDefinition Command { get; }
        public bool IsSuccess { get; }
        public CliArgsParsingError Error { get; }

        public CliArgsParsingResult(CliArgsParsingError error)
        {
            Error = error ?? throw new ArgumentNullException(nameof(error));
            IsSuccess = false;
        }

        public CliArgsParsingResult(
            ICliCommandDefinition command,
            IEnumerable<KeyValuePair<ICliOptionDefinition, IList<string>>> optionValues,
            IEnumerable<KeyValuePair<ICliOptionDefinition, bool>> optionFlags,
            IEnumerable<KeyValuePair<ICliFreeValueDefinition, string>> freeValues)
        {
            Command = command ?? throw new ArgumentNullException(nameof(command));
            this.optionValues = TransformOptionValues(optionValues);
            this.optionFlags = optionFlags?.ToArray() ?? Array.Empty<KeyValuePair<ICliOptionDefinition, bool>>();
            this.freeValues = freeValues?.ToArray() ?? Array.Empty<KeyValuePair<ICliFreeValueDefinition, string>>();
            IsSuccess = true;
        }

        static IList<KeyValuePair<ICliOptionDefinition, OptionValue>> TransformOptionValues(
            IEnumerable<KeyValuePair<ICliOptionDefinition, IList<string>>> rawOptionValues)
        {
            if (rawOptionValues == null) { return Array.Empty<KeyValuePair<ICliOptionDefinition, OptionValue>>(); }

            return rawOptionValues
                .Select(
                    ov =>
                    {
                        ICliOptionDefinition optionDefinition = ov.Key;
                        IValueTransformer transformer = optionDefinition.Transformer;

                        try
                        {
                            return new KeyValuePair<ICliOptionDefinition, OptionValue>(
                                optionDefinition,
                                new OptionValue(ov.Value, transformer.Transform(ov.Value)));
                        }
                        catch (CliArgParsingException)
                        {
                            throw;
                        }
                        catch
                        {
                            throw new CliArgParsingException(
                                CliArgsParsingErrorCode.TransformOptionValueFailed,
                                $"Option: {ov.Key.ToString()}; Values: {string.Join(" ", ov.Value)}.");
                        }
                    })
                .ToArray();
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

        public bool GetFlagValues(string flag)
        {
            KeyValuePair<ICliOptionDefinition, bool> matchedFlag = optionFlags.FirstOrDefault(o => o.Key.IsMatch(flag));
            if (matchedFlag.Key == null) { throw new ArgumentException($"The flag you specified is not defined: '{flag}'");}
            return matchedFlag.Value;
        }

        public IList<string> GetOptionRawValues(string option)
        {
            return GetOptionValueObject(option ?? throw new ArgumentNullException(nameof(option))).RawValues;
        }

        public IList<object> GetOptionValues(string option)
        {
            return GetOptionValueObject(option ?? throw new ArgumentNullException(nameof(option))).TransformedValues;
        }

        public IList<string> GetFreeValue(string name)
        {
            KeyValuePair<ICliFreeValueDefinition, string> matchedFreeValue =
                freeValues.FirstOrDefault(f => f.Key.IsMatch(name));
            if (matchedFreeValue.Key == null) 
            {
                throw new ArgumentException($"The free value name you specified is not defined: '{name}");
            }

            return new[] {matchedFreeValue.Value};
        }

        public IList<string> GetUndefinedFreeValues()
        {
            return freeValues.Where(f => f.Key is CliNullFreeValueDefinition).Select(f => f.Value).ToArray();
        }
    }
}