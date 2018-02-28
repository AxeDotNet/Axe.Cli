namespace Axe.Cli.Parser
{
    class CliDefaultCommandDefinition : CliCommandDefinitionBase
    {
        public override string Symbol => null;
        public override string Description => null;

        public override bool IsConflict(ICliCommandDefinition commandDefinition)
        {
            return true;
        }

        public override bool IsMatch(string argument)
        {
            return false;
        }

        public override string ToString()
        {
            return "DEFAULT_COMMAND";
        }
    }
}