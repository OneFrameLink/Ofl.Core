using System;
using System.Collections;
using System.Collections.Generic;
using Ofl.Core.Linq;

namespace Ofl.Core
{
    public class SlidingWindowMask<T>
    {
        #region Constructor

        public SlidingWindowMask(ICollection<T> mask) : this(mask, EqualityComparer<T>.Default)
        { }

        public SlidingWindowMask(ICollection<T> mask, IEqualityComparer<T> equalityComparer)
        {
            // Validate parameters.
            if (mask == null) throw new ArgumentNullException(nameof(mask));
            if (equalityComparer == null) throw new ArgumentNullException(nameof(equalityComparer));

            // Assign values.
            _mask = mask.ToReadOnlyCollection();
            _windows = new BitArray(_mask.Count);
            _equalityComparer = equalityComparer;
        }

        #endregion

        #region Instance, read-only state.

        private readonly IEqualityComparer<T> _equalityComparer;

        private readonly IReadOnlyList<T> _mask;

        private readonly BitArray _windows;

        #endregion

        public bool Slide(T item)
        {
            // The previous flag and current flag.
            bool previous = true;

            // Cycle through the windows.
            for (int index = 0; index < _mask.Count; ++index)
            {
                // Set current.
                bool current = _windows[index];

                // Set the window.
                _windows[index] = previous & _equalityComparer.Equals(item, _mask[index]);

                // Set the previous flag to current.
                previous = current;
            }

            // Return if the window is masked or not.
            return Masked;
        }

        public bool Masked => _windows[_windows.Length - 1];
    }
}
