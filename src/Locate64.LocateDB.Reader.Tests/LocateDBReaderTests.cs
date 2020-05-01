using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Locate64.LocateDB.Reader.Tests
{
    public class LocateDBReaderTests
    {
        private Stream testStream1;

        [SetUp]
        public void Setup()
        {
            testStream1 = typeof(LocateDBReaderTests).Assembly.GetManifestResourceStream("Locate64.LocateDB.Reader.Tests.Resources.testset-a.dbs");
        }

        [Test]
        public void Verify_FirstEntry_IsHeader()
        {
            var reader = new LocateDBReader(testStream1, true);

            var entries = reader.Traverse();

            var firstEntry = entries.First();

            Assert.That(firstEntry, Is.TypeOf<DBHeader>());
        }

        [Test]
        public void Verify_ExcludedFolder_ProperlyExcludesSelfAndChildren()
        {
            var reader = new LocateDBReader(testStream1);

            var entries = reader.Traverse(new ExcludeRootFolder1AndChildrenFilter());

            foreach (var entry in entries)
            {
                if (entry is DBFileEntry fileEntry)
                {
                    Assert.That(fileEntry.FullName, Does.Not.StartsWith(@"I:\RootFolder1"));
                }
                else if (entry is DBDirectoryEntry dirEntry)
                {
                    Assert.That(dirEntry.FullName, Does.Not.EqualTo(@"I:\RootFolder1"));
                }
            }
        }

        [Test]
        public void Verify_SecondEntry_IsValidRootDirectory()
        {
            var reader = new LocateDBReader(testStream1, true);

            var entries = reader.Traverse();

            var secondEntry = entries.Skip(1).First();

            Assert.That(secondEntry, Is.TypeOf<DBRootDirectoryEntry>());
            Assert.That(secondEntry is DBRootDirectoryEntry, Is.True);
            Assert.That(((DBRootDirectoryEntry)secondEntry).DataLength, Is.EqualTo(986));
            Assert.That(((DBRootDirectoryEntry)secondEntry).RootType, Is.EqualTo(DBRootType.Removable));
            Assert.That(((DBRootDirectoryEntry)secondEntry).VolumeName, Is.EqualTo("LOCATE32_NTFS"));
            Assert.That(((DBRootDirectoryEntry)secondEntry).VolumeSerial, Is.EqualTo(3838827262));
            Assert.That(((DBRootDirectoryEntry)secondEntry).FileSystem, Is.EqualTo("NTFS"));
            Assert.That(((DBRootDirectoryEntry)secondEntry).NumberOfFiles, Is.EqualTo(10));
            Assert.That(((DBRootDirectoryEntry)secondEntry).NumberOfDirectories, Is.EqualTo(7));
        }

        [Test]
        public void Verify_FileEntries_ShouldNotContainDoubleBackSlashes()
        {
            var reader = new LocateDBReader(testStream1, true);

            var entries = reader.Traverse().Skip(1);

            foreach (var entry in entries.Select(e => e as DBFileEntry).Where(e => e != null))
            {
                Assert.That(entry.FullName, Does.Not.Contain("\\\\"));
            }
        }

        [Test]
        public void Verify_DirectoryEntries_ShouldNotContainDoubleBackSlashes()
        {
            var reader = new LocateDBReader(testStream1, true);

            var entries = reader.Traverse().Skip(1);

            foreach (var entry in entries.Select(e => e as DBDirectoryEntry).Where(e => e != null))
            {
                Assert.That(entry.FullName, Does.Not.Contain("\\\\"));
            }
        }

        [Test]
        public void Verify_Header_IsValid()
        {
            var reader = new LocateDBReader(testStream1, true);

            var entries = reader.Traverse();

            var header = entries.First() as DBHeader;

            Assert.That(header, Is.Not.Null);
            Assert.That(header.Creator, Is.EqualTo("Creator goes here"));
            Assert.That(header.Description, Is.EqualTo("Description goes here"));
            Assert.That(header.Marker, Is.EqualTo("LOCATEDB"));
            Assert.That(header.Version, Is.EqualTo("20"));
            Assert.That(header.Flags.HasFlag(DBArchiveAttributes.LongFileName), Is.True);
            Assert.That(header.Flags.HasFlag(DBArchiveAttributes.UnicodeCharset), Is.True);
            Assert.That(header.Flags.HasFlag(DBArchiveAttributes.AnsiCharset), Is.False);
            Assert.That(header.NumberOfFiles, Is.EqualTo(10));
            Assert.That(header.NumberOfDirectories, Is.EqualTo(7));
            Assert.That(header.CreationTime, Is.EqualTo(new DBFileTime(2020, 4, 15, 13, 45, 12)));
            Assert.That(header.RemainingExtraBytes, Is.EqualTo(336));
        }


        private class ExcludeRootFolder1AndChildrenFilter : IDBEntryFilter
        {
            public DBEntryFilterResultActions Filter(DBEntry entry)
            {
                if (entry is DBDirectoryEntry dirEntry && dirEntry.FullName.StartsWith(@"I:\RootFolder1"))
                {
                    return DBEntryFilterResultActions.ExcludeChildren | DBEntryFilterResultActions.ExcludeSelf;
                }

                return DBEntryFilterResultActions.ExcludeNothing;
            }
        }
    }
}
