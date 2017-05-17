using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Ofl.Linq.Tests
{
    public partial class EnumerableExtensionsTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        public void Test_ZipChecked_SequencesEqualLegth(int length)
        {
            // Produce the sequences.
            IEnumerable<Guid> sequenceOne = Enumerable.Range(0, length).Select(i => Guid.NewGuid());
            IEnumerable<Guid> sequenceTwo = Enumerable.Range(0, length).Select(i => Guid.NewGuid());

            // Zip, iterate.
            // No exception.
            sequenceOne.ZipChecked(sequenceTwo, (f, s) => new { f, s }).ToList();
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        [InlineData(2, 0)]
        [InlineData(0, 2)]
        [InlineData(1, 2)]
        [InlineData(2, 1)]
        public void Test_ZipChecked_SequencesNotEqualLegth(int firstLength, int secondLength)
        {
            // Make sure first and second length are not the same.
            if (firstLength == secondLength) throw new InvalidOperationException($"The {nameof(firstLength)} and {nameof(secondLength)} parameters must not be equal to each other (value: { firstLength}).");

            // Produce the sequences.
            IEnumerable<Guid> sequenceOne = Enumerable.Range(0, firstLength).Select(i => Guid.NewGuid());
            IEnumerable<Guid> sequenceTwo = Enumerable.Range(0, secondLength).Select(i => Guid.NewGuid());

            // Zip, iterate.
            // Exception.
            Assert.Throws<InvalidOperationException>(() => sequenceOne.ZipChecked(sequenceTwo, (f, s) => new { f, s }).ToList());
        }
    }
}
