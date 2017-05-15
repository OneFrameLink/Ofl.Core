using System;
using AngleSharp;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;

namespace Ofl.Html
{
    public static class StringExtensions
    {
        public static IHtmlDocument ToHtmlDocument(this string str)
        {
            // Call the overload with the default.
            return str.ToHtmlDocument(Configuration.Default);
        }

        public static IHtmlDocument ToHtmlDocument(this string str, IConfiguration configuration)
        {
            // Validate parameters.
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            // Create a parser.
            var parser = new HtmlParser(configuration);

            // Parse.
            return parser.Parse(str);
        }
    }
}
