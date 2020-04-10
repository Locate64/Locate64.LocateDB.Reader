namespace Locate64.LocateDB.Reader
{
    public enum DBRootType : byte
    {
        /// <summary>
        /// Unknown root type.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Fixed root type.
        /// </summary>
        Fixed = 0x10,

        /// <summary>
        /// Removable root type.
        /// </summary>
        Removable = 0x20,

        /// <summary>
        /// CDRom root type.
        /// </summary>
        CDRom = 0x30,

        /// <summary>
        /// Remote root type.
        /// </summary>
        Remote = 0x40,

        /// <summary>
        /// Ramdisk root type.
        /// </summary>
        Ramdisk = 0x50,

        /// <summary>
        /// Directory root type.
        /// </summary>
        Directory = 0xF0,
    }
}
