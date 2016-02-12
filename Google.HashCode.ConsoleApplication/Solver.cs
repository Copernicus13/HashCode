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
            this.challenge.orders = this.challenge.orders.OrderBy(o => o.NbItemsOfType.Sum()).ToList();
        }

        public static Warehouse ComputeNearestWarehouse(Order order, IEnumerable<Warehouse> warehouses)
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

        public IEnumerable<string> ComputeCommands(Order order, Warehouse warehouse, int droneId)
        {
            var commands = new List<string>();

            var groupsOfProduct = ComputeProductGroups(order.NbItemsOfType, challenge.productWeights);

            foreach (var groupOfProduct in groupsOfProduct)
            {
                var products = groupOfProduct as IList<int> ?? groupOfProduct.ToList();

                foreach (var product in products.GroupBy(p => p))
                {
                    commands.Add(string.Format("{0} L {1} {2} {3}", droneId, warehouse.Id, product.Key, product.Count()));
                }

                foreach (var product in products.GroupBy(p => p))
                {
                    commands.Add(string.Format("{0} D {1} {2} {3}", droneId, order.Id, product.Key, product.Count()));
                }
            }

            return commands;
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
            var droneId = 0;

            foreach (var order in challenge.orders)
            {
                var nearestWarehouse = ComputeNearestWarehouse(order, challenge.warehouses);
                if (nearestWarehouse != null)
                {
                    commands.AddRange(ComputeCommands(order, nearestWarehouse, droneId % challenge.nbDrone));
                    RemoveProducts(nearestWarehouse, order);
                }
                droneId += 1;
            }

            return commands;
        }

        private static void RemoveProducts(Warehouse nearestWarehouse, Order order)
        {
            for (var i = 0; i < order.NbItemsOfType.Count; i++)
            {
                nearestWarehouse.NbItemsOfType[i] -= order.NbItemsOfType[i];
            }
        }
    }
}