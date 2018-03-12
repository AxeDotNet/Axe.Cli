namespace Axe.Cli.Parser
{
    /// <summary>
    /// Represents the type of option in command.
    /// </summary>
    public enum OptionType
    {
        /// <summary>
        /// The flag command option. Acts as a boolean value.
        /// </summary>
        Flag = 1,
        
        /// <summary>
        /// The key-value command option.
        /// </summary>
        KeyValue = 2
    }
}