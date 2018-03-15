namespace Axe.Cli.Parser
{
    class TransformedValue<TRaw, TTransform>
    {
        protected TransformedValue(TRaw raw, TTransform transformed)
        {
            Raw = raw;
            Transformed = transformed;
        }

        public TRaw Raw { get; }
        public TTransform Transformed { get; }
    }
}