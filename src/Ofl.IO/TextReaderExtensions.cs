using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ofl.IO
{
    public static class TextReaderExtensions
    {
        public static IEnumerable<string> GetEnumerable(this TextReader textReader)
        {
            // Validate parameters.
            if (textReader == null) throw new ArgumentNullException(nameof(textReader));

            // Call the implementation.
            return textReader.GetEnumerableImplementation();
        }

        private static IEnumerable<string> GetEnumerableImplementation(this TextReader textReader)
        {
            // Validate parameters.
            if (textReader == null) throw new ArgumentNullException(nameof(textReader));

            // The line.
            string line;

            // Cycle through readline.  While there's a value, yield.
            while ((line = textReader.ReadLine()) != null)
                yield return line;
        }

        public static IAsyncEnumerable<string> GetAsyncEnumerable(this TextReader reader)
        {
            // Validate parameters.
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            // The line.
            string line = null;

            // Create the enumerator.
            return AsyncEnumerable.CreateEnumerable(() =>
                AsyncEnumerable.CreateEnumerator(async ct => {
                    // Read the line.
                    line = await reader.ReadLineAsync().ConfigureAwait(false);

                    // If the line is null, stop reading.
                    return line != null;
                }, () => line, () => { }));
        }
    }
}
