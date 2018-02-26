namespace Axe.Cli.Parser
{
    public class ParsingError
    {
        public ParsingError(string errorPart, ParsingErrorCode code)
        {
            ErrorPart = errorPart;
            Code = code;
            Description = code.GetDescription();
        }

        public string ErrorPart { get; }
        public ParsingErrorCode Code { get; }
        public string Description { get; }
    }
}