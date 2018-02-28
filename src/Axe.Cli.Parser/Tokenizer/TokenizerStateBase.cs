using System.Linq;

namespace Axe.Cli.Parser.Tokenizer
{
    abstract class TokenizerStateBase : ITokenizerState
    {
        protected static bool IsEndOfArguments(string argument)
        {
            return argument == null;
        }

        protected static ICliOptionDefinition ResolveKeyValueOptionLabel(
            ICliCommandDefinition defaultCommand,
            string argument)
        {
            return defaultCommand.GetRegisteredOptions()
                .FirstOrDefault(o => o.Type == OptionType.KeyValue && o.IsMatch(argument));
        }

        public abstract ITokenizerState MoveToNext(string argument);
    }
}