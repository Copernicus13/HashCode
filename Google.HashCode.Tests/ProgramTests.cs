using System.Drawing;
using Google.HashCode.ConsoleApplication;
using NUnit.Framework;


namespace Google.HashCode.Tests
{
    [TestFixture]
    public class ProgramTests
    {
        [Test]
        public void CanCalculMove()
        {
            Assert.That(Program.CalculMove(new Point(0, 0), new Point(1, 1)), Is.EqualTo(2));
            Assert.That(Program.CalculMove(new Point(0, 0), new Point(2, 2)), Is.EqualTo(3));
            Assert.That(Program.CalculMove(new Point(0, 0), new Point(3, 3)), Is.EqualTo(5));
            Assert.That(Program.CalculMove(new Point(1, 5), new Point(1, 4)), Is.EqualTo(1));
            Assert.That(Program.CalculMove(new Point(1, 5), new Point(1, 3)), Is.EqualTo(2));
            Assert.That(Program.CalculMove(new Point(1, 5), new Point(2, 5)), Is.EqualTo(1));
            Assert.That(Program.CalculMove(new Point(1, 5), new Point(3, 5)), Is.EqualTo(2));
            Assert.That(Program.CalculMove(new Point(5, 0), new Point(0, 1)), Is.EqualTo(6));
        }
    }
}