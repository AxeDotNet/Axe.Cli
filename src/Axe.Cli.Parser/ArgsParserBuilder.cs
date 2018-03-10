namespace Axe.Cli.Parser
{
    public class ArgsParserBuilder
    {
        internal ArgsDefinition Definition { get; } = new ArgsDefinition();

        public CommandBuilder BeginDefaultCommand()
        {
            return new CommandBuilder(this);
        }

        public CommandBuilder BeginCommand(string commandName, string description)
        {
            return new CommandBuilder(this, commandName, description);
        }

        public ArgsParser Build()
        {
            return new ArgsParser(Definition);
        }
    }
}