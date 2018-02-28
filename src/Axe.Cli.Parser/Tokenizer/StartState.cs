using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Axe.Cli.Parser.Tokenizer
{
    class StartState : TokenizerStateBase
    {
        readonly CliArgsDefinition definition;
        readonly TokenizedResultBuilder resultBuilder;

        public StartState(CliArgsDefinition definition, TokenizedResultBuilder resultBuilder)
        {
            Debug.Assert(definition != null);
            Debug.Assert(resultBuilder != null);

            this.definition = definition;
            this.resultBuilder = resultBuilder;
        }

        public override ITokenizerState MoveToNext(string argument)
        {
            if (IsEndOfArguments(argument))
            {
                return HandleEndOfArgument();
            }

            CliCommandDefinition command = ResolveCommand(argument);
            if (command != null)
            {
                resultBuilder.SetCommand(command);
                return new ContinueWithCommandState(command, resultBuilder);
            }

            ICliCommandDefinition defaultCommand = EnsureDefaultCommandSet(argument);

            ICliOptionDefinition kvOption = ResolveKeyValueOptionLabel(
                (ICliCommandDefinition) defaultCommand,
                argument);
            if (kvOption != null)
            {
                return new WaitingValueWithCommandState(defaultCommand, kvOption, argument, resultBuilder);
            }

            throw new NotImplementedException();
        }

        ICliCommandDefinition EnsureDefaultCommandSet(string argument)
        {
            if (!HasDefaultCommand)
            {
                throw new CliArgParsingException(
                    CliArgsParsingErrorCode.DoesNotMatchAnyCommand,
                    argument);
            }

            resultBuilder.SetCommand(definition.DefaultCommand);
            return definition.DefaultCommand;
        }

        ITokenizerState HandleEndOfArgument()
        {
            if (!HasDefaultCommand)
            {
                throw new CliArgParsingException(
                    CliArgsParsingErrorCode.DoesNotMatchAnyCommand, "Unexpected end of arguments.");
            }

            resultBuilder.SetCommand(definition.DefaultCommand);
            return null;
        }

        CliCommandDefinition ResolveCommand(string argument)
        {
            IReadOnlyList<CliCommandDefinition> commands = definition.GetRegisteredCommands();
            return commands.FirstOrDefault(c => c.IsMatch(argument));
        }

        bool HasDefaultCommand => definition.DefaultCommand != null;
    }
}