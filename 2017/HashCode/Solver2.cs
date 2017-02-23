using System;
using System.Linq;

namespace HashCode
{
    public partial class Solver
    {
        public void Dummy()
        {
            Console.WriteLine(CachesCount);
            for (int i = 0; i < CachesCount; ++i)
                Console.WriteLine("{0} {1}", i, i);

            foreach (var video in Videos)
            {
                var requests = Requests
                    .Where(request => request.Video.VideoId == video.VideoId)
                    .GroupBy(g => g.Endpoint)
                    .Select(s => s.Key)
                    .ToList();
            }
        }
    }
}
