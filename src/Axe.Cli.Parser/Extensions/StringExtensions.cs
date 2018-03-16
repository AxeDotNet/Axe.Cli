using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;

namespace Axe.Cli.Parser.Extensions
{
    static class StringExtensions
    {
        public static IList<string> AutoWrapText([NotNull]this string text, int maxColumn)
        {
            if (maxColumn <= 1) { throw new ArgumentOutOfRangeException(nameof(maxColumn)); }

            string trimmedText = text.Trim();
            if (trimmedText.Length < maxColumn) { return new[] {trimmedText}; }

            using (var reader = new StringReader(trimmedText))
            {
                return new TextWrapper(reader, maxColumn).Wrap().ToArray();
            }
        }
    }
}