using System.Collections.Generic;

namespace HashCode
{
    public class Endpoint
    {
        public int EndpointId { get; set; }
        public int Latency { get; set; }
        public int CachesCount { get; set; }
        public IList<Cache> Caches { get; set; }

        public Endpoint()
        {
            this.Caches = new List<Cache>();
        }
    }
}
