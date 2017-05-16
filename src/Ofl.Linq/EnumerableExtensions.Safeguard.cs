using System.Collections.Generic;
using System.Linq;

namespace Ofl.Linq
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<T> Safeguard<T>(this IEnumerable<T> source) =>
            source ?? Enumerable.Empty<T>();
    }
}
