using System.Collections.Generic;

namespace Axe.Cli.Parser
{
    class IntemediateResult
    {
        public static IntemediateResult CreateFreeValue(string freeValue) 
            => new IntemediateResult(null, freeValue, null, null);

        public static IntemediateResult CreateFlag(string label)
            => new IntemediateResult(null, null, label, null);

        public static IntemediateResult CreatePair(string label, string value)
            => new IntemediateResult(null, null, null, new KeyValuePair<string, string>(label, value));

        public static IntemediateResult CreatePossibleArea(string value)
            => new IntemediateResult(value, null, null, null);

        IntemediateResult(
            string possbileArea,
            string freeValue, 
            string flag, 
            KeyValuePair<string, string>? pair)
        {
            FreeValue = freeValue;
            Flag = flag;
            Pair = pair;
            PossbileArea = possbileArea;
        }

        public string PossbileArea { get; }
        public string FreeValue { get; }
        public string Flag { get; }
        public KeyValuePair<string, string>? Pair { get; }
    }
}