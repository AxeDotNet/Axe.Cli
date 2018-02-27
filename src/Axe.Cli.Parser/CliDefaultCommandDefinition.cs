namespace Axe.Cli.Parser
{
    class CliDefaultCommandDefinition : CliCommandDefinitionBase
    {
        public override bool IsConflict(ICliCommandDefinition commandDefinition)
        {
            return true;
        }

        public override string ToString()
        {
            return "DEFAULT_COMMAND";
        }
    }
}