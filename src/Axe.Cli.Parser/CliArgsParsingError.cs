using System;

namespace Axe.Cli.Parser
{
    public class CliArgsParsingError
    {
        public CliArgsParsingError(string trigger, CliArgsParsingErrorCode code)
        {
            Trigger = trigger ?? throw new ArgumentNullException(nameof(trigger));
            Code = code;
        }

        public string Trigger { get; }
        public CliArgsParsingErrorCode Code { get; }
    }
}