namespace Axe.Cli.Parser
{
    public enum CliArgsParsingErrorCode
    {
        Unknown = 0,
        DoesNotMatchAnyCommand,
        CannotFindValueForOption
    }
}