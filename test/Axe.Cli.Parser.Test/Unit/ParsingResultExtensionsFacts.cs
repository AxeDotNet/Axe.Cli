using System;
using System.Collections.Generic;
using Xunit;

namespace Axe.Cli.Parser.Test.Unit
{
    public class ParsingResultExtensionsFacts
    {
        [Fact]
        public void should_throw_if_result_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => ((ArgsParsingResult) null).GetOptionValue<object>("-i"));
            Assert.Throws<ArgumentNullException>(() => ((ArgsParsingResult) null).GetFirstOptionValue<object>("-i"));
            Assert.Throws<ArgumentNullException>(() => ((ArgsParsingResult) null).GetFreeValue<object>("-i"));
            Assert.Throws<ArgumentNullException>(() => ((ArgsParsingResult) null).GetFirstFreeValue<object>("-i"));
        }

        [Fact]
        public void should_throw_if_option_is_null()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new [] {"command"});

            Assert.Throws<ArgumentNullException>(() => result.GetOptionValue<object>(null));
            Assert.Throws<ArgumentNullException>(() => result.GetFirstOptionValue<object>(null));
            Assert.Throws<ArgumentNullException>(() => result.GetFreeValue<object>(null));
            Assert.Throws<ArgumentNullException>(() => result.GetFirstFreeValue<object>(null));
        }

        [Fact]
        public void should_throw_if_option_is_not_defined()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new [] {"command"});

            Assert.Throws<ArgumentException>(() => result.GetOptionValue<string>("--not_existed_option"));
            Assert.Throws<ArgumentException>(() => result.GetFirstOptionValue<string>("--not_existed_option"));
            Assert.Throws<ArgumentException>(() => result.GetFreeValue<string>("not_existed_free_value"));
            Assert.Throws<ArgumentException>(() => result.GetFirstFreeValue<string>("not_existed_free_value"));
        }

        [Fact]
        public void should_throw_if_result_is_not_a_successful_one()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .AddFreeValue("name", string.Empty)
                .AddOptionWithValue("option", 'o', string.Empty, true)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new [] {"command", "-f"});

            Assert.False(result.IsSuccess);
            Assert.Throws<InvalidOperationException>(() => result.GetOptionValue<bool>("-o"));
            Assert.Throws<InvalidOperationException>(() => result.GetFirstOptionValue<bool>("-o"));
            Assert.Throws<InvalidOperationException>(() => result.GetFreeValue<bool>("name"));
            Assert.Throws<InvalidOperationException>(() => result.GetFirstFreeValue<bool>("name"));
        }

        [Fact]
        public void should_throw_if_option_has_no_value()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .AddOptionWithValue("option", 'o', string.Empty)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new [] {"command"});

            IList<string> values = result.GetOptionValue<string>("--option");
            Assert.Empty(values);
            Assert.Throws<InvalidOperationException>(() => result.GetFirstOptionValue<string>("--option"));
        }

        [Fact]
        public void should_throw_if_free_value_name_has_no_value()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginCommand("command", string.Empty)
                .AddFreeValue("name", string.Empty)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new [] {"command"});

            IList<string> values = result.GetFreeValue<string>("name");
            Assert.Empty(values);
            Assert.Throws<InvalidOperationException>(() => result.GetFirstFreeValue<string>("name"));
        }
    }
}