using System;
using System.Collections.Generic;
using System.Drawing;

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
                        Position = new Point(int.Parse(words[0]), int.Parse(words[1]))
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
                        Destination = new Point(int.Parse(words[0]), int.Parse(words[1]))
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

        public static void Main(string[] args)
        {
            // ReSharper disable once ObjectCreationAsStatement
            new Program();
        }
    }
}
