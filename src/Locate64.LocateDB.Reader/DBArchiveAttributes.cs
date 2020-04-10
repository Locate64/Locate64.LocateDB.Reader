using System;

namespace Locate64.LocateDB.Reader
{
    [Flags]
    public enum DBArchiveAttributes : byte
    {
        /// <summary>
        /// Long file name support enabled.
        /// </summary>
        LongFileName = 0x01,

        /// <summary>
        /// Using the Ansi charset
        /// </summary>
        AnsiCharset = 0x10,

        /// <summary>
        /// Using the Unicode charset.
        /// </summary>
        UnicodeCharset = 0x20,
    }
}
