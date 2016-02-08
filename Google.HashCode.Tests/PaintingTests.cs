using Google.HashCode.ConsoleApplication;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Google.HashCode.Tests
{
    [TestFixture]
    public class PaintingTests
    {
        [Test]
        public void Constructor()
        {
            // Arrange - Act :
            var painting = new Painting(1, 1);

            // Assert :
            Assert.That(painting.ToString(), Is.EqualTo("."));
        }

        [Test]
        public void ContainsCoordinates()
        {
            // Arrange - Act - Assert :
            Assert.That(new Painting(1, 1).Contains(0, 0), Is.True);
            Assert.That(new Painting(1, 1).Contains(0, 1), Is.False);
            Assert.That(new Painting(1, 1).Contains(1, 0), Is.False);
            Assert.That(new Painting(1, 1).Contains(1, 1), Is.False);
        }

        [Test]
        public void ContainsPoint()
        {
            // Arrange - Act - Assert :
            Assert.That(new Painting(1, 1).Contains(new Point(0, 0)), Is.True);
            Assert.That(new Painting(1, 1).Contains(new Point(0, 1)), Is.False);
            Assert.That(new Painting(1, 1).Contains(new Point(1, 0)), Is.False);
            Assert.That(new Painting(1, 1).Contains(new Point(1, 1)), Is.False);
            Assert.That(new Painting(3, 1).Contains(new Point(2, 0)), Is.True);
        }

        [Test]
        public void PaintCoordinates()
        {
            // Arrange :
            var painting = new Painting(1, 1);

            // Act :
            painting.Paint(0, 0);

            // Assert :
            Assert.That(painting.ToString(), Is.EqualTo("#"));
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PaintCoordinates_WithAbscissaOutOfRange()
        {
            // Arrange - Act :
            new Painting(1, 1).Paint(1, 0);

            // Assert :
            Assert.Fail("Ce test doit lever une exception.");
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PaintCoordinates_WithOrdinateOutOfRange()
        {
            // Arrange - Act :
            new Painting(1, 1).Paint(0, 1);

            // Assert :
            Assert.Fail("Ce test doit lever une exception.");
        }
    }
}
