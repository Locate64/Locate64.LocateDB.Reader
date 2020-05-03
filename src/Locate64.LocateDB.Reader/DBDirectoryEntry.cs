using System.IO;
using System.Text;

namespace Locate64.LocateDB.Reader
{
    /// <summary>
    /// This class represents a DB directory entry.
    /// </summary>
    public sealed class DBDirectoryEntry : DBEntry
    {
        public DBDirectoryEntry()
            : base(DBEntryType.Directory)
        {
        }

        public DBEntryAttributes Attributes { get; private set; }

        public DBFileTime CreationTime { get; private set; }

        public uint DataLength { get; private set; } // DWORD

        public string DirectoryName { get; private set; }

        public byte DirectoryNameLength { get; private set; }

        public string FullName { get; internal set; }

        public DBFileTime LastAccessedDate { get; private set; }

        public DBFileTime ModificationTime { get; private set; }

        public DBDirectoryEntry ParentDirectory { get; internal set; }

        public DBRootDirectoryEntry RootDirectory { get; internal set; }

        public override string ToString()
        {
            return $"{nameof(Attributes)}: {Attributes}, {nameof(DataLength)}: {DataLength}, {nameof(DirectoryNameLength)}: {DirectoryNameLength}, {nameof(FullName)}: {FullName}, {nameof(DirectoryName)}: {DirectoryName}, {nameof(ModificationTime)}: {ModificationTime}, {nameof(CreationTime)}: {CreationTime}, {nameof(LastAccessedDate)}: {LastAccessedDate}";
        }

        internal static DBDirectoryEntry ReadFrom(BinaryReader reader, DBEntryAttributes attributes, StringBuilder stringBuilder)
        {
            var entry = new DBDirectoryEntry
            {
                Attributes = attributes,
                DataLength = reader.ReadUInt32(),
                DirectoryNameLength = reader.ReadByte(),
                DirectoryName = reader.ReadNullTerminatedUtf16String(stringBuilder),
                ModificationTime = (DBDWord)reader.ReadUInt32(),
                CreationTime = (DBDWord)reader.ReadUInt32(),
                LastAccessedDate = (DBDWord)reader.ReadUInt32(),
            };

            return entry;
        }
    }
}
