namespace Axe.Cli.Parser
{
    interface IParsingState
    {
        IParsingState HandleInput(ParseResultBuilder builder, string input);
    }
}