using System;
using System.Collections.Generic;

namespace Axe.Cli.Parser
{
    public class CliArgParsingException : Exception
    {
        readonly CliArgsParsingErrorCode code;
        readonly string trigger;

        public CliArgParsingException(CliArgsParsingErrorCode code, string trigger)
            : base(CreateMessage(code))
        {
            this.code = code;
            this.trigger = trigger;
        }

        static string CreateMessage(CliArgsParsingErrorCode code)
        {
            return messages.ContainsKey(code) ? messages[code] : "Uknown error.";
        }

        static readonly Dictionary<CliArgsParsingErrorCode, string> messages =
            new Dictionary<CliArgsParsingErrorCode, string>
            {
                { CliArgsParsingErrorCode.Unknown, "Unknown error." },
                { CliArgsParsingErrorCode.DoesNotMatchAnyCommand, "The input does not match any command." },
                { CliArgsParsingErrorCode.CannotFindValueForOption, "The option requires a value." },
                { CliArgsParsingErrorCode.FreeValueNotSupported, "This command does not support free value." },
                { CliArgsParsingErrorCode.DuplicateFlagsInArgs, "Duplicate flag switches." },
                { CliArgsParsingErrorCode.UnknownOptionType, "Unsupported option." },
                { CliArgsParsingErrorCode.RequiredOptionNotPresent, "The option is mandatory." },
                { CliArgsParsingErrorCode.TransformOptionValueFailed, "The format of option value is not correct." },
                { CliArgsParsingErrorCode.TransformIntegerValueFailed, "The option value is not an integer." }
            };

        public CliArgsParsingError CreateError()
        {
            return new CliArgsParsingError(trigger, code);
        }
    }
}