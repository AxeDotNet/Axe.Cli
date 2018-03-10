using Axe.Cli.Parser.Transformers;

namespace Axe.Cli.Parser
{
    public static class ArgsTransformers
    {
        public static ValueTransformer IntegerTransformer { get; } = new IntegerTransformer();
        public static ValueTransformer Default { get; } = new DefaultTransformer();
    }
}