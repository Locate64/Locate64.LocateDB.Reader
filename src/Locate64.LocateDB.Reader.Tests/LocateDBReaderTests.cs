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
    }
}
