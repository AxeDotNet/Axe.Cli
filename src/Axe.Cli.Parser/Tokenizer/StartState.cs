using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Axe.Cli.Parser.Tokenizer
{
    class StartState : PreParsingStateBase
    {
        readonly CliArgsDefinition definition;
        readonly PreParserResultBuilder resultBuilder;

        public StartState(CliArgsDefinition definition, PreParserResultBuilder resultBuilder)
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

            ICliCommandDefinition selectedCommand = ResolveCommand(argument);
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

        IPreParsingState HandleEndOfArgument()
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
            if (!CliCommandDefinition.CanBeCommand(argument))
            {
                return null;
            }

            IReadOnlyList<CliCommandDefinition> commands = definition.GetRegisteredCommands();
            return commands.FirstOrDefault(c => c.IsMatch(argument));
        }

        bool HasDefaultCommand => definition.DefaultCommand != null;
    }
}