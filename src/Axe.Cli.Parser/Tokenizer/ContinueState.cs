using System.Collections.Generic;
using System.Diagnostics;

namespace Axe.Cli.Parser.Tokenizer
{
    class ContinueState : PreParsingStateBase
    {
        readonly ICliCommandDefinition command;
        readonly PreParserResultBuilder resultBuilder;

        public ContinueState(ICliCommandDefinition command, PreParserResultBuilder resultBuilder)
        {
            Debug.Assert(command != null);
            Debug.Assert(resultBuilder != null);

            this.command = command;
            this.resultBuilder = resultBuilder;
        }

        public override IPreParsingState MoveToNext(string argument)
        {
            if (IsEndOfArguments(argument)) { return null; }
            
            ICliOptionDefinition kvOption = ResolveKeyValueOptionLabel(command, argument);
            if (kvOption != null)
            {
                return new WaitingValueState(command, kvOption, argument, resultBuilder);
            }

            IList<ICliOptionDefinition> flagOptions = ResolveFlagOptionLabels(command, argument);
            if (flagOptions.Count > 0)
            {
                foreach (ICliOptionDefinition flagOption in flagOptions)
                {
                    resultBuilder.AppendOptionToken(new CliOptionToken(flagOption), argument);
                }
                return new ContinueState(command, resultBuilder);
            }

            return HandleFreeValueArgument(command, resultBuilder, argument);
        }
    }
}