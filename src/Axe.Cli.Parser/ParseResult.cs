using System;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Cli.Parser
{
    class ParseResult : IParseResult
    {
        public ParseResult(string errorMessage)
        {
            InitializeEmpty(false, errorMessage);
        }

        public ParseResult(IList<IntemediateResult> results)
        {
            if (results.Count == 0)
            {
                InitializeEmpty(true);
                return;
            }

            string possibleArea = GetPossibleArea(results);

            IsSuccess = true;
            HasPossibleArea = !string.IsNullOrEmpty(possibleArea);
            PossibleArea = possibleArea;
            Pairs = GetPairs(results);
            Flags = GetFlags(results);
            FreeValues = GetFreeValues(results);
            ErrorMessage = string.Empty;
        }

        void InitializeEmpty(bool isSuccess, string errorMessage = null)
        {
            IsSuccess = isSuccess;
            HasPossibleArea = false;
            PossibleArea = string.Empty;
            Pairs = Array.Empty<KeyValuePair<string, string>>();
            FreeValues = Array.Empty<string>();
            Flags = Array.Empty<string>();
            ErrorMessage = isSuccess ? string.Empty : (errorMessage ?? "Unknown error. ");
        }

        static IList<string> GetFreeValues(IList<IntemediateResult> results)
        {
            return results
                .Where(r => r.FreeValue != null)
                .Select(r => r.FreeValue)
                .ToArray();
        }

        static IList<string> GetFlags(IList<IntemediateResult> results)
        {
            return results
                .Where(r => r.Flag != null)
                .Select(r => r.Flag)
                .ToArray();
        }

        static IList<KeyValuePair<string, string>> GetPairs(IList<IntemediateResult> results)
        {
            return results
                .Where(r => r.Pair.HasValue)
                // ReSharper disable once PossibleInvalidOperationException
                .Select(r => r.Pair.Value)
                .ToArray();
        }

        static string GetPossibleArea(IList<IntemediateResult> results)
        {
            return results.FirstOrDefault(r => r.PossbileArea != null)?.PossbileArea ?? string.Empty;
        }

        public bool IsSuccess { get; private set; }
        public bool HasPossibleArea { get; private set; }
        public string PossibleArea { get; private set; }
        public IList<KeyValuePair<string, string>> Pairs { get; private set; }
        public IList<string> Flags { get; private set; }
        public IList<string> FreeValues { get; private set; }
        public string ErrorMessage { get; private set; }
    }
}