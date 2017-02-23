using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HashCode
{
    public class Program
    {
        private const string path = @"C:\Users\Michel\Développement\HashCode\2017\HashCode\Input\";

        public static void Main(string[] args)
        {
            var input = new List<string>();
            //using (var j = File.OpenText(path + "me_at_the_zoo.in"))
            //using (var j = File.OpenText(path + "videos_worth_spreading.in"))
            //using (var j = File.OpenText(path + "trending_today.in"))
            //using (var j = File.OpenText(path + "kittens.in"))
            using (var j = File.OpenText(path + "test.in"))
            {
                while (!j.EndOfStream)
                    input.Add(j.ReadLine());
            }
            var solver = ParseInput(input);
            //solver.Dummy();
            solver.Solve();
            Console.Error.WriteLine("Traitement terminé. À soumettre viiiite !!!");
        }

        public static Solver ParseInput(IList<string> input)
        {
            var solver = new Solver();
            var inputRowNumber = 0;

            // Summary (line 0)
            input[inputRowNumber].FirstIs<int>(i => solver.VideosCount = i);
            input[inputRowNumber].SecondIs<int>(i => solver.EndpointsCount = i);
            input[inputRowNumber].ThirdIs<int>(i => solver.RequestsCount = i);
            input[inputRowNumber].FourthIs<int>(i => solver.CachesCount = i);
            input[inputRowNumber].FifthIs<int>(i => solver.CacheSize = i);

            // Videos (line 1)
            inputRowNumber++;
            for (int videoIndex = 0; videoIndex < solver.VideosCount; videoIndex++)
            {
                var video = new Video();
                video.VideoId = videoIndex;
                input[inputRowNumber].NthIs<int>(videoIndex, size => video.Size = size);

                solver.Videos.Add(video);
            }

            // Endpoints
            for (int endpointIndex = 0; endpointIndex < solver.EndpointsCount; endpointIndex++)
            {
                inputRowNumber++;
                var endpoint = new Endpoint();
                endpoint.EndpointId = endpointIndex;
                input[inputRowNumber].FirstIs<int>(latency => endpoint.Latency = latency);
                input[inputRowNumber].SecondIs<int>(cachesCount => endpoint.CachesCount = cachesCount);

                for (int cacheIndex = 0; cacheIndex < endpoint.CachesCount; cacheIndex++)
                {
                    inputRowNumber++;
                    var cache = new Cache();
                    input[inputRowNumber].FirstIs<int>(cacheId => cache.CacheId = cacheId);
                    input[inputRowNumber].SecondIs<int>(cacheLatency => cache.Latency = cacheLatency);
                    endpoint.Caches.Add(cache);
                }

                solver.Endpoints.Add(endpoint);
            }

            // Requests 
            for (int requestIndex = 0; requestIndex < solver.RequestsCount; requestIndex++)
            {
                inputRowNumber++;
                var request = new Request();
                request.RequestId = requestIndex;
                input[inputRowNumber].FirstIs<int>(videoId => request.Video = solver.Videos.First(v => v.VideoId == videoId));
                input[inputRowNumber].SecondIs<int>(endPointId => request.Endpoint = solver.Endpoints.First(e => e.EndpointId == endPointId));
                input[inputRowNumber].ThirdIs<int>(requestsCount => request.RequestsCount = requestsCount);

                solver.Requests.Add(request);
            }

            return solver;
        }
    }

    public static class UsefulExtensions
    {
        private const char _Separator = ' ';

        // Get single
        public static T GetAs<T>(this IList<string> input, int rowIndex, int colIndex) { return input.Row(rowIndex).Col(colIndex).As<T>(); }
        public static string Row(this IList<string> list, int rowIndex) { return list.Skip(rowIndex).First(); }
        public static string Col(this string input, int index) { return input.Split(_Separator)[index]; }
        public static T As<T>(this string input) { return (T)Convert.ChangeType(input, typeof(T)); }
        // Get list
        public static IList<T> GetAsListOf<T>(this IList<string> input, int rowStartIndex, int rowCount) { return input.Skip(rowStartIndex).Take(rowCount).Select(s => s.As<T>()).ToList(); }
        public static IList<T> GetAsListOf<T>(this string row) { return row.Split(_Separator).Select(s => s.As<T>()).ToList(); }
        // Fluent parsing
        public static string FirstIs<T>(this string input, Action<T> affectTo) { return input.NthIs<T>(0, affectTo); }
        public static string SecondIs<T>(this string input, Action<T> affectTo) { return input.NthIs<T>(1, affectTo); }
        public static string ThirdIs<T>(this string input, Action<T> affectTo) { return input.NthIs<T>(2, affectTo); }
        public static string FourthIs<T>(this string input, Action<T> affectTo) { return input.NthIs<T>(3, affectTo); }
        public static string FifthIs<T>(this string input, Action<T> affectTo) { return input.NthIs<T>(4, affectTo); }
        public static string SixthIs<T>(this string input, Action<T> affectTo) { return input.NthIs<T>(5, affectTo); }
        public static string NthIs<T>(this string input, int colIndex, Action<T> affectTo) { affectTo(input.Col(colIndex).As<T>()); return input; }
    }
}
