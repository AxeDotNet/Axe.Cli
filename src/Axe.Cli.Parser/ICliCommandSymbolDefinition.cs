namespace Axe.Cli.Parser
{
    public interface ICliCommandSymbolDefinition
    {
        string Symbol { get; }
        string Description { get; }
        bool IsConflict(ICliCommandSymbolDefinition commandDefinition);
        string ToString();
        bool IsMatch(string argument);
    }
}