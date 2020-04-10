using System;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace Locate64.LocateDB.Reader.Tests
{
    public class ExtensionTests
    {
        [Test]
        public void ReadNullTerminatedUtf16String_BinaryReaderIsNull_ThrowNullReferenceException()
        {
            Assert.Catch<NullReferenceException>(() => Extensions.ReadNullTerminatedUtf16String(null));
        }
        
        [Test]
        [TestCase("Hello World")]
        [TestCase("Héllo Wörld")]
        [TestCase("Héllo 😀")]
        public void ReadNullTerminatedUtf16String_BinaryReaderHasValidStringToRead_ReturnsCorrectString(string testValue)
        {
            /* Preparation */

            // Prepare a stream
            var memoryStream = new MemoryStream();

            // Write the test value into it
            memoryStream.WriteNullTerminatedUtf16String(testValue);

            // Reset the position
            memoryStream.Position = 0;

            // Create the binary reader that will read from the prepared memory stream
            var binaryReader = new BinaryReader(memoryStream, Encoding.Unicode, true);

            /* Execution */

            var readValue = binaryReader.ReadNullTerminatedUtf16String();

            /* Assertion */

            Assert.AreEqual(testValue, readValue);
        }
    }
}
