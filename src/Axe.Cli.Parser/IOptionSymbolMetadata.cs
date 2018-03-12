namespace Axe.Cli.Parser
{
    /// <summary>
    /// Represent an symbol metadata containing the full form and the abbreviation.
    /// </summary>
    public interface IOptionSymbolMetadata
    {
        /// <summary>
        /// Get the abbreviation form of the symbol.
        /// </summary>
        char? Abbreviation { get; }
        
        /// <summary>
        /// Get the full form of the symbol.
        /// </summary>
        string FullForm { get; }
        
        /// <summary>
        /// Get a meaningful string representing current symbol. This is mainly for diagnostics
        /// purpose.
        /// </summary>
        /// <returns>A string representing current instance.</returns>
        string ToString();
    }
}