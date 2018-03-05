using System;
using System.Collections.Generic;
using Axe.Cli.Parser.Tokenizer;

namespace Axe.Cli.Parser
{
    public class CliArgsParser
    {
        readonly CliArgsDefinition definition;

        internal CliArgsParser(CliArgsDefinition definition)
        {
            this.definition = definition ?? throw new ArgumentNullException(nameof(definition));
        }

        public CliArgsPreParsingResult Parse(IList<string> args)
        {
            if (args == null) { throw new ArgumentNullException(nameof(args)); }

            try
            {
                return new CliArgsPreParser(definition).Parse(args);
            }
            catch (CliArgParsingException error)
            {
                return new CliArgsPreParsingResult(error.CreateError());
            }
            catch (Exception)
            {
                return new CliArgsPreParsingResult(new CliArgsParsingError(string.Join(" ", args), CliArgsParsingErrorCode.Unknown));
            }
        }
    }
}