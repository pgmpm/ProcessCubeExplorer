using System.Collections.Generic;

namespace pgmpm.ConformanceChecking
{
    public class KeyPairDictonary<TKey1, TKey2, TValue> : Dictionary<KeyPair<TKey1, TKey2>, TValue>
    {
        public TValue this[TKey1 key1, TKey2 key2]
        {
            get
            {
                return this[new KeyPair<TKey1, TKey2>(key1, key2)];
            }
            set
            {
                this[new KeyPair<TKey1, TKey2>(key1, key2)] = value;
            }
        }
    }
}