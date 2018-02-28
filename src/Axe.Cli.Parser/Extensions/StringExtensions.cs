using System;
using System.Linq;
using System.Text;

namespace Axe.Cli.Parser.Extensions
{
    static class StringExtensions
    {
        public static string MakeSingleLine(this string description)
        {
            if (description == null) { return String.Empty; }
            return description.Aggregate(
                new StringBuilder(),
                (builder, c) =>
                {
                    if (c == '\r') { return builder; }
                    if (c == '\n') { return builder.Append(' '); }
                    return builder.Append(c);
                }).ToString();
        }
    }
}