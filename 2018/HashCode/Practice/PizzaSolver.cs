using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CoperAlgoLib.Data;
using CoperAlgoLib.Geometry;

namespace HashCode.Practice
{
    public class PizzaSolver
    {
        // Input data
        public int NbRows { get; private set; }
        public int NbColumns { get; private set; }
        public int MinIngredient { get; private set; }
        public int MaxSliceCells { get; private set; }
        public int[,] Pizza { get; private set; }

        // Computed data
        private List<int> _PizzaList;
        public List<int> PizzaList
        {
            get { return _PizzaList ?? (_PizzaList = Pizza.ToList()); }
        }
        public int NbTomatoes { get; private set; }
        public int NbMushrooms { get; private set; }
        public int NbCells { get; private set; }
        public int TheoreticalBestNbSlices { get; private set; }
        public int TheoreticalMaxPossibleSlices { get; private set; }

        private IList<Rectangle> _Combinations;

        public PizzaSolver(StreamReader inputStream)
        {
            int[] config = inputStream.ReadLine().Split(' ').Select(s => int.Parse(s)).ToArray();
            NbRows = config[0];
            NbColumns = config[1];
            MinIngredient = config[2];
            MaxSliceCells = config[3];

            Pizza = new int[NbRows, NbColumns];
            for (int y = 0; y < NbRows; ++y)
            {
                string line = inputStream.ReadLine();
                for (int x = 0; x < NbColumns; ++x)
                    Pizza[y, x] = line[x] == 'T' ? 1 : 2;
            }

            NbTomatoes = PizzaList.Count(c => c == 1);
            NbMushrooms = PizzaList.Count(c => c == 2);
            NbCells = NbRows * NbColumns;
            TheoreticalBestNbSlices = (int)Math.Ceiling((double)NbCells / MaxSliceCells);
            TheoreticalMaxPossibleSlices = Math.Min(NbTomatoes, NbMushrooms) / MinIngredient;
        }

        public void ResolveUsingBruteforce(bool resultToConsoleOnly = true)
        {
            int i = 0;
            _Combinations = CreateAllValidSlices()
                .OrderByDescending(o => o.Area)
                .ToList();

            int r = _Combinations.Count / 2;

            while (i < r)
            {
                var d = ArrangeCombinations(new List<Rectangle> { _Combinations[i] });
                Console.Write(i + " : ");
                Console.WriteLine(d.Aggregate(0l, (a, b) => a + b.Area));
                foreach (var g in d)
                    Console.Write(g.Y1 + " " + g.X1 + " " + (g.Y2 - 1) + " " + (g.X2 - 1) + " | ");
                Console.WriteLine();
                ++i;
            }
        }

        public void ResolveUsingLightBruteforce(bool resultToConsoleOnly = true)
        {
            _Combinations = CreateAllValidSlices()
                .OrderByDescending(o => o.Area)
                .ThenBy(t => t.X)
                .ThenBy(t => t.Y)
                .ToList();

            IList<Rectangle> actual = new List<Rectangle> { _Combinations[0] };
            for (int i = 0; i < _Combinations.Count; ++i)
            {
                bool isIntersecting = false;
                foreach (var t in actual)
                    if (_Combinations[i].IntersectsWith(t))
                    {
                        isIntersecting = true;
                        break;
                    }
                if (!isIntersecting)
                    actual.Add(_Combinations[i]);
            }
            Console.WriteLine(actual.Aggregate(0l, (a, b) => a + b.Area));
            foreach (var g in actual)
                Console.Write(g.Y1 + " " + g.X1 + " " + (g.Y2 - 1) + " " + (g.X2 - 1) + " | ");
            Console.WriteLine();
        }

        private IList<Rectangle> ArrangeCombinations(IList<Rectangle> tmp)
        {
            IList<Rectangle> possible = new List<Rectangle>();
            IList<Rectangle> best = new List<Rectangle>();
            for (int j = 0; j < _Combinations.Count; ++j)
            {
                bool isIntersecting = false;
                foreach (var t in tmp)
                    if (_Combinations[j].IntersectsWith(t))
                    {
                        isIntersecting = true;
                        break;
                    }
                if (!isIntersecting)
                    possible.Add(_Combinations[j]);
            }
            if (!possible.Any())
                return tmp;
            long res = tmp.Aggregate(0l, (a, b) => a + b.Area);
            var m = possible.Max(o => o.Area);
            possible = possible.Where(w => w.Area == m).ToList();
            foreach (var q in possible)
            {
                var l = new List<Rectangle>(tmp);
                l.Add(q);
                IList<Rectangle> y = new List<Rectangle>();
                if (l.Count < TheoreticalMaxPossibleSlices)
                    y = ArrangeCombinations(l);
                else
                    y = l;
                var z = y.Aggregate(0l, (a, b) => a + b.Area);
                if (z > res)
                {
                    best = y;
                    res = z;
                }
            }
            return best;
        }

        private IList<Rectangle> CreateAllValidSlices()
        {
            IList<Rectangle> list = new List<Rectangle>();
            for (int y = 0; y < NbRows; ++y)
                for (int x = 0; x < NbColumns; ++x)
                    for (int h = 1; h <= NbRows - y && h <= MaxSliceCells; ++h)
                        for (int w = 1; w <= NbColumns - x && w * h <= MaxSliceCells; ++w)
                            if (IsValid(Pizza.GetSubArray(x, y, w, h)))
                                list.Add(new Rectangle(x, y, w, h));
            return list;
        }

        private bool IsValid(int[,] pizza)
        {
            var list = pizza.ToList();
            return list.Count <= MaxSliceCells &&
                list.Count(c => c == 1) >= MinIngredient &&
                list.Count(c => c == 2) >= MinIngredient;
        }
    }
}
