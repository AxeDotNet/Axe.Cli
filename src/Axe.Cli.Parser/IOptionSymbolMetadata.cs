namespace Axe.Cli.Parser
{
    public interface IOptionSymbolMetadata
    {
        char? Abbreviation { get; }
        string Symbol { get; }
        string ToString();
    }
}