using System;
using System.Collections.Generic;
using System.Linq;
using Axe.Cli.Parser.Tokenizer;

namespace Axe.Cli.Parser
{
    public class ArgsParser
    {
        readonly ArgsDefinition definition;

        internal ArgsParser(ArgsDefinition definition)
        {
            this.definition = definition ?? throw new ArgumentNullException(nameof(definition));
        }

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