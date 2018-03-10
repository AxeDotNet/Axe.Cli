namespace Axe.Cli.Parser
{
    public class CliCommandBuilder
    {
        readonly CliArgsParserBuilder parentBuilder;
        readonly ICliCommandDefinition commandDefinition;
        readonly bool isDefaultCommand;
        bool allowFreeValue;

        public CliCommandBuilder(CliArgsParserBuilder parentBuilder)
        {
            this.parentBuilder = parentBuilder;
            isDefaultCommand = true;
            commandDefinition = new CliDefaultCommandDefinition();
        }

        public CliCommandBuilder(CliArgsParserBuilder parentBuilder, string commandName, string description)
        {
            this.parentBuilder = parentBuilder;
            isDefaultCommand = false;
            commandDefinition = new CliCommandDefinition(commandName, description);
        }

        public CliCommandBuilder AddOptionWithValue(
            string fullForm,
            char? abbreviation,
            string description,
            bool isRequired = false,
            IValueTransformer transformer = null)
        {
            commandDefinition.RegisterOption(
                new CliOptionDefinition(
                    fullForm,
                    abbreviation,
                    description,
                    isRequired,
                    OptionType.KeyValue,
                    transformer));
            return this;
        }

        public CliCommandBuilder AddFlagOption(
            string fullForm,
            char? abbreviation,
            string description)
        {
            commandDefinition.RegisterOption(
                new CliOptionDefinition(fullForm, abbreviation, description, false, OptionType.Flag));
            return this;
        }

        public CliCommandBuilder ConfigFreeValue(bool allow = false)
        {
            allowFreeValue = allow;
            return this;
        }

        public CliArgsParserBuilder EndCommand()
        {
            if (isDefaultCommand)
            {
                var command = (CliDefaultCommandDefinition) commandDefinition;
                command.AllowFreeValue = allowFreeValue;
                parentBuilder.Definition.SetDefaultCommand(command);
            }
            else
            {
                var command = (CliCommandDefinition)commandDefinition;
                command.AllowFreeValue = allowFreeValue;
                parentBuilder.Definition.RegisterCommand(command);
            }

            return parentBuilder;
        }

        public CliCommandBuilder AddFreeValue(string name, string description)
        {
            allowFreeValue = true;
            var definition = new CliFreeValueDefinition(name, description);
            commandDefinition.RegisterFreeValue(definition);
            return this;
        }
    }
}