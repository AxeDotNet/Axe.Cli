using System;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Cli.Parser
{
    public class CliArgsParsingResult
    {
        readonly IList<KeyValuePair<ICliOptionDefinition, bool>> optionFlags;
        readonly IList<KeyValuePair<ICliOptionDefinition, OptionValue>> optionValues;
        readonly IList<KeyValuePair<ICliFreeValueDefinition, FreeValue>> freeValues;

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
            IList<KeyValuePair<ICliFreeValueDefinition, string>> freeValues)
        {
            Command = command ?? throw new ArgumentNullException(nameof(command));
            this.optionValues = TransformOptionValues(optionValues);
            this.optionFlags = optionFlags?.ToArray() ?? Array.Empty<KeyValuePair<ICliOptionDefinition, bool>>();
            this.freeValues = TransformFreeValues(freeValues);
            IsSuccess = true;
        }

        IList<KeyValuePair<ICliFreeValueDefinition, FreeValue>> TransformFreeValues(
            IList<KeyValuePair<ICliFreeValueDefinition, string>> rawFreeValues)
        {
            if (rawFreeValues == null) { return Array.Empty<KeyValuePair<ICliFreeValueDefinition, FreeValue>>(); }

            return rawFreeValues.Select(fv =>
                {
                    ICliFreeValueDefinition freeValueDefinition = fv.Key;
                    IValueTransformer transformer = freeValueDefinition.Transformer;

                    try
                    {
                        string[] rawValues = string.IsNullOrEmpty(fv.Value) ? Array.Empty<string>() : new[] {fv.Value};
                        return new KeyValuePair<ICliFreeValueDefinition, FreeValue>(
                            freeValueDefinition,
                            new FreeValue(fv.Value, transformer.Transform(rawValues)));
                    }
                    catch (CliArgParsingException)
                    {
                        throw;
                    }
                    catch
                    {
                        throw new CliArgParsingException(
                            CliArgsParsingErrorCode.TransformValueFailed,
                            fv.Value);
                    }
                })
                .ToArray();
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
                                CliArgsParsingErrorCode.TransformValueFailed,
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

        FreeValue GetFreeValueObject(string name)
        {
            KeyValuePair<ICliFreeValueDefinition, FreeValue> matchedFreeValue =
                freeValues.FirstOrDefault(f => f.Key.IsMatch(name));
            if (matchedFreeValue.Key == null) 
            {
                throw new ArgumentException($"The free value name you specified is not defined: '{name}");
            }

            return matchedFreeValue.Value;
        }

        public bool GetFlagValues(string flag)
        {
            KeyValuePair<ICliOptionDefinition, bool> matchedFlag = optionFlags.FirstOrDefault(o => o.Key.IsMatch(flag));
            if (matchedFlag.Key == null) { throw new ArgumentException($"The flag you specified is not defined: '{flag}'");}
            return matchedFlag.Value;
        }

        public IList<string> GetOptionRawValue(string option)
        {
            return GetOptionValueObject(option ?? throw new ArgumentNullException(nameof(option))).RawValues;
        }

        public IList<object> GetOptionValue(string option)
        {
            return GetOptionValueObject(option ?? throw new ArgumentNullException(nameof(option))).TransformedValues;
        }

        public IList<object> GetFreeValue(string name)
        {
            return GetFreeValueObject(name ?? throw new ArgumentNullException(nameof(name))).TransformedValues;
        }

        public string GetFreeRawValue(string name)
        {
            return GetFreeValueObject(name ?? throw new ArgumentNullException(nameof(name))).RawValue;
        }

        public IList<string> GetUndefinedFreeValues()
        {
            return freeValues
                .Where(f => f.Key is CliNullFreeValueDefinition)
                .Select(f => f.Value.RawValue)
                .ToArray();
        }
    }
}