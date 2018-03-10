namespace Axe.Cli.Parser
{
    public enum ArgsParsingErrorCode
    {
        Unknown = 0,
        DoesNotMatchAnyCommand,
        CannotFindValueForOption,
        FreeValueNotSupported,
        DuplicateFlagsInArgs,
        UnknownOptionType,
        RequiredOptionNotPresent,
        TransformValueFailed,
        TransformIntegerValueFailed
    }
}