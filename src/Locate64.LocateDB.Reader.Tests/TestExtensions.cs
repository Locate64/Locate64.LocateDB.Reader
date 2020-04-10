using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Locate64.LocateDB.Reader.Tests
{
    internal static class TestExtensions
    {
        public static void WriteNullTerminatedUtf16String(this MemoryStream memoryStream, string value)
        {
            using var binaryWriter = new BinaryWriter(memoryStream, Encoding.Unicode, true);
            
            // Write the value as a char array
            binaryWriter.Write(value.ToCharArray());
            
            // Write null-terminator
            binaryWriter.Write((byte)0);
            binaryWriter.Write((byte)0);
        }
    }
}
