using System;
using System.Runtime.Serialization;

namespace Locate64.LocateDB.Reader
{
    [Serializable]
    public class IncompatibleArchiveException
        : ArchiveException
    {
        public IncompatibleArchiveException()
        {
        }

        public IncompatibleArchiveException(string message)
            : base(message)
        {
        }

        public IncompatibleArchiveException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected IncompatibleArchiveException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
