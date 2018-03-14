namespace Axe.Cli.Parser.Transformers
{
    /// <summary>
    /// The default transformer. This transformer actually does nothing on the argument.
    /// </summary>
    public class DefaultTransformer : SingleValueTransformer
    {
        /// <summary>
        /// This method actually does nothing on the argument. But return the argument directly.
        /// </summary>
        /// <param name="argument">The argument input by user.</param>
        /// <returns>The argument input by user.</returns>
        protected override object TransformSingleArgument(string argument)
        {
            return argument;
        }
        
        /// <summary>
        /// The instance of <see cref="DefaultTransformer"/>. This transformer is thread-safe so
        /// it is fine to use the singleton method.
        /// </summary>
        public static ValueTransformer Instance { get; } = new DefaultTransformer();
    }
}