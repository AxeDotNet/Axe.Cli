using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Axe.Cli.Parser.Tokenizer
{
    class StartState : PreParsingStateBase
    {
        readonly ArgsDefinition definition;
        readonly PreParserResultBuilder resultBuilder;

        public StartState(ArgsDefinition definition, PreParserResultBuilder resultBuilder)
        {
            Debug.Assert(definition != null);
            Debug.Assert(resultBuilder != null);

            this.definition = definition;
            this.resultBuilder = resultBuilder;
        }

        public override IPreParsingState MoveToNext(string argument)
        {
            if (IsEndOfArguments(argument))
            {
                return HandleEndOfArgument();
            }

            ICommandDefinition selectedCommand = ResolveCommand(argument);
            if (selectedCommand != null)
            {
                resultBuilder.SetCommand(selectedCommand);
                return new ContinueState(selectedCommand, resultBuilder);
            }

            selectedCommand = EnsureDefaultCommandSet(argument);

            IPreParsingState nextStateForKeyValueOption = HandleKeyValueOptionArgument(
                selectedCommand,
                resultBuilder,
                argument);
            if (nextStateForKeyValueOption != null) { return nextStateForKeyValueOption; }
            
            IPreParsingState nextStateForFlagOption = HandleFlagOptionArgument(selectedCommand, resultBuilder, argument);
            if (nextStateForFlagOption != null) { return nextStateForFlagOption;}

            return HandleFreeValueArgument(selectedCommand, resultBuilder, argument);
        }

        ICommandDefinition EnsureDefaultCommandSet(string argument)
        {
            if (!HasDefaultCommand)
            {
                throw new ArgParsingException(
                    ArgsParsingErrorCode.DoesNotMatchAnyCommand,
                    argument);
            }

            resultBuilder.SetCommand(definition.DefaultCommand);
            return definition.DefaultCommand;
        }

        IPreParsingState HandleEndOfArgument()
        {
            if (!HasDefaultCommand)
            {
                throw new ArgParsingException(
                    ArgsParsingErrorCode.DoesNotMatchAnyCommand, "Unexpected end of arguments.");
            }

            resultBuilder.SetCommand(definition.DefaultCommand);
            return null;
        }

        ICommandDefinition ResolveCommand(string argument)
        {
            if (!CommandDefinition.CanBeCommand(argument))
            {
                return null;
            }

            IEnumerable<ICommandDefinition> commands = definition.GetRegisteredCommands();
            return commands.FirstOrDefault(c => c.IsMatch(argument));
        }

        bool HasDefaultCommand => definition.DefaultCommand != null;
    }
}