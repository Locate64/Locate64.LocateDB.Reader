using System;
using NUnit.Framework;

namespace Locate64.LocateDB.Reader.Tests
{
    public class DBDWordTests
    {
        [Test]
        [TestCase((uint)12345678, (ushort)24910)]
        [TestCase((uint)15678, (ushort)15678)]
        [TestCase(UInt32.MinValue, (ushort)0)]
        [TestCase(UInt32.MaxValue, (ushort)65535)]
        public void Verify_Low_Matches(uint value, ushort expectedLowValue)
        {
            var dwordValue = new DBDWord(value);

            var lowValue = dwordValue.Low;

            Assert.AreEqual(expectedLowValue, lowValue);
        }

        [Test]
        [TestCase((uint)12345678, (ushort)188)]
        [TestCase((uint)15678, (ushort)0)]
        [TestCase(UInt32.MinValue, (ushort)0)]
        [TestCase(UInt32.MaxValue, (ushort)65535)]
        public void Verify_High_Matches(uint value, ushort expectedHighValue)
        {
            var dwordValue = new DBDWord(value);

            var highValue = dwordValue.High;

            Assert.AreEqual(expectedHighValue, highValue);
        }

        [Test]
        [TestCase((uint)12345678)]
        [TestCase((uint)15678)]
        [TestCase(UInt32.MinValue)]
        [TestCase(UInt32.MaxValue)]
        public void Verify_ImplicitFromUint(uint testValue)
        {
            var leftSideValue = (DBDWord)testValue;
            
            Assert.AreEqual(leftSideValue.Value, testValue);
        }

        [Test]
        [TestCase((uint)12345678)]
        [TestCase((uint)15678)]
        [TestCase(UInt32.MinValue)]
        [TestCase(UInt32.MaxValue)]
        public void Verify_ImplicitToUint(uint testValue)
        {
            var leftSideValue = (DBDWord)testValue;
            
            Assert.AreEqual(0 + leftSideValue, testValue);
        }

        [Test]
        [TestCase((uint)12345678)]
        [TestCase((uint)15678)]
        [TestCase(UInt32.MinValue)]
        [TestCase(UInt32.MaxValue)]
        public void Verify_Equality_Equals(uint testValue)
        {
            var leftSideValue = (DBDWord)testValue;
            var rightSideValue = (DBDWord)testValue;
            
            Assert.AreEqual(leftSideValue, rightSideValue);
        }

        [Test]
        [TestCase((uint)12345678)]
        [TestCase((uint)15678)]
        [TestCase(UInt32.MinValue)]
        [TestCase(UInt32.MaxValue)]
        public void Verify_Equality_Operator(uint testValue)
        {
            var leftSideValue = (DBDWord)testValue;
            var rightSideValue = (DBDWord)testValue;
            
            Assert.IsTrue(leftSideValue == rightSideValue);
        }

        [Test]
        [TestCase((uint)12345678)]
        [TestCase((uint)15678)]
        [TestCase(UInt32.MinValue)]
        [TestCase(UInt32.MaxValue)]
        public void Verify_Inequality_Operator(uint testValue)
        {
            var leftSideValue = (DBDWord)testValue;
            var rightSideValue = (DBDWord)testValue;
            
            Assert.IsFalse(leftSideValue != rightSideValue);
        }

        [Test]
        [TestCase((uint)12345678)]
        [TestCase((uint)15678)]
        [TestCase(UInt32.MinValue)]
        [TestCase(UInt32.MaxValue)]
        public void Verify_GetHashCode_Equality(uint testValue)
        {
            var leftSideValue = (DBDWord)testValue;
            var rightSideValue = (DBDWord)testValue;
            
            Assert.AreEqual(leftSideValue.GetHashCode(), rightSideValue.GetHashCode());
        }
    }
}
