using System;
using System.Collections.Generic;
using System.Text;

namespace Locate64.LocateDB.Reader
{
    /// <summary>
    /// Interface for DB entry filters.
    /// </summary>
    public interface IDBEntryFilter
    {
        /// <summary>
        /// Let the filter decide what to do with the entry.
        /// </summary>
        /// <param name="entry">DBEntry instance to be filtered.</param>
        /// <returns>The chosen action. Some actions that do not make sense do nothing.</returns>
        DBEntryFilterResultActions Filter(DBEntry entry);
    }
}
