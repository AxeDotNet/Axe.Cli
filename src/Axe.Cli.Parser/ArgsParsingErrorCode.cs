namespace Axe.Cli.Parser
{
    /// <summary>
    /// The error code represents the possible parsing error.
    /// </summary>
    public enum ArgsParsingErrorCode
    {
        /// <summary>
        /// The error is caused by an unknown error. This is usually caused by an unexpected
        /// exception.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The command line argument matches no named command. And the command line argument
        /// definition contains no default command.
        /// </summary>
        DoesNotMatchAnyCommand,

        /// <summary>
        /// The command line parser receives a key argument, but cannot find the value part to
        /// that key.
        /// </summary>
        CannotFindValueForOption,

        /// <summary>
        /// The command line parser receives a free value argument. But free value is not supported
        /// according to current definitions.
        /// </summary>
        FreeValueNotSupported,

        /// <summary>
        /// The command line parser receives duplicated flag arguments.
        /// </summary>
        DuplicateFlagsInArgs,

        /// <summary>
        /// The command line parser receives a option argument, but it is not a key-value pair or
        /// a flag argument. This is usually caused by an implementation error.
        /// </summary>
        UnknownOptionType,

        /// <summary>
        /// The command line parser cannot find a required key-value option from the argument.
        /// </summary>
        RequiredOptionNotPresent,

        /// <summary>
        /// The command line parser cannot translate values to an target type.
        /// </summary>
        TransformValueFailed,

        /// <summary>
        /// The command line parser cannot translate values to an integer.
        /// </summary>
        TransformIntegerValueFailed
    }
}