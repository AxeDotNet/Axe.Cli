using System.Collections.Generic;
using System.Diagnostics;

namespace Axe.Cli.Parser.Tokenizer
{
    class CliArgsTokenizer
    {
        readonly CliArgsDefinition definition;

        public CliArgsTokenizer(CliArgsDefinition definition)
        {
            Debug.Assert(definition != null);

            this.definition = definition;
        }

        public CliArgsParsingResult Tokenize(IList<string> args)
        {
            /*
             * (start) --[EoA]--> (no default? error|stop)
             * (start) --[command]--> (save command|continue with command)
             * (start) --[option(kv) in default command]--> (save default command|waiting value with command)
             * (start) --[option-groups(f) in default command]--> (save default command|yield flag|continue with command)
             * (start) --[other]--> (save default command|yield free value|continue with command-free value)
             * 
             * (continue with command) --[option(kv) in command]--> (waiting value with command)
             * (continue with command) --[option-groups(f) in command]--> (yield flag|continue with command)
             * (continue with command) --[unresolved option in command]--> (error|stop)
             * (continue with command) --[EoA]--> (stop)
             * (continue with command) --[other]--> (yield free value|continue with command-free value)
             * 
             * (waiting value with command) --[EoA]--> (error|stop)
             * (waiting value with command) --[other]--> (yield key value|continue with command)
             * 
             * (continue with command-free value) --[EoA]--> (stop)
             * (continue with command-free value) --[other] --> (yield free value|continue with command-free value)
             */

            var builder = new TokenizedResultBuilder();
            ITokenizerState state = new StartState(definition, builder);
            foreach (string arg in args)
            {
                state = state.MoveToNext(arg);
                if (state == null) { return builder.Build(); }
            }

            state.MoveToNext(null);
            return builder.Build();
        }
    }
}