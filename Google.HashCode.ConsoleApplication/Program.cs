using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Google.HashCode.ConsoleApplication
{
    public class Program
    {
        public int[,] _map;
        public int nbRow;
        public int nbCol;
        public int nbDrone;
        public int nbTurn;
        public int maxPayload;
        public int nbProductType;
        public IList<int> productWeights = new List<int>();
        public int nbWarehouse;
        public IList<Warehouse> warehouses = new List<Warehouse>();
        public int nbOrder;
        public IList<Order> orders = new List<Order>();

        public Program()
        {
            // Rows, Columns, Drones, Max turns, Max payload
            string line = Console.ReadLine();
            var words = line.Split(' ');
            nbRow = int.Parse(words[0]);
            nbCol = int.Parse(words[1]);
            nbDrone = int.Parse(words[2]);
            nbTurn = int.Parse(words[3]);
            maxPayload = int.Parse(words[4]);

            // Number of product types
            line = Console.ReadLine();
            nbProductType = int.Parse(line);

            // Product weigths
            line = Console.ReadLine();
            foreach (string s in line.Split(' '))
                productWeights.Add(int.Parse(s));

            // Number of warehouses
            line = Console.ReadLine();
            nbWarehouse = int.Parse(line);

            // Warehouses data
            for (int i = 0; i < nbWarehouse; ++i)
            {
                // Location
                line = Console.ReadLine();
                words = line.Split(' ');
                var wh = new Warehouse(nbProductType)
                    {
                        Position = new Point(int.Parse(words[0]), int.Parse(words[1])),
                        Id = i
                    };

                // Products stored
                line = Console.ReadLine();
                int j = 0;
                foreach (string s in line.Split(' '))
                    wh.NbItemsOfType[j++] = int.Parse(s);

                warehouses.Add(wh);
            }

            // Number of orders
            line = Console.ReadLine();
            nbOrder = int.Parse(line);

            // Orders data
            for (int i = 0; i < nbOrder; ++i)
            {
                // Destination
                line = Console.ReadLine();
                words = line.Split(' ');
                var ord = new Order(nbProductType)
                    {
                        Destination = new Point(int.Parse(words[0]), int.Parse(words[1])),
                        Id = i
                    };

                // Items count in order, we do not care...
                Console.ReadLine();

                // Product types
                line = Console.ReadLine();
                foreach (string s in line.Split(' '))
                    ++ord.NbItemsOfType[int.Parse(s)];

                orders.Add(ord);
            }
        }


        private bool IsValid(List<string> lines)
        {
            try
            {
                var isValid = true;
                var dronesPosition = new List<Point>();
                for (int i = 0; i < nbDrone; i++)
                {
                    dronesPosition.Add(warehouses[0].Position);
                }

                // Conforme à la description
                if (lines.Count == 0 || lines.Count == 1)
                    isValid = false;
                else
                {
                    if ((lines.Count - 1) != Int32.Parse(lines[0]))
                        isValid = false;
                }

                // Toutes les commandes sont valides

                // Pas de nombre d'items dans une commande > au nombre spécifié dans la commande

                // Toutes les commandes ont duré pas plus de T tours
                var numberToursPerDrone = new Dictionary<int, int>();
                foreach (var line in lines)
                {
                    var drone = line[0];
                    var action = line[1];
                    var whOrCustNumber = line[2];
                    var pdNumber = line[3];
                    var pdQuantity = line[4];

                    if (!numberToursPerDrone.ContainsKey(drone))
                        numberToursPerDrone.Add(drone, CalculMove(action.Equals('D') ? orders[whOrCustNumber].Destination : warehouses[whOrCustNumber].Position, dronesPosition[drone]));
                    else
                    {
                        numberToursPerDrone[drone] += CalculMove(action.Equals('D') ? orders[whOrCustNumber].Destination : warehouses[whOrCustNumber].Position, dronesPosition[drone]);
                    }

                    dronesPosition[drone] = action.Equals('D') ? orders[whOrCustNumber].Destination : warehouses[whOrCustNumber].Position;
                }
                if (numberToursPerDrone.Values.Max() > nbTurn)
                    isValid = false;

                return isValid;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static int CalculMove(Point departure, Point arrival)
        {
            var result = 0d;

            result = Math.Sqrt(Math.Pow(Math.Abs(departure.X - arrival.X), 2) + Math.Pow(Math.Abs(departure.Y - arrival.Y), 2));

            return (int)Math.Ceiling(result);
        }


        public static void Main(string[] args)
        {
            // ReSharper disable once ObjectCreationAsStatement
            new Program();
        }
    }
}
