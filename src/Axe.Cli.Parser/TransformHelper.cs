using System;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Cli.Parser
{
    static class TransformHelper
    {
        public static IList<KeyValuePair<IFreeValueDefinition, FreeValue>> TransformFreeValues(
            IList<KeyValuePair<IFreeValueDefinition, string>> rawFreeValues)
        {
            if (rawFreeValues == null) { return Array.Empty<KeyValuePair<IFreeValueDefinition, FreeValue>>(); }

            return rawFreeValues.Select(fv =>
                {
                    IFreeValueDefinition freeValueDefinition = fv.Key;
                    ValueTransformer transformer = freeValueDefinition.Transformer;

                    try
                    {
                        IList<object> transformed = transformer.Transform(fv.Value);
                        return new KeyValuePair<IFreeValueDefinition, FreeValue>(
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

        public static IList<KeyValuePair<IOptionDefinition, OptionValue>> TransformOptionValues(
            IEnumerable<KeyValuePair<IOptionDefinition, IList<string>>> rawOptionValues)
        {
            if (rawOptionValues == null) { return Array.Empty<KeyValuePair<IOptionDefinition, OptionValue>>(); }

            return rawOptionValues
                .Select(
                    ov =>
                    {
                        IOptionDefinition optionDefinition = ov.Key;
                        ValueTransformer transformer = optionDefinition.Transformer;

                        try
                        {
                            return new KeyValuePair<IOptionDefinition, OptionValue>(
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