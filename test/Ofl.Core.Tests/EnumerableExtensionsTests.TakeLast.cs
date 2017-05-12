using System;
using System.Collections.Generic;
using System.Linq;
using Ofl.Core.Linq;
using Xunit;

namespace Ofl.Core.Tests
{
    public partial class EnumerableExtensionsTests
    {
        #region Helpers.

        private static void AssertTakeLast<T>(IEnumerable<T> sequence, int itemsToTake, int expectedTakeLastCount)
        {
            // The harvester.
            IList<T> harvester = new List<T>();

            // Harvest each item while sequencing, then call take last on top of that.
            IReadOnlyCollection<T> lastItems = sequence.
                // Harvest.
                Harvest(harvester).
                // Take the last items.
                TakeLast(itemsToTake).
                // Materialize.
                ToReadOnlyCollection();

            // Compare the sequences.
            Assert.Equal(expectedTakeLastCount, lastItems.Count);
            Assert.True(lastItems.SequenceEqual(harvester.Skip(Math.Max(harvester.Count - itemsToTake, 0))));
        }

        private static IEnumerable<int> CreateSequence(int sequenceLength)
        {
            // Loop, yield.
            for (int index = 0; index < sequenceLength; ++index) yield return index;
        }

        #endregion

        #region Tests.

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0, 1, 0)]
        [InlineData(1, 0, 0)]
        [InlineData(1, 2, 1)]
        [InlineData(1, 1, 1)]
        [InlineData(2, 0, 0)]
        [InlineData(2, 3, 2)]
        [InlineData(2, 1, 1)]
        [InlineData(10, 0, 0)]
        [InlineData(10, 50, 10)]
        [InlineData(10, 5, 5)]
        public void Test_TakeLast_Collection(int sequenceLength, int itemsToTake, int expectedTakeLastCount)
        {
            // Create the sequence.
            var sequence = Enumerable.Range(0, sequenceLength).ToList();

            // Assert the call.
            AssertTakeLast(sequence, itemsToTake, expectedTakeLastCount);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0, 1, 0)]
        [InlineData(1, 0, 0)]
        [InlineData(1, 2, 1)]
        [InlineData(1, 1, 1)]
        [InlineData(2, 0, 0)]
        [InlineData(2, 3, 2)]
        [InlineData(2, 1, 1)]
        [InlineData(10, 0, 0)]
        [InlineData(10, 50, 10)]
        [InlineData(10, 5, 5)]
        public void Test_TakeLast_ReadOnlyCollection(int sequenceLength, int itemsToTake, int expectedTakeLastCount)
        {
            // Create the sequence.
            var sequence = Enumerable.Range(0, sequenceLength).ToReadOnlyCollection();

            // Assert the call.
            AssertTakeLast(sequence, itemsToTake, expectedTakeLastCount);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0, 1, 0)]
        [InlineData(1, 0, 0)]
        [InlineData(1, 2, 1)]
        [InlineData(1, 1, 1)]
        [InlineData(2, 0, 0)]
        [InlineData(2, 3, 2)]
        [InlineData(2, 1, 1)]
        [InlineData(10, 0, 0)]
        [InlineData(10, 50, 10)]
        [InlineData(10, 5, 5)]
        public void Test_TakeLast_Enumerable(int sequenceLength, int itemsToTake, int expectedTakeLastCount)
        {
            // Create the sequence.
            var sequence = CreateSequence(sequenceLength);

            // Assert the call.
            AssertTakeLast(sequence, itemsToTake, expectedTakeLastCount);
        }

        #endregion
    }
}
