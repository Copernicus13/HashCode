using System;
using System.Collections.Generic;
using System.Linq;

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
            var listeResult = GenererListeResult();
            var listeLigneResultat = ParseListEndPoint(listeResult);

            Console.WriteLine($"{listeLigneResultat.GroupBy(x => x.CacheId).Count()}");
            foreach (var ligneResultat in listeLigneResultat.GroupBy(x => x.CacheId))
            {
                Console.WriteLine($"{ligneResultat.Key} {string.Join(" ", ligneResultat.Select(x => x.VideoId))}");
            }
        }

        private List<Result> GenererListeResult()
        {
            var listeResultat = new List<Result>();
            foreach (var video in Videos)
            {
                var resultatPourVideo = new Result { Video = video };
                var requests = Requests.Where(request => request.Video.VideoId == video.VideoId);
                var endpoints = requests.Select(request => request.Endpoint);

                if (video.Size > CacheSize)
                {
                    resultatPourVideo.MinSumLatency = requests.Sum(requete => requete.Endpoint.Latency * requete.RequestsCount);
                    resultatPourVideo.Endpoints = requests.Select(requete => Tuple.Create(requete.Endpoint, -1)).ToList();
                }
                else
                {
                    resultatPourVideo.MinSumLatency = 0;
                    resultatPourVideo.Endpoints = new List<Tuple<Endpoint, int>>();
                    foreach (var requete in requests)
                    {
                        var meilleurCache = requete.Endpoint.Caches.OrderBy(cache => cache.Latency).FirstOrDefault();
                        if (meilleurCache != null && meilleurCache.Latency <= requete.Endpoint.Latency)
                        {
                            resultatPourVideo.MinSumLatency += meilleurCache.Latency * requete.RequestsCount;
                            resultatPourVideo.Endpoints.Add(Tuple.Create(requete.Endpoint, meilleurCache.CacheId));
                        }
                        else
                        {
                            resultatPourVideo.MinSumLatency += requete.Endpoint.Latency * requete.RequestsCount;
                            resultatPourVideo.Endpoints.Add(Tuple.Create(requete.Endpoint, -1));
                        }
                    }
                }
                listeResultat.Add(resultatPourVideo);
            }
            return listeResultat;
        }

        public List<LigneResultat> ParseListEndPoint(List<Result> listResultat)
        {
            var resultatTemp = new List<LigneResultat>();

            foreach (var result in listResultat.OrderByDescending(res => res.MinSumLatency))
            {
                foreach (var endpoint in result.Endpoints)
                {
                    // si item2 == -1
                    if (endpoint.Item2 == -1)
                    {
                        continue;
                    }

                    // sinon
                    foreach (var cache in endpoint.Item1.Caches.OrderBy(cache => cache.Latency))
                    {
                        var cacheTemp = resultatTemp.GroupBy(res => res.CacheId).FirstOrDefault(group => group.Key == cache.CacheId);
                        if (cacheTemp == null || (cacheTemp.Sum(video => video.VideoSize) + result.Video.Size <= CacheSize))
                        {
                            if (cacheTemp == null || !cacheTemp.Any(video => video.VideoId == result.Video.VideoId))
                            {
                                resultatTemp.Add(new LigneResultat
                                {
                                    CacheId = cache.CacheId,
                                    VideoId = result.Video.VideoId,
                                    VideoSize = result.Video.Size
                                });
                            }
                            break;
                        }
                    }
                }
            }

            return resultatTemp;
        }

        public class Result
        {
            public Video Video { get; set; }
            // Somme des latences minimales
            public long MinSumLatency { get; set; }
            // Liste des endints avec cache optimal ou -1 si ds data center
            public ICollection<Tuple<Endpoint, int>> Endpoints { get; set; }
        }
    }
}
