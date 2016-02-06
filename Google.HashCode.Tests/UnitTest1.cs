using System;
using NUnit.Framework;
using Google.HashCode.ConsoleApplication;
using System.Diagnostics.CodeAnalysis;

namespace Google.HashCode.Tests
{
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        [ExcludeFromCodeCoverage]
        public void TestMethod1_WithTrue()
        {
            // Arrange
            var c = new Class1();
            var expected = "I";

            // Act
            var actual = c.Method1(true);

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        [ExcludeFromCodeCoverage]
        public void TestMethod1_WithFalse()
        {
            // Arrange
            var c = new Class1();
            var expected = "O";

            // Act
            var actual = c.Method1(false);

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        [ExcludeFromCodeCoverage]
        public void TestMethod2()
        {
            // Just a test...
            Assert.That(1 + 1, Is.EqualTo(2));
        }
    }
}
