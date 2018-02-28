namespace Axe.Cli.Parser
{
    public class CliCommandBuilder
    {
        readonly CliArgsParserBuilder parentBuilder;
        readonly ICliCommandDefinition commandDefinition;
        readonly bool isDefaultCommand;

        public CliCommandBuilder(CliArgsParserBuilder parentBuilder, string commandName, string description)
        {
            this.parentBuilder = parentBuilder;
            isDefaultCommand = commandName == null;
            commandDefinition = isDefaultCommand
                ? (ICliCommandDefinition) new CliDefaultCommandDefinition()
                : new CliCommandDefinition(commandName, description);
        }

        public CliCommandBuilder AddOptionWithValue(
            string fullForm,
            char? abbreviation,
            string description,
            bool isRequired = false)
        {
            commandDefinition.RegisterOption(
                new CliOptionDefinition(fullForm, abbreviation, description, isRequired));
            return this;
        }

        public CliCommandBuilder AddFlagOption(
            string fullForm,
            char? abbreviation,
            string description,
            bool isRequired = false)
        {
            commandDefinition.RegisterOption(
                new CliOptionDefinition(fullForm, abbreviation, description, isRequired, OptionType.Flag));
            return this;
        }

        public CliArgsParserBuilder EndCommand()
        {
            if (isDefaultCommand)
            {
                parentBuilder.Definition.SetDefaultCommand((CliDefaultCommandDefinition) commandDefinition);
            }
            else
            {
                parentBuilder.Definition.RegisterCommand((CliCommandDefinition)commandDefinition);
            }

            return parentBuilder;
        }
    }
}