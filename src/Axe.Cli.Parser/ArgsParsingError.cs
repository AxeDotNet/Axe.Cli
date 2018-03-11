using System;

namespace Axe.Cli.Parser
{
    /// <summary>
    /// Represents the error information that occured during parsing process.
    /// </summary>
    public class ArgsParsingError
    {
        internal ArgsParsingError(string trigger, ArgsParsingErrorCode code)
        {
            Trigger = trigger ?? throw new ArgumentNullException(nameof(trigger));
            Code = code;
        }

        /// <summary>
        /// Represent the original argument or parts that cause the error happen.
        /// </summary>
        public string Trigger { get; }

        /// <summary>
        /// The error code that represent the type of the error.
        /// </summary>
        public ArgsParsingErrorCode Code { get; }
    }
}