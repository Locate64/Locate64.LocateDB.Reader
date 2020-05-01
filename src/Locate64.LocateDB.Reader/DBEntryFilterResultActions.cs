using System;

namespace Locate64.LocateDB.Reader
{
    [Flags]
    public enum DBEntryFilterResultActions
    {
        /// <summary>
        /// Excludes nothing.
        /// </summary>
        ExcludeNothing = 0,

        /// <summary>
        /// Excludes the entry from being returned.
        /// </summary>
        ExcludeSelf = 1,

        /// <summary>
        /// Excludes the entry its children to be returned.
        /// </summary>
        ExcludeChildren = 2,
    }
}
