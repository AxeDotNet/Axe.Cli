using System;
using System.Collections.Generic;

namespace Axe.Cli.Parser
{
    public class ArgParsingException : Exception
    {
        readonly ArgsParsingErrorCode code;
        readonly string trigger;

        public ArgParsingException(ArgsParsingErrorCode code, string trigger)
            : base(CreateMessage(code))
        {
            this.code = code;
            this.trigger = trigger;
        }

        static string CreateMessage(ArgsParsingErrorCode code)
        {
            return messages.ContainsKey(code) ? messages[code] : "Uknown error.";
        }

        static readonly Dictionary<ArgsParsingErrorCode, string> messages =
            new Dictionary<ArgsParsingErrorCode, string>
            {
                { ArgsParsingErrorCode.Unknown, "Unknown error." },
                { ArgsParsingErrorCode.DoesNotMatchAnyCommand, "The input does not match any command." },
                { ArgsParsingErrorCode.CannotFindValueForOption, "The option requires a value." },
                { ArgsParsingErrorCode.FreeValueNotSupported, "This command does not support free value." },
                { ArgsParsingErrorCode.DuplicateFlagsInArgs, "Duplicate flag switches." },
                { ArgsParsingErrorCode.UnknownOptionType, "Unsupported option." },
                { ArgsParsingErrorCode.RequiredOptionNotPresent, "The option is mandatory." },
                { ArgsParsingErrorCode.TransformValueFailed, "The format of value is not correct." },
                { ArgsParsingErrorCode.TransformIntegerValueFailed, "The value is not an integer." }
            };

        public ArgsParsingError CreateError()
        {
            return new ArgsParsingError(trigger, code);
        }
    }
}