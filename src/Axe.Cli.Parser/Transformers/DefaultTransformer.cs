namespace Axe.Cli.Parser.Transformers
{
    class DefaultTransformer : SingleValueTransformer
    {
        protected override object TransformSingleArgument(string argument)
        {
            return argument;
        }
    }
}