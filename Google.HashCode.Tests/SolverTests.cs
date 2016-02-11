using System.Collections.Generic;
using Google.HashCode.ConsoleApplication;
using NUnit.Framework;

namespace Google.HashCode.Tests
{
    [TestFixture]
    internal class SolverTests
    {
        [Test]
        public void CanComputeProductGroups()
        {
            var solver = new Solver(new Program(true) { maxPayload = 400 });
            var productList = new[] { 1, 2, 3 };
            var weightList = new[] { 100, 50, 200 };

            IList<IEnumerable<int>> expectedList  = new[] { new[] { 2, 2 }, new[] { 2, 0, 1, 1 } };

            var actual = solver.ComputeProductGroups(productList, weightList);
            Assert.That(actual, Is.EqualTo(expectedList));
        }
    }
}