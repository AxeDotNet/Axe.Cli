namespace Axe.Cli.Parser
{
    class TransformedValue<TRaw, TTransform>
    {
        public TransformedValue(TRaw raw, TTransform transformed)
        {
            Raw = raw;
            Transformed = transformed;
        }

        public TRaw Raw { get; }
        public TTransform Transformed { get; }
    }
}