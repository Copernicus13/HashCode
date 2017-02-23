using System.Collections.Generic;

namespace HashCode
{
    public partial class Solver
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

        public void Solve()
        {
            var listeResultat = new List<Result>();
            foreach (var video in Videos)
            {
                var resultatPourVideo = new Result { VideoId = video.VideoId };
                var requests = Requests.Where(request => request.Video.VideoId == video.VideoId);
                var endpoints = requests.Select(request => request.Endpoint);

                if (video.Size > CacheSize)
                {
                    resultatPourVideo.MinSumLatency = requests.Sum(requete => requete.Endpoint.Latency * requete.RequestsCount);
                    resultatPourVideo.Endpoints = requests.Select(requete => Tuple.Create(requete.Endpoint.EndpointId, -1)).ToList();
                }
                else
                {
                    resultatPourVideo.MinSumLatency = 0;
                    resultatPourVideo.Endpoints = new List<Tuple<int, int>>();
                    foreach (var requete in requests)
                    {

                        var meilleurCache = requete.Endpoint.Caches.OrderBy(cache => cache.Latency).FirstOrDefault();
                        if (meilleurCache != null && meilleurCache.Latency <= requete.Endpoint.Latency)
                        {
                            resultatPourVideo.MinSumLatency += meilleurCache.Latency * requete.RequestsCount;
                            resultatPourVideo.Endpoints.Add(Tuple.Create(requete.Endpoint.EndpointId, meilleurCache.CacheId));
                        }
                        else
                        {
                            resultatPourVideo.MinSumLatency += requete.Endpoint.Latency * requete.RequestsCount;
                            resultatPourVideo.Endpoints.Add(Tuple.Create(requete.Endpoint.EndpointId, -1));
                        }
                    }
                }

                listeResultat.Add(resultatPourVideo);
            }


        }

        private class Result
        {
            public int VideoId { get; set; }
            public long MinSumLatency { get; set; }
            public ICollection<Tuple<int, int>> Endpoints { get; set; }
        }
    }
}
