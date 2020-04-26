using System.IO;

namespace Locate64.LocateDB.Reader
{
    /// <summary>
    /// This class represents a DB root directory entry.
    /// </summary>
    public sealed class DBRootDirectoryEntry : DBEntry
    {
        public DBRootDirectoryEntry()
            : base(DBEntryType.RootDirectory)
        {
        }

        public uint DataLength { get; set; } // DWORD

        public string FileSystem { get; set; }

        public string FullName { get; set; }

        public uint NumberOfDirectories { get; set; } // DWORD

        public uint NumberOfFiles { get; set; } // DWORD

        public string Path { get; set; }

        public DBRootType RootType { get; set; }

        public string VolumeName { get; set; }

        public uint VolumeSerial { get; set; } // DWORD

        public static DBRootDirectoryEntry ReadFrom(BinaryReader reader)
        {
            var dataLength = reader.ReadUInt32();

            if (dataLength == 0)
            {
                return null;
            }

            var entry = new DBRootDirectoryEntry
            {
                DataLength = dataLength,
                RootType = (DBRootType)reader.ReadByte(),
                Path = reader.ReadNullTerminatedUtf16String(),
                VolumeName = reader.ReadNullTerminatedUtf16String(),
                VolumeSerial = reader.ReadUInt32(),
                FileSystem = reader.ReadNullTerminatedUtf16String(),
                NumberOfFiles = reader.ReadUInt32(),
                NumberOfDirectories = reader.ReadUInt32(),
            };

            return entry;
        }

        public override string ToString()
        {
            return $"{nameof(DataLength)}: {DataLength}, {nameof(RootType)}: {RootType}, {nameof(Path)}: {Path}, {nameof(VolumeName)}: {VolumeName}, {nameof(VolumeSerial)}: {VolumeSerial}, {nameof(FileSystem)}: {FileSystem}, {nameof(NumberOfFiles)}: {NumberOfFiles}, {nameof(NumberOfDirectories)}: {NumberOfDirectories}";
        }
    }
}
