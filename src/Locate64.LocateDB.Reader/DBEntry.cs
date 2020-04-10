namespace Locate64.LocateDB.Reader
{
    /// <summary>
    /// Base class for all different types of archive entries, including its header.
    /// </summary>
    public abstract class DBEntry
    {
        internal DBEntry(DBEntryType type)
        {
            Type = type;
        }

        /// <summary>
        /// Gets the type of the entry.
        /// </summary>
        public DBEntryType Type { get; }
    }
}
