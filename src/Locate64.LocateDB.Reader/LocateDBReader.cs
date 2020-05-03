using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Locate64.LocateDB.Reader
{
    public sealed class LocateDBReader : IDisposable
    {
        private readonly bool leaveOpen;
        private readonly Stack<string> currentPathStack = new Stack<string>(64);
        private readonly Stack<DBDirectoryEntry> dirStack = new Stack<DBDirectoryEntry>(64);
        private readonly StringBuilder strBuilder = new StringBuilder(255);
        private BinaryReader binaryReader;
        private Stream inputStream;
        private long positionBeforeReadRootDirectory;
        private long positionBeforeReadDirectory;
        private DBRootDirectoryEntry currentRootDirectory;
        private int depth;
        private ReaderState state = ReaderState.ReadHeaderNext;
        private string currentPath = string.Empty;
        private DBEntryAttributes typeAndAttributes = DBEntryAttributes.Unknown;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocateDBReader" /> class.
        /// </summary>
        /// <param name="inputStream">Input stream to read from. Cannot be null and must support reading.</param>
        public LocateDBReader(Stream inputStream)
            : this(inputStream, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocateDBReader" /> class.
        /// </summary>
        /// <param name="inputStream">Input stream to read from. Cannot be null and must be readable.</param>
        /// <param name="leaveOpen">
        /// true to leave the input stream open after the LocateDBReader object is disposed; otherwise,
        /// false.
        /// </param>
        public LocateDBReader(Stream inputStream, bool leaveOpen)
        {
            this.inputStream = inputStream ?? throw new ArgumentNullException(nameof(inputStream));

            if (!inputStream.CanRead)
            {
                throw new ArgumentOutOfRangeException(nameof(inputStream), "The input stream must be readable.");
            }

            this.leaveOpen = leaveOpen;
            binaryReader = new BinaryReader(this.inputStream, Encoding.UTF8, true);
        }

        private enum ReaderState
        {
            ReadHeaderNext,

            ReadRootDirectoryNext,

            ReadFileOrDirectoryNext,

            EndOfFile,
        }

        /// <summary>
        /// Gets the last read entry. If nothing was read yet, or we finished reading all entries this will return null.
        /// </summary>
        public DBEntry LastReadEntry { get; private set; }

        /// <summary>
        /// Gets the read header. If nothing was read yet this will return null.
        /// </summary>
        public DBHeader Header { get; private set; }

        public void Dispose()
        {
            if (!leaveOpen)
            {
                inputStream?.Close();
            }

            inputStream = null;
            binaryReader = null;
        }

        /// <summary>
        /// Read the next entry.
        /// </summary>
        /// <returns>The read DBEntry if any; null otherwise.</returns>
        public DBEntry Read()
        {
            switch (state)
            {
                case ReaderState.EndOfFile:
                {
                    return null;
                }

                case ReaderState.ReadHeaderNext:
                {
                    LastReadEntry = Header = DBHeader.ReadFrom(binaryReader, strBuilder);

                    state = ReaderState.ReadRootDirectoryNext;

                    return LastReadEntry;
                }

                case ReaderState.ReadRootDirectoryNext:
                {
                    positionBeforeReadRootDirectory = inputStream.Position;

                    LastReadEntry = currentRootDirectory = DBRootDirectoryEntry.ReadFrom(binaryReader, strBuilder);

                    if (currentRootDirectory == null)
                    {
                        state = ReaderState.EndOfFile;
                        return null;
                    }

                    currentPathStack.Push(currentPath = currentRootDirectory.Path);

                    currentRootDirectory.FullName = currentPath;

                    state = ReaderState.ReadFileOrDirectoryNext;

                    return LastReadEntry;
                }

                case ReaderState.ReadFileOrDirectoryNext:
                {
                    typeAndAttributes = (DBEntryAttributes)binaryReader.ReadByte();

                    if (typeAndAttributes != 0 || depth > 0)
                    {
                        if (typeAndAttributes.HasFlag(DBEntryAttributes.Directory))
                        {
                            positionBeforeReadDirectory = inputStream.Position;

                            var directoryEntry = DBDirectoryEntry.ReadFrom(binaryReader, typeAndAttributes, strBuilder);

                            currentPath += (depth == 0 ? string.Empty : @"\") + directoryEntry.DirectoryName;

                            directoryEntry.ParentDirectory = depth == 0 ? null : dirStack.Peek();
                            directoryEntry.RootDirectory = currentRootDirectory;
                            directoryEntry.FullName = currentPath;

                            depth++;

                            dirStack.Push(directoryEntry);

                            return LastReadEntry = directoryEntry;
                        }

                        if (typeAndAttributes != 0)
                        {
                            var fileEntry = DBFileEntry.ReadFrom(binaryReader, typeAndAttributes, strBuilder);

                            fileEntry.ParentDirectory = depth == 0 ? null : dirStack.Peek();
                            fileEntry.RootDirectory = currentRootDirectory;
                            fileEntry.FullName = currentPath + (depth == 0 ? string.Empty : @"\") + fileEntry.FileName;

                            return LastReadEntry = fileEntry;
                        }

                        depth--;
                        var poppedDir = dirStack.Pop();

                        currentPath = poppedDir.ParentDirectory?.FullName ?? poppedDir.RootDirectory?.FullName ?? string.Empty;

                        return Read();
                    }

                    var nullByte = binaryReader.ReadByte(); // normally its a dword byte but we should already have read a zero byte before

                    if (nullByte != 0)
                    {
                        throw new ArchiveException($"Was expecting to read a final null-byte, read {nullByte} instead.");
                    }

                    currentPathStack.Pop();

                    state = ReaderState.ReadRootDirectoryNext;

                    return Read();
                }
            }

            LastReadEntry = null;
            return null;
        }

        /// <summary>
        /// Skip reading the children of the last read entry.
        /// </summary>
        /// <returns>True if the skip succeeded; false otherwise.</returns>
        public bool SkipChildren()
        {
            if (LastReadEntry is DBRootDirectoryEntry)
            {
                // Skip the rest
                inputStream.Position += (positionBeforeReadRootDirectory - inputStream.Position) + currentRootDirectory.DataLength + 4;

                currentPathStack.Pop();

                positionBeforeReadRootDirectory = inputStream.Position;

                state = ReaderState.ReadRootDirectoryNext;

                return true;
            }

            if (LastReadEntry is DBDirectoryEntry directoryEntry && depth > 0)
            {
                // Skip the rest
                inputStream.Position += (positionBeforeReadDirectory - inputStream.Position) + directoryEntry.DataLength;

                var poppedDir = dirStack.Count > 0 ? dirStack.Pop() : null;

                currentPath = poppedDir?.ParentDirectory?.FullName ?? directoryEntry?.RootDirectory?.FullName ?? string.Empty;

                depth--;

                return true;
            }

            return false;
        }
    }
}
