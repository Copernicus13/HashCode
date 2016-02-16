using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Google.HashCode.ConsoleApplication
{
    public class Product
    {
        public int Count { get; set; }
        public int Id { get; set; }
        public int Weight { get; set; }
    }

    public class Solver
    {
        private readonly Program challenge;

        public Solver(Program challenge)
        {
            this.challenge = challenge;
            this.challenge.orders = this.challenge.orders.OrderBy(o => o.NbItemsOfType.Sum()).ToList();
        }

        public IEnumerable<string> ComputeCommands(Order order, int droneId)
        {
            var canBeResolved = true;
            var commands = new List<string>();

            var groupsOfProduct = ComputeProductGroups(order.NbItemsOfType, challenge.productWeights);

            foreach (var groupOfProduct in groupsOfProduct)
            {
                var ofProduct = groupOfProduct as IList<int> ?? groupOfProduct.ToList();
                var nearestWarehouse = ComputeNearestWarehouse(ofProduct, challenge.warehouses, order);
                if (nearestWarehouse != null)
                {
                    commands.AddRange(ComputeCommandsForOneGroup(ofProduct, nearestWarehouse, order, droneId));
                }
                else
                {
                    canBeResolved = false;
                    break;
                }
            }

            return canBeResolved ? commands : new List<string>();
        }

        public Warehouse ComputeNearestWarehouse(IEnumerable<int> groupOfProduct,
                                                 IEnumerable<Warehouse> warehouses, Order order)
        {
            IList<int> listOfProduct = Enumerable.Repeat(0, challenge.nbProductType).ToList();
            groupOfProduct.GroupBy(v => v).ToList().ForEach(g => listOfProduct[g.Key] = g.Count());

            Predicate<Warehouse> isPossibleWarehouse =
                warehouse => warehouse.NbItemsOfType.Select((p, i) => p >= listOfProduct[i]).All(b => b);

            return
                warehouses.Where(w => isPossibleWarehouse(w))
                          .OrderBy(w => Program.CalculMove(w.Position, order.Destination))
                          .FirstOrDefault();
        }

        public IEnumerable<IEnumerable<int>> ComputeProductGroups(IList<int> productList, IList<int> productWeightList)
        {
            IList<IEnumerable<int>> result = new List<IEnumerable<int>>();
            var completeProductList =
                productList.Select((p, i) => new Product { Id = i, Weight = productWeightList[i], Count = p })
                           .OrderByDescending(p => p.Weight)
                           .Where(p => p.Count > 0)
                           .ToList();

            while (completeProductList.Any())
            {
                var currentCount = 0;
                var newGroupOfProduct = new List<int>();
                while (currentCount < challenge.maxPayload && completeProductList.Any())
                {
                    var possible =
                        completeProductList.FirstOrDefault(p => p.Weight <= challenge.maxPayload - currentCount);

                    if (possible != null)
                    {
                        currentCount += possible.Weight;
                        newGroupOfProduct.Add(possible.Id);
                        completeProductList.First(p => p.Id == possible.Id).Count -= 1;
                        if (completeProductList.First(p => p.Id == possible.Id).Count == 0)
                        {
                            completeProductList.Remove(completeProductList.First(p => p.Id == possible.Id));
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                result.Add(newGroupOfProduct);
            }

            return result;
        }

        public IEnumerable<string> Solve()
        {
            var commands = new List<string>();
            var droneList = new Dictionary<int, Tuple<int, Point>>();

            for (var i = 0; i < challenge.nbDrone; i++)
            {
                droneList.Add(i, Tuple.Create(0, challenge.warehouses[0].Position));
            }

            foreach (var order in challenge.orders)
            {
                var droneId =
                    droneList.OrderBy(i => i.Value.Item1)
                             .First();
                var computedCommands = ComputeCommands(order, droneId.Key).ToList();
                commands.AddRange(computedCommands);
                droneList[droneId.Key] =
                    Tuple.Create(
                        droneList[droneId.Key].Item1 +
                        ComputeDistanceTakenByCommands(computedCommands, droneList[droneId.Key].Item2),
                        order.Destination);
            }

            return commands;
        }

        private static IEnumerable<string> ComputeCommandsForOneGroup(IList<int> products, Warehouse nearestWarehouse,
                                                                      Order order, int droneId)
        {
            var commands = new List<string>();
            foreach (var product in products.GroupBy(p => p))
            {
                commands.Add(string.Format("{0} L {1} {2} {3}", droneId, nearestWarehouse.Id, product.Key,
                                           product.Count()));
            }

            foreach (var product in products.GroupBy(p => p))
            {
                commands.Add(string.Format("{0} D {1} {2} {3}", droneId, order.Id, product.Key, product.Count()));
            }

            RemoveProducts(nearestWarehouse, products);

            return commands;
        }

        private static void RemoveProducts(Warehouse nearestWarehouse, IEnumerable<int> groupOfProduct)
        {
            foreach (var product in groupOfProduct)
            {
                nearestWarehouse.NbItemsOfType[product] -= 1;
            }
        }

        private int ComputeDistanceTakenByCommands(IEnumerable<string> commands, Point start)
        {
            var totalDistance = 0;
            foreach (var command in commands)
            {
                var splittedCommand = command.Split(' ');
                var destination = new Point();

                if (splittedCommand[1] == "D")
                {
                    destination = challenge.orders[int.Parse(splittedCommand[2])].Destination;
                }
                else if (splittedCommand[1] == "L")
                {
                    destination = challenge.warehouses[int.Parse(splittedCommand[2])].Position;
                }

                totalDistance += Program.CalculMove(start, destination);
            }

            return totalDistance;
        }
    }
}