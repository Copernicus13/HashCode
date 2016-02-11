using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

        public Program(bool bypassInputParsing, StreamReader input)
        {
            if (bypassInputParsing)
                return;
            if (input != null)
                Console.SetIn(input);

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


        public bool IsValid(List<string> lines)
        {
            try
            {
                var isValid = true;
                // Init position
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

                // Toutes les commandes ont duré pas plus de T tours
                var numberToursPerDrone = new Dictionary<int, int>();
                // première clé : customer, seconde clé : product id, value : quantité
                var productsQuantity = new Dictionary<int, Dictionary<int, int>>();
                foreach (var line in lines.Skip(1))
                {
                    var words = line.Split(' ');
                    var drone = words[0];
                    var action = words[1];
                    string whOrCustNumber = string.Empty;
                    string pdNumber = string.Empty;
                    string pdQuantity = string.Empty;
                    string turnToWait = string.Empty;
                    int droneInt = Int32.Parse(drone.ToString());
                    int whOrCustNumberInt = 0;
                    int pdNumberInt = 0;
                    int pdQuantityInt = 0;
                    int turnToWaitInt = 0;

                    if (action.Equals('W'))
                    {
                        turnToWait = words[2];
                        // Toutes les commandes sont valides
                        // Ca plante si c'est pas bon
                        turnToWaitInt = Int32.Parse(turnToWait);
                    }
                    else
                    {
                        whOrCustNumber = words[2];
                        pdNumber = words[3];
                        pdQuantity = words[4];
                        // Toutes les commandes sont valides
                        // Ca plante si c'est pas bon
                        whOrCustNumberInt = Int32.Parse(whOrCustNumber);
                        pdNumberInt = Int32.Parse(pdNumber);
                        pdQuantityInt = Int32.Parse(pdQuantity.ToString());
                    }

                    if (!(action.Equals("D") || action.Equals("L") || action.Equals("U") || action.Equals("W")))
                        isValid = false;

                    if (!numberToursPerDrone.ContainsKey(droneInt))
                    {
                        if (action.Equals("D"))
                        {
                            numberToursPerDrone.Add(droneInt, CalculMove(orders[whOrCustNumberInt].Destination, dronesPosition[droneInt]) + 1);
                        }
                        else if (action.Equals("L") || action.Equals("U"))
                        {
                            numberToursPerDrone.Add(droneInt, CalculMove(warehouses[whOrCustNumberInt].Position, dronesPosition[droneInt]) + 1);
                        }
                        else
                        {
                            numberToursPerDrone.Add(droneInt, turnToWaitInt);
                        }
                    }
                    else
                    {
                        if (action.Equals("D"))
                        {
                            numberToursPerDrone[droneInt] += CalculMove(orders[whOrCustNumberInt].Destination, dronesPosition[droneInt]) + 1;
                        }
                        else if (action.Equals("L") || action.Equals("U"))
                        {
                            numberToursPerDrone[droneInt] += CalculMove(warehouses[whOrCustNumberInt].Position, dronesPosition[droneInt]) + 1;
                        }
                        else
                        {
                            numberToursPerDrone[droneInt] += turnToWaitInt;
                        }
                    }

                    if (!productsQuantity.ContainsKey(whOrCustNumberInt))
                    {
                        if (action.Equals("D"))
                        {
                            var productTypeAndQuantity = new Dictionary<int, int>();
                            productTypeAndQuantity.Add(pdNumberInt, pdQuantityInt);
                            productsQuantity.Add(whOrCustNumberInt, productTypeAndQuantity);
                        }
                    }
                    else
                    {
                        if (action.Equals("D"))
                        {
                            if (!productsQuantity[whOrCustNumberInt].ContainsKey(pdNumberInt))
                            {
                                productsQuantity[whOrCustNumberInt].Add(pdNumberInt, pdQuantityInt);
                            }
                            else
                            {
                                productsQuantity[whOrCustNumberInt][pdNumberInt] += pdQuantityInt;
                            }
                        }
                    }

                    dronesPosition[droneInt] = action.Equals("D") ? orders[whOrCustNumberInt].Destination :
                        (action.Equals("U") || action.Equals("L") ? warehouses[whOrCustNumberInt].Position : dronesPosition[droneInt]);
                }
                if (numberToursPerDrone.Values.Max() > nbTurn)
                    isValid = false;

                // Pas de nombre d"items dans une commande > au nombre spécifié dans la commande
                foreach (var productQuantity in productsQuantity)
                {
                    foreach (var product in productQuantity.Value)
                    {
                        if (product.Value > orders[productQuantity.Key].NbItemsOfType[product.Key])
                            isValid = false;
                    }
                }

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
            new Program(false, null);
        }
    }
}
