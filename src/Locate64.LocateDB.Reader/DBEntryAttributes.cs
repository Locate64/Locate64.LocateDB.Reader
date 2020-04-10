using System;

namespace Locate64.LocateDB.Reader
{
    /// <summary>
    /// Possible DB entry attributes.
    /// </summary>
    [Flags]
    public enum DBEntryAttributes : byte
    {
        /// <summary>
        /// An Unknown entry.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// A Directory entry.
        /// </summary>
        Directory = 0x80,

        /// <summary>
        /// A Hidden entry.
        /// </summary>
        Hidden = 0x01,

        /// <summary>
        /// A Read Only entry.
        /// </summary>
        ReadOnly = 0x02,

        /// <summary>
        /// An Archive entry.
        /// </summary>
        Archive = 0x04,

        /// <summary>
        /// A System entry.
        /// </summary>
        System = 0x08,

        /// <summary>
        /// A symlink entry.
        /// </summary>
        Symlink = 0x20,

        /// <summary>
        /// A File entry.
        /// </summary>
        File = 0x10,

        /// <summary>
        /// A Junction Point entry.
        /// </summary>
        JunctionPoint = 0x40,
    }
}
