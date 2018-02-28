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
                TokenizedResult tokenResult = new CliArgsTokenizer(definition).Tokenize(args);
                return Merge(tokenResult);
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

        static CliArgsParsingResult Merge(TokenizedResult tokenResult)
        {
            ICliCommandSymbolDefinition command = tokenResult.Command;
            return new CliArgsParsingResult(command, null, null, null);
        }
    }
}