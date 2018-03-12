using System;
using System.Linq;
using Axe.Cli.Parser.Test.Helpers;
using Xunit;

namespace Axe.Cli.Parser.Test.End2End
{
    public class WhenGetOptionMetadataFromCommandInformation
    {
        [Fact]
        public void should_get_option_basic_metadata()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .AddFlagOption("flag", 'f', "flag description")
                .AddOptionWithValue("key-value", 'k', "key value description")
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new [] {"command", "--flag", "-k", "value"});

            result.AssertSuccess();

            IOptionDefinitionMetadata[] optionDefinitionMetadatas =
                result.Command.GetRegisteredOptionsMetadata().ToArray();
            IOptionDefinitionMetadata flagMetadata = optionDefinitionMetadatas
                .Single(d => d.SymbolMetadata.FullForm.Equals("flag", StringComparison.OrdinalIgnoreCase));
            IOptionDefinitionMetadata kvMetadata = optionDefinitionMetadatas
                .Single(d => d.SymbolMetadata.FullForm.Equals("key-value", StringComparison.OrdinalIgnoreCase));

            Assert.Equal("flag", flagMetadata.SymbolMetadata.FullForm);
            Assert.Equal('f', flagMetadata.SymbolMetadata.Abbreviation);
            Assert.Equal("flag description", flagMetadata.Description);
            Assert.Equal(OptionType.Flag, flagMetadata.Type);
            Assert.False(flagMetadata.IsRequired);

            Assert.Equal("key-value", kvMetadata.SymbolMetadata.FullForm);
            Assert.Equal('k', kvMetadata.SymbolMetadata.Abbreviation);
            Assert.Equal("key value description", kvMetadata.Description);
            Assert.Equal(OptionType.KeyValue, kvMetadata.Type);
            Assert.False(kvMetadata.IsRequired);
        }
    }
}