using System;
using System.Collections.Generic;

namespace Axe.Cli.Parser.Extensions
{
    static class StringExtensions
    {
        public static IList<string> AutoWrapText(this string text, int maxColumn)
        {
            if (maxColumn <= 1) { throw new ArgumentOutOfRangeException(nameof(maxColumn)); }
            return new TextWrapper().Wrap(text, maxColumn);
        }
    }
}