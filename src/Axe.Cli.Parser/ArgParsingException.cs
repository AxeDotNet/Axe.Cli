﻿using System;
using System.Collections.Generic;

namespace Axe.Cli.Parser
{
    class ArgParsingException : Exception
    {
        readonly ArgsParsingErrorCode code;
        readonly string trigger;

        internal ArgParsingException(ArgsParsingErrorCode code, string trigger)
            : base(CreateMessage(code))
        {
            this.code = code;
            this.trigger = trigger;
        }

        static string CreateMessage(ArgsParsingErrorCode code)
        {
            return Messages.ContainsKey(code) ? Messages[code] : "Uknown error.";
        }

        static readonly Dictionary<ArgsParsingErrorCode, string> Messages =
            new Dictionary<ArgsParsingErrorCode, string>
            {
                { ArgsParsingErrorCode.Unknown, "Unknown error." },
                { ArgsParsingErrorCode.DoesNotMatchAnyCommand, "The input does not match any command." },
                { ArgsParsingErrorCode.CannotFindValueForOption, "The option requires a value." },
                { ArgsParsingErrorCode.FreeValueNotSupported, "This command does not support free value." },
                { ArgsParsingErrorCode.DuplicateFlagsInArgs, "Duplicate flag switches." },
                { ArgsParsingErrorCode.UnknownOptionType, "Unsupported option." },
                { ArgsParsingErrorCode.RequiredOptionNotPresent, "The option is mandatory." },
                { ArgsParsingErrorCode.RequiredFreeValueNotPresent, "The free value is mandatory. " },
                { ArgsParsingErrorCode.TransformValueFailed, "The format of value is not correct." },
                { ArgsParsingErrorCode.TransformIntegerValueFailed, "The value is not an integer." }
            };
        
        internal ArgsParsingError CreateError()
        {
            return new ArgsParsingError(trigger, code);
        }
    }
}