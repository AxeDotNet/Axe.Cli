namespace Axe.Cli.Parser
{
    interface ICliOptionToken
    {
        ICliOptionDefinition Definition { get; set; }
        object Value { get; }
    }
}