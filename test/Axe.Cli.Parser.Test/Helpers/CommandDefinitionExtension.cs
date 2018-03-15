namespace Axe.Cli.Parser.Test.Helpers
{
    public static class CommandDefinitionExtension
    {
        public static bool IsDefaultCommand(this ICommandDefinitionMetadata commandDefinition)
        {
            return commandDefinition.ToString() == "default command";
        }
    }
}