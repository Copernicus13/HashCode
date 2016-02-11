using System;
using System.Collections.Generic;
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
        }

        public IEnumerable<string> ComputeCommands(Order order, Warehouse warehouse, int droneId)
        {
            var commands = new List<string>();

            var groups = ComputeProductGroups(order.NbItemsOfType, challenge.productWeights);

            foreach (var group in groups)
            {
                foreach (var product in group.GroupBy(p => p))
                {
                    commands.Add(string.Format("{0} L {1} {2} {3}", droneId, warehouse.Id, product.Key, product.Count()));
                }
            }

            return commands;
        }

        public Warehouse ComputeNearestWarehouse(Order order, IEnumerable<Warehouse> warehouses)
        {
            var possibleWarehouses = new List<Warehouse>();

            foreach (var warehouse in warehouses)
            {
                if (warehouse.NbItemsOfType.Select((p, i) => p >= order.NbItemsOfType[i]).All(b => b))
                {
                    possibleWarehouses.Add(warehouse);
                }
            }

            return possibleWarehouses.OrderBy(w => Program.CalculMove(w.Position, order.Destination)).FirstOrDefault();
        }

        public IEnumerable<IEnumerable<int>> ComputeProductGroups(IList<int> productList, IList<int> productWeightList)
        {
            IList<IEnumerable<int>> result = new List<IEnumerable<int>>();
            var completeProductList =
                productList.Select((p, i) => new Product { Id = i, Weight = productWeightList[i], Count = p })
                           .OrderByDescending(p => p.Weight)
                           .ToList();

            while (completeProductList.Any())
            {
                var currentCount = 0;
                var newGroupOfProduct = new List<int>();
                while (currentCount < challenge.maxPayload)
                {
                    if (completeProductList.First().Weight <= challenge.maxPayload - currentCount)
                    {
                        currentCount += completeProductList.First().Weight;
                        completeProductList.First().Count -= 1;
                        newGroupOfProduct.Add(completeProductList.First().Id);
                        if (completeProductList.First().Count == 0)
                        {
                            completeProductList.RemoveAt(0);
                        }
                    }
                }

                result.Add(newGroupOfProduct);
            }

            return result;
        }

        public IEnumerable<string> Solve()
        {
            throw new NotImplementedException();
        }
    }
}