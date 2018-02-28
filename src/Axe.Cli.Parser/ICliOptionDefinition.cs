namespace Axe.Cli.Parser
{
    public interface ICliOptionDefinition
    {
        ICliOptionSymbol Symbol { get; }
        string Description { get; }
        bool IsRequired { get; }
        OptionType Type { get; }
        bool IsConflict(ICliOptionDefinition optionDefinition);
        string ToString();
        bool IsMatch(string argument);
    }
}