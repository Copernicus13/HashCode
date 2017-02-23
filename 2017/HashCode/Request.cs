namespace HashCode
{
    public class Request
    {
        public int RequestId { get; set; }
        public int RequestsCount { get; set; }
        public Video Video { get; set; }
        public Endpoint Endpoint { get; set; }

        public override string ToString()
        {
            return string.Format("[{0}] {1} (V:{2} ; E:{3})", RequestId, RequestsCount, Video, Endpoint);
        }
    }
}
