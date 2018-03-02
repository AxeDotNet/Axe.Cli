using System.Collections.Generic;
using System.Diagnostics;

namespace Axe.Cli.Parser.Tokenizer
{
    class ContinueWithCommandState : TokenizerStateBase
    {
        readonly ICliCommandDefinition command;
        readonly TokenizedResultBuilder resultBuilder;

        public ContinueWithCommandState(ICliCommandDefinition command, TokenizedResultBuilder resultBuilder)
        {
            Debug.Assert(command != null);
            Debug.Assert(resultBuilder != null);

            this.command = command;
            this.resultBuilder = resultBuilder;
        }

        public override ITokenizerState MoveToNext(string argument)
        {
            if (IsEndOfArguments(argument)) { return null; }
            
            ICliOptionDefinition kvOption = ResolveKeyValueOptionLabel(command, argument);
            if (kvOption != null)
            {
                return new WaitingValueWithCommandState(command, kvOption, argument, resultBuilder);
            }

            IList<ICliOptionDefinition> flagOptions = ResolveFlagOptionLabels(command, argument);
            if (flagOptions.Count > 0)
            {
                foreach (ICliOptionDefinition flagOption in flagOptions)
                {
                    resultBuilder.AppendOptionToken(new CliOptionToken(flagOption), argument);
                }
                return new ContinueWithCommandState(command, resultBuilder);
            }

            throw new CliArgParsingException(CliArgsParsingErrorCode.FreeValueNotSupported, argument);
        }
    }
}