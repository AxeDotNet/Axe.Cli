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

            ICliCommandDefinition selectedCommand;
            
            selectedCommand = ResolveCommand(argument);
            if (selectedCommand != null)
            {
                resultBuilder.SetCommand(selectedCommand);
                return new ContinueWithCommandState(selectedCommand, resultBuilder);
            }

            selectedCommand = EnsureDefaultCommandSet(argument);

            ICliOptionDefinition kvOption = ResolveKeyValueOptionLabel(
                selectedCommand,
                argument);
            if (kvOption != null)
            {
                return new WaitingValueWithCommandState(selectedCommand, kvOption, argument, resultBuilder);
            }

            IList<ICliOptionDefinition> flagOptions = ResolveFlagOptionLabels(
                selectedCommand,
                argument);
            if (flagOptions.Count > 0)
            {
                foreach (ICliOptionDefinition flagOption in flagOptions)
                {
                    resultBuilder.AppendOptionToken(new CliOptionToken(flagOption, true));
                }
                return new ContinueWithCommandState(selectedCommand, resultBuilder);
            }

            throw new CliArgParsingException(
                CliArgsParsingErrorCode.FreeValueNotSupported,
                argument);
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