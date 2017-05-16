using System.Collections.ObjectModel;

namespace Ofl.Collections.Generic
{
    public static partial class ReadOnlyCollectionExtensions
    {
        public static ReadOnlyCollection<T> EmptyReadOnlyCollection<T>() => 
            EmptyReadOnlyCollectionSingleton<T>.Instance;

        private static class EmptyReadOnlyCollectionSingleton<T>
        {
            internal static readonly ReadOnlyCollection<T> Instance = new ReadOnlyCollection<T>(new T[0]);
        }
    }
}
