using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Axe.Cli.Parser.Test.Helpers
{
    static class ParsingResultTestExtensions
    {
        [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
        public static void AssertError(
            this ArgsParsingResult result,
            ArgsParsingErrorCode code,
            string trigger)
        {
            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
            Assert.Equal(code, result.Error.Code);
            Assert.Equal(trigger, result.Error.Trigger);
        }
    }
}