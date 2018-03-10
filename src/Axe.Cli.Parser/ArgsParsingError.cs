using System;

namespace Axe.Cli.Parser
{
    public class ArgsParsingError
    {
        public ArgsParsingError(string trigger, ArgsParsingErrorCode code)
        {
            Trigger = trigger ?? throw new ArgumentNullException(nameof(trigger));
            Code = code;
        }

        public string Trigger { get; }
        public ArgsParsingErrorCode Code { get; }
    }
}