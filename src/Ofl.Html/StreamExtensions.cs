using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;

namespace Ofl.Html
{
    public static class StreamExtensions
    {
        public static Task<IHtmlDocument> ToHtmlDocumentAsync(this Stream stream, CancellationToken cancellationToken)
        {
            // Call the overload with the default.
            return stream.ToHtmlDocumentAsync(Configuration.Default, cancellationToken);
        }

        public static Task<IHtmlDocument> ToHtmlDocumentAsync(this Stream stream, IConfiguration configuration, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            // Create a parser.
            var parser = new HtmlParser(configuration);

            // Parse.
            return parser.ParseAsync(stream, cancellationToken);
        }
    }
}
