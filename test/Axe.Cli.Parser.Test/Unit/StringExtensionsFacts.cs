using System;
using System.Collections.Generic;
using Axe.Cli.Parser.Extensions;
using Xunit;

namespace Axe.Cli.Parser.Test.Unit
{
    public class StringExtensionsFacts
    {
        [Fact]
        public void should_throw_if_max_column_is_le_zero()
        {
            const string original = "some text";

            Assert.Throws<ArgumentOutOfRangeException>(() => original.AutoWrapText(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => original.AutoWrapText(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => original.AutoWrapText(1));
        }

        [Fact]
        public void should_handle_empty_string()
        {
            const string empty = "";
            Assert.Equal(new [] {string.Empty}, empty.AutoWrapText(2));
        }

        [Fact]
        public void should_handle_string_wrapping_for_a_single_line_origin_string()
        {
            const string oneLine = "This is a single line string. With a 20 chars length.";

            IEnumerable<string> autoWrapText = oneLine.AutoWrapText(20);

            Assert.Equal(new[]
            {
                "This is a single li-",
                "ne string. With a 20",
                "chars length."
            }, autoWrapText);
        }
        
        [Fact]
        public void should_handle_string_wrapping_for_a_single_line_origin_string_with_leading_or_trailing_spaces()
        {
            const string oneLine = "        This is a single line string. With a 20 chars length.   ";

            IEnumerable<string> autoWrapText = oneLine.AutoWrapText(20);

            Assert.Equal(new[]
            {
                "This is a single li-",
                "ne string. With a 20",
                "chars length."
            }, autoWrapText);
        }

        [Fact]
        public void should_avoid_leading_spaces_when_wrapping_lines()
        {
            const string oneLine = "This is a single line string. With a 20                chars length.";

            IEnumerable<string> autoWrapText = oneLine.AutoWrapText(20);

            Assert.Equal(new[]
            {
                "This is a single li-",
                "ne string. With a 20",
                "chars length."
            }, autoWrapText);
        }
        
        [Fact]
        public void should_support_multi_line_wrapping()
        {
            const string multiLine =
                "This is a single line string. With a 20 chars length.\r\n" +
                "This is a single line string. With a 20 chars length.";
            
            Assert.Equal(new []
            {
                "This is a single li-",
                "ne string. With a 20",
                "chars length.",
                "This is a single li-",
                "ne string. With a 20",
                "chars length."
            }, multiLine.AutoWrapText(20));
        }

        [Fact]
        public void should_apply_trimming_logic_to_each_line()
        {
            const string multiLine =
                "       This is a single line string. With a 20 chars length.                 \r\n" +
                "    This is a single line string. With a 20 chars length.           ";
            
            Assert.Equal(new []
            {
                "This is a single li-",
                "ne string. With a 20",
                "chars length.",
                "This is a single li-",
                "ne string. With a 20",
                "chars length."
            }, multiLine.AutoWrapText(20));
        }
    }
}