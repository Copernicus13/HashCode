namespace HashCode
{
    public class Video
    {
        public int VideoId { get; set; }
        public int Size { get; set; }

        public override string ToString()
        {
            return string.Format("[{0}] {1}MB", VideoId, Size);
        }
    }
}
