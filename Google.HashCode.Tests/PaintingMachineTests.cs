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
    }
}
