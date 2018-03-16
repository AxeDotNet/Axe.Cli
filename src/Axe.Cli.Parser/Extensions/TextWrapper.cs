using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Axe.Cli.Parser.Extensions
{
    class TextWrapper
    {
        readonly StringReader reader;
        readonly char[] buffer;

        public TextWrapper(StringReader reader, int maxColumn)
        {
            this.reader = reader;
            buffer = new char[maxColumn - 1];
        }

        public IEnumerable<string> Wrap()
        {
            while (true)
            {
                string line = GetNextLine();
                if (line == null) { yield break; }

                yield return line;
            }
        }

        string GetNextLine()
        {
            int numberOfCharRead = reader.Read(buffer, 0, buffer.Length);
            if (numberOfCharRead == 0) return null;
            StringBuilder builder = new StringBuilder(numberOfCharRead).Append(buffer, 0, numberOfCharRead);
            if (numberOfCharRead < buffer.Length) { return builder.ToString(); }
            int endOfLineCode = reader.Peek();
            if (endOfLineCode == -1) { return builder.ToString(); }

            var endOfLineChar = unchecked((char) endOfLineCode);
            if (char.IsWhiteSpace(endOfLineChar))
            {
                reader.Read();
                return builder.ToString();
            }
            
            if (char.IsLetter(endOfLineChar)) { return builder.Append('-').ToString(); }

            reader.Read();
            builder.Append(endOfLineChar);
            return builder.ToString();
        }
    }
}