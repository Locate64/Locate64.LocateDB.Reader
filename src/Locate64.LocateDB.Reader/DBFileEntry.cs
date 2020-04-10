using System.IO;

namespace Locate64.LocateDB.Reader
{
    /// <summary>
    /// This class represents a DB file entry.
    /// </summary>
    public sealed class DBFileEntry : DBEntry
    {
        public DBFileEntry()
            : base(DBEntryType.File)
        {
        }

        /// <summary>
        /// Gets the attributes for this entry.
        /// </summary>
        public DBEntryAttributes Attributes { get; private set; }

        /// <summary>
        /// Gets the creation time.
        /// </summary>
        public DBFileTime CreationTime { get; private set; }

        /// <summary>
        /// Gets the index of the file extension inside the file name.
        /// </summary>
        public byte FileExtensionIndex { get; private set; }

        /// <summary>
        /// Gets the filename.
        /// </summary>
        public string FileName { get; private set; }

        // public DBDWord FileSizeLo { get; private set; }

        // public ushort FileSizeHi { get; private set; }

        /// <summary>
        /// Gets the file size in bytes.
        /// </summary>
        public ulong FileSize { get; private set; }

        /// <summary>
        /// Gets the full path of the file.
        /// </summary>
        public string FullName { get; internal set; }

        /// <summary>
        /// Gets the last accessed date.
        /// </summary>
        public DBFileTime LastAccessedDate { get; private set; }

        /// <summary>
        /// Gets the modification time.
        /// </summary>
        public DBFileTime ModificationTime { get; private set; }

        /// <summary>
        /// Gets the parent directory.
        /// </summary>
        public DBDirectoryEntry ParentDirectory { get; internal set; }

        /// <summary>
        /// Gets the root directory.
        /// </summary>
        public DBRootDirectoryEntry RootDirectory { get; internal set; }

        /// <summary>
        /// Gets the length of the file name in bytes.
        /// </summary>
        internal byte FileNameLength { get; private set; }

        public override string ToString()
        {
            return $"{nameof(Attributes)}: {Attributes}, {nameof(FileNameLength)}: {FileNameLength}, {nameof(FileExtensionIndex)}: {FileExtensionIndex}, {nameof(FileName)}: {FileName},  {nameof(FullName)}: {FullName}, {nameof(FileSize)}: {FileSize}, {nameof(ModificationTime)}: {ModificationTime}, {nameof(CreationTime)}: {CreationTime}, {nameof(LastAccessedDate)}: {LastAccessedDate}";
        }

        internal static DBFileEntry ReadFrom(BinaryReader reader, DBEntryAttributes attributes)
        {
            var entry = new DBFileEntry
            {
                Attributes = attributes,
                FileNameLength = reader.ReadByte(),
                FileExtensionIndex = reader.ReadByte(),
                FileName = reader.ReadNullTerminatedUtf16String(),
            };

            var fileSizeLo = reader.ReadUInt32();

            var fileSizeHi = (ulong)reader.ReadUInt16();

            entry.FileSize = fileSizeHi << 32 | fileSizeLo;

            entry.ModificationTime = (DBDWord)reader.ReadUInt32();

            entry.CreationTime = (DBDWord)reader.ReadUInt32();

            entry.LastAccessedDate = (DBDWord)reader.ReadUInt32();

            return entry;
        }
    }
}
