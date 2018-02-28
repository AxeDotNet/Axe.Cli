namespace Axe.Cli.Parser
{
    public interface ICliOptionSymbol
    {
        char? Abbreviation { get; }
        string Symbol { get; }

        bool IsConflict(ICliOptionSymbol other);
        string ToString();
        bool IsMatch(string argument);
    }
}