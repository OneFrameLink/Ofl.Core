using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ofl.Core.IO
{
    public static class TextReaderExtensions
    {
        public static IAsyncEnumerable<string> ReadAllLines(this TextReader reader)
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
