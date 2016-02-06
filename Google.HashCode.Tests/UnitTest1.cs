using System;
using NUnit.Framework;

namespace Google.HashCode.Tests
{
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        public void TestMethod1()
        {
            // Just a test...
            Assert.IsTrue(true);
        }

        [Test]
        public void TestMethod2()
        {
            // Just a test...
            Assert.That(1 + 1, Is.EqualTo(2));
        }
    }
}
