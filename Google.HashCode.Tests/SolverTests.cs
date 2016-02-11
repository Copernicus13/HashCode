using System.Collections.Generic;
using Google.HashCode.ConsoleApplication;
using NUnit.Framework;

namespace Google.HashCode.Tests
{
    [TestFixture]
    internal class SolverTests
    {
        [Test]
        public void CanComputeCommands()
        {
            var solver = new Solver(new Program(true) { maxPayload = 400, productWeights = new[] { 100, 50, 200 } });
            var order = new Order(5) { Id = 0, NbItemsOfType = new[] { 1, 2, 3 } };
            var warehouse = new Warehouse(5) { Id = 0, NbItemsOfType = new[] { 1, 2, 3 } };

            var actual = solver.ComputeCommands(order, warehouse, 0);

            var expected = new[]
                {
                    "0 L 0 2 2", "0 D 0 2 2", "0 L 0 2 1", "0 L 0 0 1", "0 L 0 1 2", "0 D 0 2 1", "0 D 0 0 1",
                    "0 D 0 1 2", "0 L 0 0 0"
                };

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void CanComputeProductGroups()
        {
            var solver = new Solver(new Program(true) { maxPayload = 400 });
            var productList = new[] { 1, 2, 3 };
            var weightList = new[] { 100, 50, 200 };

            IList<IEnumerable<int>> expectedList = new[] { new[] { 2, 2 }, new[] { 2, 0, 1, 1 } };

            var actual = solver.ComputeProductGroups(productList, weightList);
            Assert.That(actual, Is.EqualTo(expectedList));
        }
    }
}