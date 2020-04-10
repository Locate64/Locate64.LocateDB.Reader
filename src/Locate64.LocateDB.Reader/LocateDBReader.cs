using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

[assembly:InternalsVisibleTo("Locate64.LocateDB.Reader.Tests")]

namespace Locate64.LocateDB.Reader
{
    /// <summary>
    /// Locate32 DB file format compatible reader.
    /// </summary>
    public sealed class LocateDBReader : IDisposable
    {
        private readonly bool leaveOpen;
        private BinaryReader binaryReader;
        private Stream inputStream;

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

        /// <summary>
        /// Finalizes an instance of the <see cref="LocateDBReader" /> class.
        /// </summary>
        ~LocateDBReader()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets a value indicating whether this instance has been disposed or not.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Disposes the instance if not disposed yet, optionally disposing the assigned input stream.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Traverse the tree map, returning each individually found entry (root dir, dir or file).
        ///
        /// <remarks>
        /// Will throw if the data is corrupted or when the object was disposed while still traversing.
        /// Do not forget to dispose of the reader and close streams yourself if leaveOpen is false when you stop
        /// traversing before it finished yielding all results.
        /// </remarks>
        /// </summary>
        /// <returns>Enumerable tree map.</returns>
        public IEnumerable<DBEntry> Traverse()
        {
            var header = DBHeader.ReadFrom(binaryReader);

            // Always return the header as the first entry
            yield return header;

            // var positionBeforeRead = inputStream.Position;

            var currentRootDirectory = DBRootDirectoryEntry.ReadFrom(binaryReader);

            // var readLength = inputStream.Position - positionBeforeRead;

            int depth = 0;
            var dirs = new Stack<DBDirectoryEntry>();
            var currentPathStack = new Stack<string>(260);

            while (currentRootDirectory != null)
            {
                string currentPath;
                currentPathStack.Push(currentPath = currentRootDirectory.Path + @"\");

                currentRootDirectory.FullName = currentPath;

                yield return currentRootDirectory;

                var typeAndAttributes = (DBEntryAttributes)binaryReader.ReadByte();

                while (typeAndAttributes != 0 || depth > 0)
                {
                    if (typeAndAttributes.HasFlag(DBEntryAttributes.Directory))
                    {
                        var directoryEntry = DBDirectoryEntry.ReadFrom(binaryReader, typeAndAttributes);

                        currentPath += @"\" + directoryEntry.DirectoryName;

                        directoryEntry.ParentDirectory = depth == 0 ? null : dirs.Peek();
                        directoryEntry.RootDirectory = currentRootDirectory;
                        directoryEntry.FullName = currentPath;

                        depth++;

                        dirs.Push(directoryEntry);

                        yield return directoryEntry;
                    }
                    else if (typeAndAttributes != 0)
                    {
                        var fileEntry = DBFileEntry.ReadFrom(binaryReader, typeAndAttributes);

                        fileEntry.ParentDirectory = depth == 0 ? null : dirs.Peek();
                        fileEntry.RootDirectory = currentRootDirectory;
                        fileEntry.FullName = currentPath + @"\" + fileEntry.FileName;

                        yield return fileEntry;
                    }
                    else
                    {
                        depth--;
                        var poppedDir = dirs.Pop();

                        currentPath = poppedDir.ParentDirectory?.FullName ?? poppedDir.RootDirectory?.FullName ?? string.Empty;
                    }

                    typeAndAttributes = (DBEntryAttributes)binaryReader.ReadByte();
                }

                var nullByte = binaryReader.ReadByte(); // normally its a dword byte but we should already have read a zero byte before

                if (nullByte != 0)
                {
                    throw new ArchiveException($"Was expecting to read a final null-byte, read {nullByte} instead.");
                }

                // positionBeforeRead = inputStream.Position;

                currentPathStack.Pop();
                currentRootDirectory = DBRootDirectoryEntry.ReadFrom(binaryReader);

                // readLength = inputStream.Position - positionBeforeRead;
            }
        }

        private void Dispose(bool disposing)
        {
            if (!disposing || IsDisposed)
            {
                return;
            }

            try
            {
                binaryReader?.Dispose();

                if (!leaveOpen)
                {
                    inputStream?.Close();
                }
            }
            finally
            {
                IsDisposed = true;
                binaryReader = null;
                inputStream = null;
            }
        }
    }
}
