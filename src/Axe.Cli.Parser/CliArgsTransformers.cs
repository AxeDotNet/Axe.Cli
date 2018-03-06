using Axe.Cli.Parser.Transformers;

namespace Axe.Cli.Parser
{
    public static class CliArgsTransformers
    {
        public static IValueTransformer IntegerTransformer { get; } = new IntegerTransformer();
        public static IValueTransformer Default { get; } = new DefaultTransformer();
    }
}