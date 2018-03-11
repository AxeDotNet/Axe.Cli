namespace Axe.Cli.Parser
{
    public class CommandBuilder
    {
        readonly ArgsParserBuilder parentBuilder;
        readonly ICommandDefinition commandDefinition;
        readonly bool isDefaultCommand;
        bool allowFreeValue;

        public CommandBuilder(ArgsParserBuilder parentBuilder)
        {
            this.parentBuilder = parentBuilder;
            isDefaultCommand = true;
            commandDefinition = new DefaultCommandDefinition();
        }

        public CommandBuilder(ArgsParserBuilder parentBuilder, string commandName, string description)
        {
            this.parentBuilder = parentBuilder;
            isDefaultCommand = false;
            commandDefinition = new CommandDefinition(commandName, description);
        }

        public CommandBuilder AddOptionWithValue(
            string fullForm,
            char? abbreviation,
            string description,
            bool isRequired = false,
            ValueTransformer transformer = null)
        {
            commandDefinition.RegisterOption(
                new OptionDefinition(
                    fullForm,
                    abbreviation,
                    description,
                    isRequired,
                    OptionType.KeyValue,
                    transformer));
            return this;
        }

        public CommandBuilder AddFlagOption(
            string fullForm,
            char? abbreviation,
            string description)
        {
            commandDefinition.RegisterOption(
                new OptionDefinition(fullForm, abbreviation, description, false, OptionType.Flag));
            return this;
        }

        public CommandBuilder ConfigFreeValue(bool allow = false)
        {
            allowFreeValue = allow;
            return this;
        }

        public ArgsParserBuilder EndCommand()
        {
            if (isDefaultCommand)
            {
                var command = (DefaultCommandDefinition) commandDefinition;
                command.AllowFreeValue = allowFreeValue;
                parentBuilder.Definition.SetDefaultCommand(command);
            }
            else
            {
                var command = (CommandDefinition)commandDefinition;
                command.AllowFreeValue = allowFreeValue;
                parentBuilder.Definition.RegisterCommand(command);
            }

            return parentBuilder;
        }

        public CommandBuilder AddFreeValue(string name, string description, ValueTransformer transformer = null)
        {
            allowFreeValue = true;
            var definition = new FreeValueDefinition(name, description, transformer);
            commandDefinition.RegisterFreeValue(definition);
            return this;
        }
    }
}