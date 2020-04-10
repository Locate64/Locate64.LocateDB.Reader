using System.IO;

namespace Locate64.LocateDB.Reader
{
    internal static class Extensions
    {
        /// <summary>
        /// Read a null terminated UTF16 string from the given binary reader.
        /// </summary>
        /// <param name="binaryReader">BinaryReader used to read the string from. Throws NullReferenceException if binaryReader is null.</param>
        /// <returns>The read string.</returns>
        public static string ReadNullTerminatedUtf16String(this BinaryReader binaryReader)
        {
            var str = string.Empty;
            char ch;

            while ((ch = (char)binaryReader.ReadInt16()) != 0)
            {
                str += ch;
            }

            return str;
        }
    }
}
