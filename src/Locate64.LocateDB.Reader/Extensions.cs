using System.IO;
using System.Text;

namespace Locate64.LocateDB.Reader
{
    internal static class Extensions
    {
        /// <summary>
        /// Read a null terminated UTF16 string from the given binary reader.
        /// </summary>
        /// <param name="binaryReader">BinaryReader used to read the string from. Throws NullReferenceException if binaryReader is null.</param>
        /// <param name="stringBuilder">StringBuilder instance used to build the final string. Will be Clear()ed first.</param>
        /// <returns>The read string.</returns>
        public static string ReadNullTerminatedUtf16String(this BinaryReader binaryReader, StringBuilder stringBuilder)
        {
            char ch;

            stringBuilder.Clear();

            while ((ch = (char)binaryReader.ReadInt16()) != 0)
            {
                stringBuilder.Append(ch);
            }

            return stringBuilder.ToString();
        }
    }
}
