using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Axe.Cli.Parser.Test.Helpers
{
    static class ParsingResultExtensions
    {
        [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
        public static void AssertError(
            this CliArgsPreParsingResult result,
            CliArgsParsingErrorCode code,
            string trigger)
        {
            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
            Assert.Equal(code, result.Error.Code);
            Assert.Equal(trigger, result.Error.Trigger);
        }
    }
}