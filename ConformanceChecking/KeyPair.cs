namespace pgmpm.ConformanceChecking
{
    public class KeyPair<TKey1, TKey2>
    {
        public KeyPair(TKey1 key1, TKey2 key2)
        {
            Key1 = key1;
            Key2 = key2;
        }

        public TKey1 Key1 { get; set; }
        public TKey2 Key2 { get; set; }

        public override int GetHashCode()
        {
            return Key1.GetHashCode() ^ Key2.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            KeyPair<TKey1, TKey2> pair = obj as KeyPair<TKey1, TKey2>;
            if (pair == null)
                return false;
            return Key1.Equals(pair.Key1) && Key2.Equals(pair.Key2);
        }
    }
}