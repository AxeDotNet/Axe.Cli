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

        public CliArgsParsingResult Parse(IList<string> args)
        {
            if (args == null) { throw new ArgumentNullException(nameof(args)); }

            try
            {
                return new CliArgsTokenizer(definition).Tokenize(args);
            }
            catch (CliArgParsingException error)
            {
                return new CliArgsParsingResult(error.CreateError());
            }
            catch (Exception)
            {
                return new CliArgsParsingResult(new CliArgsParsingError(string.Join(" ", args), CliArgsParsingErrorCode.Unknown));
            }
        }
    }
}