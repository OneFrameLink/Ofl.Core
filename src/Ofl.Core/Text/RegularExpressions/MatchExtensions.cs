using System;
using System.Text.RegularExpressions;

namespace Ofl.Core.Text.RegularExpressions
{
    public static class MatchExtensions
    {
        public static string GetGroupValue(this Match match, string group)
        {
            // Validate parameters.
            if (match == null) throw new ArgumentNullException(nameof(match));
            if (string.IsNullOrWhiteSpace(group)) throw new ArgumentNullException(nameof(group));

            // Get the group.
            Group g = match.Groups[group];

            // If no success, throw.
            if (!match.Success || !g.Success) throw new InvalidOperationException($"The group ({ group }) was not successfully matched in the match.");

            // Return the value.
            return g.Value;
        }
    }
}
