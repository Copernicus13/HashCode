using System;
using System.Drawing;
using Google.HashCode.ConsoleApplication;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;


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


        [Test]
        public void CanIsValid()
        {
            var input = new StreamReader(@"D:\Development\Google.HashCode\input\small.in");
            //Console.SetIn(input);
            var program = new Program(false, input);
            //var lines = new List<string>
            //{
            //    "9",
            //    "0 L 0 0 1 ",
            //    "0 L 0 1 1 ",
            //    "0 D 0 0 1 ",
            //    "0 L 1 2 1 ",
            //    "0 D 0 2 1 ",
            //    "1 L 1 2 1 ",
            //    "1 D 2 2 1 ",
            //    "1 L 0 0 1 ",
            //    "1 D 1 0 1 "
            //};
            var lines = new List<string>
            {
                "8",
                "0 L 0 0 1 ",
                "0 D 0 0 1 ",
                "0 L 1 2 1 ",
                "0 D 0 2 1 ",
                "1 L 0 0 3 ",
                "1 D 1 0 3 ",
                "2 L 1 2 1 ",
                "2 D 2 2 1 "
            };

            Assert.That(program.IsValid(lines), Is.EqualTo(true));
        }
    }
}