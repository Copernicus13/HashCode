using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Google.HashCode.ConsoleApplication
{
    internal struct Drone
    {
        public readonly Point CurrentPosition;
        public readonly int DistanceTraveled;
        public readonly int Id;

        public Drone(Point currentPosition, int distanceTraveled, int id)
        {
            CurrentPosition = currentPosition;
            DistanceTraveled = distanceTraveled;
            Id = id;
        }
    }

    public class Solver
    {
        private readonly Program challenge;

        public Solver(Program challenge)
        {
            this.challenge = challenge;
            this.challenge.orders = this.challenge.orders.OrderBy(o => o.NbItemsOfType.Sum()).ToList();
        }

        public IEnumerable<string> ComputeCommands(Order order, int droneId, Point dronePosition,
                                                   int maxItemsInGroup = 0)
        {
            var canBeResolved = true;
            var commands = new List<string>();
            var newDronePosition = dronePosition;

            var groupsOfProduct = ComputeProductGroups(order.NbItemsOfType, challenge.productWeights, maxItemsInGroup);

            foreach (var groupOfProduct in groupsOfProduct)
            {
                var ofProduct = groupOfProduct as IList<int> ?? groupOfProduct.ToList();
                var nearestWarehouse = ComputeNearestWarehouse(ofProduct, challenge.warehouses, order, newDronePosition);
                if (nearestWarehouse != null)
                {
                    commands.AddRange(ComputeCommandsForOneGroup(ofProduct, nearestWarehouse, order, droneId));
                }
                else
                {
                    canBeResolved = false;
                    break;
                }
                newDronePosition = order.Destination;
            }

            return canBeResolved ? commands : new List<string>();
        }

        public int ComputeDistanceTakenByCommands(IEnumerable<string> commands, Point start)
        {
            var totalDistance = 0;
            var last = start;
            foreach (var command in commands)
            {
                var splittedCommand = command.Split(' ');
                var destination = Point.Empty;

                if (splittedCommand[1] == "D")
                {
                    destination = challenge.orders[int.Parse(splittedCommand[2])].Destination;
                }
                else if (splittedCommand[1] == "L")
                {
                    destination = challenge.warehouses[int.Parse(splittedCommand[2])].Position;
                }

                totalDistance += 1 + Program.CalculMove(last, destination);
                last = destination;
            }

            return totalDistance;
        }

        public Warehouse ComputeNearestWarehouse(IEnumerable<int> groupOfProduct, IEnumerable<Warehouse> warehouses,
                                                 Order order, Point dronePosition)
        {
            IList<int> listOfProduct = Enumerable.Repeat(0, challenge.nbProductType).ToList();
            groupOfProduct.GroupBy(v => v).ToList().ForEach(g => listOfProduct[g.Key] = g.Count());

            Predicate<Warehouse> isPossibleWarehouse =
                warehouse => warehouse.NbItemsOfType.Select((p, i) => p >= listOfProduct[i]).All(b => b);

            return
                warehouses.Where(w => isPossibleWarehouse(w))
                          .OrderBy(
                              w =>
                              Program.CalculMove(w.Position, order.Destination) +
                              Program.CalculMove(dronePosition, w.Position))
                          .FirstOrDefault();
        }

        public IEnumerable<IEnumerable<int>> ComputeProductGroups(IList<int> productList, IList<int> productWeightList,
                                                                  int maxItemsInGroup = 0)
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
                while (currentCount < challenge.maxPayload && completeProductList.Any() &&
                       (maxItemsInGroup == 0 || newGroupOfProduct.Count < maxItemsInGroup))
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

            // L'id du drone est le même que sa position dans la liste
            var droneList = new List<Drone>();
            for (var i = 0; i < challenge.nbDrone; i++)
            {
                droneList.Add(new Drone(challenge.warehouses[0].Position, 0, i));
            }

            commands.AddRange(ComputeRun(droneList, 0));
            commands.AddRange(ComputeRun(droneList, 1));

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

            nearestWarehouse.RemoveProducts(products);

            return commands;
        }

        private IEnumerable<string> ComputeRun(IList<Drone> droneList, int maxItemsInGroup)
        {
            var commands = new List<string>();
            foreach (var order in challenge.orders.Where(o => o.Completed == false))
            {
                var droneId =
                    droneList.OrderBy(i => i.DistanceTraveled + Program.CalculMove(i.CurrentPosition, order.Destination))
                             .First();

                var computedCommands =
                    ComputeCommands(order, droneId.Id, droneId.CurrentPosition, maxItemsInGroup).ToList();
                commands.AddRange(computedCommands);

                if (computedCommands.Any())
                {
                    order.Completed = true;

                    droneList[droneId.Id] = new Drone(order.Destination,
                                                      droneId.DistanceTraveled +
                                                      ComputeDistanceTakenByCommands(computedCommands,
                                                                                     droneId.CurrentPosition),
                                                      droneId.Id);
                }
            }

            return commands;
        }
    }
}