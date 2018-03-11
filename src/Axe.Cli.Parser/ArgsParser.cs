using System;
using System.Collections.Generic;
using System.Linq;
using Axe.Cli.Parser.Tokenizer;

namespace Axe.Cli.Parser
{
    /// <summary>
    /// Prepresent the parser to parse the command line arguments. Please use
    /// <see cref="ArgsParserBuilder"/> to create the parser.
    /// </summary>
    public class ArgsParser
    {
        readonly ArgsDefinition definition;

        internal ArgsParser(ArgsDefinition definition)
        {
            this.definition = definition ?? throw new ArgumentNullException(nameof(definition));
        }

        /// <summary>
        /// Parse the command line arguments.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>
        /// The parsing result.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="args"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// There is at least one element in <paramref name="args"/> that is <c>null</c>.
        /// </exception>
        public ArgsParsingResult Parse(IList<string> args)
        {
            if (args == null) { throw new ArgumentNullException(nameof(args)); }
            if (args.Any(arg => arg == null))
            {
                throw new ArgumentException("The command argument contains null element.");
            }
            
            try
            {
                return new ArgsPreParser(definition).Parse(args);
            }
            catch (ArgParsingException error)
            {
                return new ArgsParsingResult(error.CreateError());
            }
            catch (Exception)
            {
                return new ArgsParsingResult(new ArgsParsingError(string.Join(" ", args), ArgsParsingErrorCode.Unknown));
            }
        }
    }
}