namespace Axe.Cli.Parser
{
    interface IOptionSymbol : IOptionSymbolMetadata
    {
        bool IsConflict(IOptionSymbol other);
        bool IsMatch(string argument);
    }
}