using System;
using System.IO;
using System.Text;

namespace Locate64.LocateDB.Reader
{
    /// <summary>
    /// This class represents a DB header.
    /// </summary>
    public sealed class DBHeader : DBEntry
    {
        public DBHeader()
            : base(DBEntryType.Header)
        {
        }

        public DBFileTime CreationTime { get; set; } // DWORD, FILETIME structure

        public string Creator { get; set; } // null-terminated utf16 or ansi string

        public string Description { get; set; } // null-terminated utf16 or ansi string

        public string ExtraInformation1 { get; set; } // null-terminated utf16 or ansi string

        public string ExtraInformation2 { get; set; } // null-terminated utf16 or ansi string

        public DBArchiveAttributes Flags { get; set; } // BYTE, 01h = long file names, 0h = OEM charset, 10h = ANSI charset, 20h = Unicode charset

        public string Marker { get; set; } // 8xBYTE, LOCATEDB (ANSI)

        public uint NumberOfDirectories { get; set; } // DWORD

        public uint NumberOfFiles { get; set; } // DWORD

        public uint RemainingExtraBytes { get; set; } // DWORD, how many bytes should ignored to skip the rest of header

        public string Version { get; set; } // 2xBYTE, 20

        public override string ToString()
        {
            return $"{nameof(Marker)}: {Marker}, {nameof(Version)}: {Version}, {nameof(Flags)}: {Flags}, {nameof(RemainingExtraBytes)}: {RemainingExtraBytes}, {nameof(Creator)}: {Creator}, {nameof(Description)}: {Description}, {nameof(ExtraInformation1)}: {ExtraInformation1}, {nameof(ExtraInformation2)}: {ExtraInformation2}, {nameof(CreationTime)}: {CreationTime}, {nameof(NumberOfFiles)}: {NumberOfFiles}, {nameof(NumberOfDirectories)}: {NumberOfDirectories}";
        }

        internal static DBHeader ReadFrom(BinaryReader reader)
        {
            var marker = Encoding.UTF8.GetString(reader.ReadBytes(8));
            var version = Encoding.UTF8.GetString(reader.ReadBytes(2));
            var flags = (DBArchiveAttributes)reader.ReadByte();

            if (marker != "LOCATEDB")
            {
                throw new IncompatibleArchiveException("LOCATEDB header marker not found.");
            }

            if (version != "20")
            {
                throw new IncompatibleArchiveException($"Header version 20 expected but found {version}.");
            }

            if (!flags.HasFlag(DBArchiveAttributes.LongFileName))
            {
                throw new IncompatibleArchiveException($"Archives with long file names disabled are currently not supported.");
            }

            if (!flags.HasFlag(DBArchiveAttributes.UnicodeCharset))
            {
                throw new IncompatibleArchiveException($"Only unicode archives are currently supported.");
            }

            var header = new DBHeader
            {
                Marker = marker,
                Version = version,
                Flags = flags,
                RemainingExtraBytes = reader.ReadUInt32(),
                Creator = reader.ReadNullTerminatedUtf16String(),
                Description = reader.ReadNullTerminatedUtf16String(),
                ExtraInformation1 = reader.ReadNullTerminatedUtf16String(),
                ExtraInformation2 = reader.ReadNullTerminatedUtf16String(),
                CreationTime = (DBDWord)reader.ReadUInt32(),
                NumberOfFiles = reader.ReadUInt32(),
                NumberOfDirectories = reader.ReadUInt32(),
            };

            return header;
        }
    }
}
