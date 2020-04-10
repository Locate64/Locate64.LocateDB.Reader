namespace Locate64.LocateDB.Reader
{
    public enum DBEntryType : byte
    {
        /// <summary>
        /// A header entry.
        /// </summary>
        Header,

        /// <summary>
        /// A root directory entry.
        /// </summary>
        RootDirectory,

        /// <summary>
        /// A directory entry.
        /// </summary>
        Directory,

        /// <summary>
        /// A file entry.
        /// </summary>
        File,
    }
}
