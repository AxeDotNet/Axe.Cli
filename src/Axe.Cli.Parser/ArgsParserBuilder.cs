namespace Axe.Cli.Parser
{
    /// <summary>
    /// This class is used for creating the <see cref="ArgsParser"/>. 
    /// </summary>
    public class ArgsParserBuilder
    {
        internal ArgsDefinition Definition { get; } = new ArgsDefinition();

        /// <summary>
        /// Begin declaring a default command definition.
        /// </summary>
        /// <returns>
        /// A <see cref="CommandBuilder"/> that holds the information of the command.
        /// </returns>
        public CommandBuilder BeginDefaultCommand()
        {
            return new CommandBuilder(this);
        }

        /// <summary>
        /// Begin declaring a named command definition.
        /// </summary>
        /// <param name="commandName">
        /// The name of the command.
        /// </param>
        /// <param name="description">
        /// The description of the command.
        /// </param>
        /// <returns>
        /// A <see cref="CommandBuilder"/> that holds the information of the command.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// The <paramref name="commandName"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The <paramref name="commandName"/> is of an invalid format.
        /// </exception>
        public CommandBuilder BeginCommand(string commandName, string description)
        {
            return new CommandBuilder(this, commandName, description);
        }

        /// <summary>
        /// Create a <see cref="ArgsParser"/>.
        /// </summary>
        /// <returns>The <see cref="ArgsParser"/> instance.</returns>
        public ArgsParser Build()
        {
            return new ArgsParser(Definition);
        }
    }
}