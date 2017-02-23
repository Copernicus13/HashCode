using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCode
{
    public class Solver
    {
        public int VideosCount { get; set; }
        public int EndpointsCount { get; set; }
        public int RequestsCount { get; set; }
        public int CachesCount { get; set; }
        public int CacheSize { get; set; }

        public IList<Video> Videos { get; set; }
        public IList<Endpoint> Endpoints { get; set; }
        public IList<Request> Requests { get; set; }

        public Solver()
        {
            this.Videos = new List<Video>();
            this.Endpoints = new List<Endpoint>();
            this.Requests = new List<Request>();
        }
    }
}
