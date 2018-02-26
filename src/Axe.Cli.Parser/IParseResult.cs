using System.Collections.Generic;

namespace Axe.Cli.Parser
{
    public interface IParseResult
    {
        bool IsSuccess { get; }
        string PossibleArea { get; }
        IList<KeyValuePair<string, string>> Pairs { get; }
        IList<string> Flags { get; }
        IList<string> FreeValues { get; }
        string ErrorMessage { get; }
        bool HasPossibleArea { get; }
    }
}