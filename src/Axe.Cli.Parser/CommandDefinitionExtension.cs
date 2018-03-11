namespace Axe.Cli.Parser
{
    public static class CommandDefinitionExtension
    {
        public static bool IsDefaultCommand(this ICommandDefinitionMetadata commandDefinition)
        {
            return commandDefinition is DefaultCommandDefinition;
        }
    }
}