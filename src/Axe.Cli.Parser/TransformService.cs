using System;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Cli.Parser
{
    class TransformService
    {
        public static IList<KeyValuePair<ICliFreeValueDefinition, FreeValue>> TransformFreeValues(
            IList<KeyValuePair<ICliFreeValueDefinition, string>> rawFreeValues)
        {
            if (rawFreeValues == null) { return Array.Empty<KeyValuePair<ICliFreeValueDefinition, FreeValue>>(); }

            return rawFreeValues.Select(fv =>
                {
                    ICliFreeValueDefinition freeValueDefinition = fv.Key;
                    ValueTransformer transformer = freeValueDefinition.Transformer;

                    try
                    {
                        IList<object> transformed = transformer.Transform(fv.Value);
                        return new KeyValuePair<ICliFreeValueDefinition, FreeValue>(
                            freeValueDefinition,
                            new FreeValue(fv.Value, transformed));
                    }
                    catch (ArgParsingException)
                    {
                        throw;
                    }
                    catch
                    {
                        throw new ArgParsingException(
                            ArgsParsingErrorCode.TransformValueFailed,
                            fv.Value);
                    }
                })
                .ToArray();
        }

        public static IList<KeyValuePair<ICliOptionDefinition, OptionValue>> TransformOptionValues(
            IEnumerable<KeyValuePair<ICliOptionDefinition, IList<string>>> rawOptionValues)
        {
            if (rawOptionValues == null) { return Array.Empty<KeyValuePair<ICliOptionDefinition, OptionValue>>(); }

            return rawOptionValues
                .Select(
                    ov =>
                    {
                        ICliOptionDefinition optionDefinition = ov.Key;
                        ValueTransformer transformer = optionDefinition.Transformer;

                        try
                        {
                            return new KeyValuePair<ICliOptionDefinition, OptionValue>(
                                optionDefinition,
                                new OptionValue(ov.Value, transformer.Transform(ov.Value)));
                        }
                        catch (ArgParsingException)
                        {
                            throw;
                        }
                        catch
                        {
                            throw new ArgParsingException(
                                ArgsParsingErrorCode.TransformValueFailed,
                                $"Option: {ov.Key.ToString()}; Values: {String.Join(" ", ov.Value)}.");
                        }
                    })
                .ToArray();
        }
    }
}