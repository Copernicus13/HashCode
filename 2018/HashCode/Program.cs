using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HashCode
{
    public class Program
    {
        private const string path = @"C:\Users\PC\Documents\GitHub\HashCode\2018\HashCode\Input\";

        public static void Main(string[] args)
        {
            var input = new List<string>();
            //using (var j = File.OpenText(path + "file1.in"))
            //using (var j = File.OpenText(path + "file2.in"))
            //using (var j = File.OpenText(path + "file3.in"))
            //using (var j = File.OpenText(path + "file4.in"))
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

            // TODO : parse input

            // Examples :
            //input[inputRowNumber].FirstIs<int>(i => solver.VideosCount = i);
            //input[inputRowNumber].SecondIs<int>(i => solver.EndpointsCount = i);
            //inputRowNumber++;
            //for (int index = 0; index < solver.VideosCount; index++)
            //{
            //    var video = new Video();
            //    video.VideoId = index;
            //    input[inputRowNumber].NthIs<int>(index, size => video.Size = size);

            //    solver.Videos.Add(video);
            //}

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
