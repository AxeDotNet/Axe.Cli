using System;
using System.Linq;

namespace Axe.Cli.Parser
{
    /// <summary>
    /// Represent a builder to create a command definition.
    /// </summary>
    public class CommandBuilder
    {
        readonly ArgsParserBuilder parentBuilder;
        readonly ICommandDefinition commandDefinition;
        readonly bool isDefaultCommand;
        bool allowFreeValue;

        internal CommandBuilder(ArgsParserBuilder parentBuilder)
        {
            this.parentBuilder = parentBuilder;
            isDefaultCommand = true;
            commandDefinition = new DefaultCommandDefinition();
        }

        internal CommandBuilder(ArgsParserBuilder parentBuilder, string commandName, string description)
        {
            this.parentBuilder = parentBuilder;
            isDefaultCommand = false;
            commandDefinition = new CommandDefinition(commandName, description);
        }

        /// <summary>
        /// Add a key-value option for current command.
        /// </summary>
        /// <param name="fullForm">
        /// The full form of the key. If current key does not contains a full form. The argument
        /// should be <c>null</c>.
        /// </param>
        /// <param name="abbreviation">
        /// The abbreviation form of the key. If current key does not contains a abbreviation form.
        /// The argument should be <c>null</c>.
        /// </param>
        /// <param name="description">
        /// The description for this key-value option. Please note that all the line-breaks will be
        /// removed for consisitency.
        /// </param>
        /// <param name="isRequired">
        /// Indicating whether this key-value option is required.
        /// </param>
        /// <param name="transformer">
        /// The transformer of the value. If you would like to keep the original string representation,
        /// just pass <c>null</c>.
        /// </param>
        /// <returns>The command builder instance.</returns>
        /// <exception cref="ArgumentException">
        /// <para>
        /// The <paramref name="fullForm"/> and <paramref name="abbreviation"/> both are <c>null</c>.
        /// </para>
        /// <para>-- Or --</para>
        /// <para>
        /// The <paramref name="fullForm"/> is not a valid identifier.
        /// </para>
        /// <para>-- Or --</para>
        /// <para>
        /// The <paramref name="abbreviation"/> is not an english alphabet.
        /// </para>
        /// <para>-- Or --</para>
        /// <para>
        /// This definition is conflict with another existed definition.
        /// </para>
        /// </exception>
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

        /// <summary>
        /// Add a flag option for current command. A flag is actually a switch act as a boolean
        /// variable.
        /// </summary>
        /// <param name="fullForm">
        /// The full form of the key. If current key does not contains a full form. The argument
        /// should be <c>null</c>.
        /// </param>
        /// <param name="abbreviation">
        /// The abbreviation form of the key. If current key does not contains a abbreviation form.
        /// The argument should be <c>null</c>.
        /// </param>
        /// <param name="description">
        /// The description for this flag option.
        /// </param>
        /// <returns>The command builder instance.</returns>
        /// <exception cref="ArgumentException">
        /// <para>
        /// The <paramref name="fullForm"/> and <paramref name="abbreviation"/> both are <c>null</c>.
        /// </para>
        /// <para>-- Or --</para>
        /// <para>
        /// The <paramref name="fullForm"/> is not a valid identifier.
        /// </para>
        /// <para>-- Or --</para>
        /// <para>
        /// The <paramref name="abbreviation"/> is not an english alphabet.
        /// </para>
        /// <para>-- Or --</para>
        /// <para>
        /// This definition is conflict with another existed definition.
        /// </para>
        /// </exception>
        public CommandBuilder AddFlagOption(
            string fullForm,
            char? abbreviation,
            string description)
        {
            commandDefinition.RegisterOption(
                new OptionDefinition(fullForm, abbreviation, description, false, OptionType.Flag));
            return this;
        }

        /// <summary>
        /// Add a free value definition for current command. This operation will automatically enable
        /// free value for this command.
        /// </summary>
        /// <param name="name">The name of the free value.</param>
        /// <param name="description">
        /// The description of the free value. Please note that all the line-breaks will be
        /// removed for consisitency.
        /// </param>
        /// <param name="isRequired">
        /// Whether the free value is mandatory.
        /// </param>
        /// <param name="transformer">
        /// The transformer of the value. If you would like to keep the original string representation,
        /// just pass <c>null</c>.
        /// </param>
        /// <returns>The command builder instance.</returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="name"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Current free value is conflict with existing free value definitions.
        /// </exception>
        public CommandBuilder AddFreeValue(string name, string description, bool isRequired = false, ValueTransformer transformer = null)
        {
            var definition = new FreeValueDefinition(name, description, isRequired, transformer);
            commandDefinition.RegisterFreeValue(definition);
            allowFreeValue = true;
            return this;
        }

        /// <summary>
        /// Configure whether or not to allow free values for this command. Note that if you use
        /// <see cref="AddFreeValue"/>. The free values will automatically enabled for this command.
        /// </summary>
        /// <param name="allow">Enable free values for this command.</param>
        /// <returns>The command builder instance.</returns>
        /// <exception cref="InvalidOperationException">
        /// There is already at least one free value definitions added by <see cref="AddFreeValue"/>
        /// while you would like to disable free value usage.
        /// </exception>
        public CommandBuilder ConfigFreeValue(bool allow = false)
        {
            if (commandDefinition.GetRegisteredFreeValues().Any())
            {
                throw new InvalidOperationException(
                    "You cannot disable free value because you have already add free value definitions.");
            }

            allowFreeValue = allow;
            return this;
        }

        /// <summary>
        /// Finalize current command definition.
        /// </summary>
        /// <returns>The parser builder instance.</returns>
        /// <exception cref="InvalidOperationException">
        /// <para>
        /// You would like to finalized a default command but a default command has already been
        /// defined.
        /// </para>
        /// <para>-- Or --</para>
        /// <para>
        /// The command registered is conflict with existed command.
        /// </para>
        /// </exception>
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
    }
}