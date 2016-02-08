using Google.HashCode.ConsoleApplication;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Google.HashCode.Tests
{
    [TestFixture]
    public class PaintingMachineTests
    {
        [Test]
        public void PaintSquare()
        {
            // Arrange :
            var painting = new Painting(5, 5);
            var paintingMachine = new PaintingMachine(painting);
            var expectedDraw =
                "....." + Environment.NewLine +
                ".###." + Environment.NewLine +
                ".###." + Environment.NewLine +
                ".###." + Environment.NewLine +
                ".....";

            // Act :
            paintingMachine.PaintSquare(2, 2, 1);
            var actualDraw = painting.ToString();

            // Assert :
            Assert.That(actualDraw, Is.EqualTo(actualDraw));
        }

        //[Test]
        //[ExpectedException(typeof(ArgumentOutOfRangeException))]
        //public void PaintSquare_WithUpperLeftCornerOutOfRange()
        //{
        //}

        [Test]
        public void PaintRow()
        {
            // Arrange :
            var painting = new Painting(5, 2);
            var paintingMachine = new PaintingMachine(painting);
            var expectedDraw =
                "....." + Environment.NewLine +
                ".####";

            // Act :
            paintingMachine.PaintLine(1, 1, 1, 4);
            var actualDraw = painting.ToString();

            // Assert :
            Assert.That(actualDraw, Is.EqualTo(actualDraw));
        }

        [Test]
        public void PaintColumn()
        {
            // Arrange :
            var painting = new Painting(2, 5);
            var paintingMachine = new PaintingMachine(painting);
            var expectedDraw =
                "#." + Environment.NewLine +
                "#." + Environment.NewLine +
                "#." + Environment.NewLine +
                ".." + Environment.NewLine +
                "..";

            // Act :
            paintingMachine.PaintLine(0, 0, 2, 0);
            var actualDraw = painting.ToString();

            // Assert :
            Assert.That(actualDraw, Is.EqualTo(actualDraw));
        }
    }
}
