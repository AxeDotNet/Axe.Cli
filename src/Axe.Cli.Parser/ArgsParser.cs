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
                return ParseInternal(args);
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
        
        ArgsParsingResult ParseInternal(IList<string> args)
        {
            /*
             * (start) --[EoA]--> (no default? error|stop)
             * (start) --[command]--> (save command|continue with command)
             * (start) --[option(kv) in default command]--> (save default command|waiting value with command)
             * (start) --[option-groups(f) in default command]--> (save default command|yield flag|continue with command)
             * (start) --[other]--> (save default command|yield free value|continue with command-free value)
             * 
             * (continue with command) --[option(kv) in command]--> (waiting value with command)
             * (continue with command) --[option-groups(f) in command]--> (yield flag|continue with command)
             * (continue with command) --[EoA]--> (stop)
             * (continue with command) --[other]--> (yield free value|continue with command-free value)
             * 
             * (waiting value with command) --[EoA]--> (error|stop)
             * (waiting value with command) --[other]--> (yield key value|continue with command)
             * 
             * (continue with command-free value) --[EoA]--> (stop)
             * (continue with command-free value) --[other] --> (yield free value|continue with command-free value)
             */

            var builder = new PreParserResultBuilder();
            IPreParsingState state = new StartState(definition, builder);
            foreach (string arg in args)
            {
                state = state.MoveToNext(arg);
                if (state == null) { return builder.Build(); }
            }

            state.MoveToNext(null);
            return builder.Build();
        }
    }
}