namespace Axe.Cli.Parser
{
    class DefaultCommandDefinition : CommandDefinitionBase
    {
        public override string Symbol => null;
        public override string Description => null;

        public override bool IsConflict(ICommandDefinition commandDefinition)
        {
            return true;
        }

        public override bool IsMatch(string argument)
        {
            return false;
        }

        public override string ToString()
        {
            return "default command";
        }
    }
}