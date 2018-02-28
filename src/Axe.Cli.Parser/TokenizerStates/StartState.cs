using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Axe.Cli.Parser.TokenizerStates
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

            ICliCommandSymbolDefinition defaultCommand = EnsureDefaultCommandSet(argument);

            ICliOptionDefinition kvOption = ResolveKeyValueOptionLabel(
                (ICliOptionDefinitionContainer) defaultCommand,
                argument);
            if (kvOption != null)
            {
                return new WaitingValueWithCommandState(defaultCommand, kvOption, argument, resultBuilder);
            }

            throw new NotImplementedException();
        }

        ICliCommandSymbolDefinition EnsureDefaultCommandSet(string argument)
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

    class WaitingValueWithCommandState : TokenizerStateBase
    {
        readonly ICliCommandSymbolDefinition defaultCommand;
        readonly ICliOptionDefinition kvOption;
        readonly string labelArgument;
        readonly TokenizedResultBuilder resultBuilder;

        public WaitingValueWithCommandState(
            ICliCommandSymbolDefinition defaultCommand,
            ICliOptionDefinition kvOption,
            string labelArgument,
            TokenizedResultBuilder resultBuilder)
        {
            Debug.Assert(defaultCommand != null);
            Debug.Assert(kvOption != null);
            Debug.Assert(resultBuilder != null);

            this.defaultCommand = defaultCommand;
            this.kvOption = kvOption;
            this.labelArgument = labelArgument;
            this.resultBuilder = resultBuilder;
        }

        public override ITokenizerState MoveToNext(string argument)
        {
            if (IsEndOfArguments(argument))
            {
                throw new CliArgParsingException(CliArgsParsingErrorCode.CannotFindValueForOption, labelArgument);
            }

            throw new NotImplementedException();
        }
    }
}