using System.Collections.Generic;
using Ofl.Core.Collections.Generic;
using Xunit;

namespace Ofl.Core.Tests.Collections.Generic
{
    public class ReadOnlyListExtensionsTests
    {
        #region Tests

        [Theory]
        // Found.
        // At beginning.
        [InlineData(0, 0, 0, 10, 20, 30, 40, 50)]
        // At end.
        [InlineData(50, 5, 0, 10, 20, 30, 40, 50)]
        // In middle.
        [InlineData(30, 3, 0, 10, 20, 30, 40, 50)]

        // Not found.
        // At beginning.
        [InlineData(-1, ~0, 0, 10, 20, 30, 40, 50)]
        // At end.
        [InlineData(100, ~6, 0, 10, 20, 30, 40, 50)]
        // In middle.
        [InlineData(25, ~3, 0, 10, 20, 30, 40, 50)]
        public void Test_BinarySearch(int searchFor, int expectedResult, params int[] items)
        {
            // Cast items to IReadOnlyList<T>.
            IReadOnlyList<int> list = items;

            // Search.
            int result = list.BinarySearch(searchFor);

            // Compare the result.
            Assert.Equal(expectedResult, result);
        }

        #endregion
    }
}
