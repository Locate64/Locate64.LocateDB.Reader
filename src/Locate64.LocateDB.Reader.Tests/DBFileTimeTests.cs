using System;
using NUnit.Framework;

namespace Locate64.LocateDB.Reader.Tests
{
    public class DBFileTimeTests
    {
        [Test]
        public void Verify_OutOfRange_HasOutOfRangeFlagSet()
        {
            var testedValue = new DBFileTime(ushort.MaxValue, 1, 1, 1, 1, 1);
            
            Assert.IsTrue(testedValue.IsOutOfRange);
        }

        [Test]
        public void Verify_InRange_HasOutOfRangeFlagNotSet()
        {
            var testedValue = new DBFileTime(2050, 1, 1, 1, 1, 1);
            
            Assert.IsFalse(testedValue.IsOutOfRange);
        }

        [Test]
        public void ToDateTime_InRange_KindIsLocal()
        {
            var testedValue = new DBFileTime(2050, 1, 1, 1, 1, 1);

            var testValue = testedValue.ToDateTime();
            
            Assert.AreEqual(DateTimeKind.Local, testValue.Kind);
        }

        [Test]
        [TestCase((ushort)2020, (ushort)12, (ushort)20, (ushort)21, (ushort)50, (ushort)40)]
        [TestCase((ushort)2020, (ushort)2, (ushort)29, (ushort)1, (ushort)59, (ushort)58)]
        public void Verify_ToDateTime_Matches(ushort year, ushort month, ushort day, ushort hour, ushort minute, ushort second)
        {
            var testedValue = new DBFileTime(year, month, day, hour, minute, second);

            var testValue = testedValue.ToDateTime();
            
            Assert.AreEqual(year, testValue.Year);
            Assert.AreEqual(month, testValue.Month);
            Assert.AreEqual(day, testValue.Day);
            Assert.AreEqual(hour, testValue.Hour);
            Assert.AreEqual(second, testValue.Second);
        }

        [Test]
        [TestCase((ushort)2020, (ushort)12, (ushort)20, (ushort)21, (ushort)50, (ushort)40)]
        [TestCase((ushort)2020, (ushort)2, (ushort)29, (ushort)1, (ushort)59, (ushort)58)]
        public void Verify_GetHashCode_Matches(ushort year, ushort month, ushort day, ushort hour, ushort minute, ushort second)
        {
            var leftSideValue = new DBFileTime(year, month, day, hour, minute, second);
            var rightSideValue = new DBFileTime(year, month, day, hour, minute, second);

            Assert.AreEqual(leftSideValue.GetHashCode(), rightSideValue.GetHashCode());
        }
    }
}
