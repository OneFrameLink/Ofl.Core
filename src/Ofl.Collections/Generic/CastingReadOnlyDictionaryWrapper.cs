using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ofl.Collections.Generic
{
    internal class CastingReadOnlyDictionaryWrapper<TFromKey, TFromValue, TToKey, TToValue> : 
        IReadOnlyDictionary<TToKey, TToValue>
        where TFromKey : TToKey
        where TFromValue : TToValue
    {
        #region Constructor.

        internal CastingReadOnlyDictionaryWrapper(IReadOnlyDictionary<TFromKey, TFromValue> source)
        {
            // Validate parameters.
            _source = source ?? throw new ArgumentNullException(nameof(source));
        }

        #endregion

        #region Instance, read-only state.

        private readonly IReadOnlyDictionary<TFromKey, TFromValue> _source;

        #endregion

        public IEnumerator<KeyValuePair<TToKey, TToValue>> GetEnumerator()
        {
            // Retrun a projection.
            return _source.Select(p => new KeyValuePair<TToKey, TToValue>(p.Key, p.Value)).
                GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => _source.Count;

        public bool ContainsKey(TToKey key) => _source.ContainsKey((TFromKey) key);

        public bool TryGetValue(TToKey key, out TToValue value)
        {
            // The output type.
            TFromValue typedValue;

            // Try and get the value;
            bool result = _source.TryGetValue((TFromKey) key, out typedValue);

            // Set the value.
            value = typedValue;

            // Return the result.
            return result;
        }

        public TToValue this[TToKey key] => _source[(TFromKey) key];

        public IEnumerable<TToKey> Keys => _source.Keys.Cast<TToKey>();

        public IEnumerable<TToValue> Values => _source.Keys.Cast<TToValue>();
    }
}
