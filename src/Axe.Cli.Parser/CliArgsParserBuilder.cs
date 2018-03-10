namespace Axe.Cli.Parser
{
    public class CliArgsParserBuilder
    {
        internal CliArgsDefinition Definition { get; } = new CliArgsDefinition();

        public CliCommandBuilder BeginDefaultCommand()
        {
            return new CliCommandBuilder(this);
        }

        public CliCommandBuilder BeginCommand(string commandName, string description)
        {
            return new CliCommandBuilder(this, commandName, description);
        }

        public CliArgsParser Build()
        {
            return new CliArgsParser(Definition);
        }
    }
}