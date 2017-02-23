namespace HashCode
{
    public class Cache
    {
        public int CacheId { get; set; }
        public int Latency { get; set; }

        public override string ToString()
        {
            return string.Format("[{0}] {1}ms", CacheId, Latency);
        }
    }
}
