using System;
using System.Collections.Generic;
using System.Linq;
using Axe.Cli.Parser.Extensions;
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
            ICliCommandDefinition command = tokenResult.Command;
            IList<KeyValuePair<ICliOptionDefinition, bool>> flags = MergeFlags(tokenResult, command);

            return new CliArgsParsingResult(command, null, flags, null);
        }

        static IList<KeyValuePair<ICliOptionDefinition, bool>> MergeFlags(TokenizedResult tokenResult, ICliCommandDefinition command)
        {
            Dictionary<ICliOptionDefinition, bool> flags = tokenResult
                .Tokens
                .Where(t => t.Definition.Type == OptionType.Flag)
                .MergeToDictionary(t => t.Definition, t => true);
            foreach (var flag in command.GetRegisteredOptions()
                .Where(o => o.Type == OptionType.Flag))
            {
                if (flags.ContainsKey(flag)) { continue; }
                flags.Add(flag, false);
            }

            return flags.ToArray();
        }
    }
}