using System.Collections.Generic;
using System.Drawing;
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
            var solver =
                new Solver(new Program(true, null)
                    {
                        maxPayload = 400,
                        productWeights = new[] { 100, 50, 200 },
                        nbProductType = 3,
                        warehouses =
                            new List<Warehouse>
                                {
                                    new Warehouse(3)
                                        {
                                            Id = 1,
                                            NbItemsOfType = new[] { 1, 2, 3 },
                                            Position = new Point(1, 1)
                                        },
                                    new Warehouse(3)
                                        {
                                            Id = 0,
                                            NbItemsOfType = new[] { 1, 2, 3 },
                                            Position = new Point(0, 0)
                                        }
                                }
                    });
            var order = new Order(3) { Id = 0, NbItemsOfType = new[] { 1, 2, 3 }, Destination = new Point(0, 0) };

            var actual = solver.ComputeCommands(order, 0, Point.Empty);

            var expected = new[]
                {
                    "0 L 0 2 2", "0 D 0 2 2", "0 L 0 2 1", "0 L 0 0 1", "0 L 0 1 2", "0 D 0 2 1", "0 D 0 0 1",
                    "0 D 0 1 2"
                };

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void CanComputeDistanceTakenByCommands()
        {
            var solver =
                new Solver(new Program(true, null)
                    {
                        maxPayload = 400,
                        productWeights = new[] { 100, 50, 200 },
                        nbProductType = 3,
                        warehouses =
                            new List<Warehouse>
                                {
                                    new Warehouse(3)
                                        {
                                            Id = 0,
                                            NbItemsOfType = new[] { 1, 2, 3 },
                                            Position = new Point(5, 2)
                                        }
                                },
                        orders =
                            new List<Order>
                                {
                                    new Order(3)
                                        {
                                            Id = 0,
                                            NbItemsOfType = new[] { 1, 2, 3 },
                                            Destination = new Point(3, 4)
                                        }
                                }
                    });

            var actual = solver.ComputeDistanceTakenByCommands(new[] { "0 L 0 2 2", "0 D 0 2 2", "0 L 0 2 1" },
                                                               new Point(0, 0));

            Assert.That(actual, Is.EqualTo(15));
        }

        [Test]
        public void CanComputeDistanceTakenByNoCommandsIsZero()
        {
            var solver = new Solver(new Program(true, null));

            var actual = solver.ComputeDistanceTakenByCommands(new string[] { }, new Point(0, 0));

            Assert.That(actual, Is.EqualTo(0));
        }

        [Test]
        public void CanComputeProductGroups()
        {
            var solver = new Solver(new Program(true, null) { maxPayload = 400 });
            var productList = new[] { 1, 2, 3 };
            var weightList = new[] { 100, 50, 200 };

            IList<IEnumerable<int>> expectedList = new[] { new[] { 2, 2 }, new[] { 2, 0, 1, 1 } };

            var actual = solver.ComputeProductGroups(productList, weightList);
            Assert.That(actual, Is.EqualTo(expectedList));
        }

        [Test]
        public void CanComputeProductGroupsIsSemiOptimized()
        {
            var solver = new Solver(new Program(true, null) { maxPayload = 500 });
            var productList = new[] { 2, 2 };
            var weightList = new[] { 450, 50 };

            IList<IEnumerable<int>> expectedList = new[] { new[] { 0, 1 }, new[] { 0, 1 } };

            var actual = solver.ComputeProductGroups(productList, weightList);
            Assert.That(actual, Is.EqualTo(expectedList));
        }
    }
}