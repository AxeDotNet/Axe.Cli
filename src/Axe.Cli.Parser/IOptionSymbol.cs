namespace Axe.Cli.Parser
{
    interface IOptionSymbol
    {
        char? Abbreviation { get; }
        string Symbol { get; }

        bool IsConflict(IOptionSymbol other);
        bool IsMatch(string argument);
        string ToString();
    }
}