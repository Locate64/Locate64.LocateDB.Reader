using System;

namespace Locate64.LocateDB.Reader
{
    public struct DBFileTime
    {
        /// <summary>
        /// The day.
        /// </summary>
        public readonly int Day;

        /// <summary>
        /// The hour.
        /// </summary>
        public readonly int Hour;

        /// <summary>
        /// The minute.
        /// </summary>
        public readonly int Minute;

        /// <summary>
        /// The month.
        /// </summary>
        public readonly int Month;

        /// <summary>
        /// The second.
        /// </summary>
        public readonly int Second;

        /// <summary>
        /// The year.
        /// </summary>
        public readonly int Year;

        /// <summary>
        /// DateTime returned when trying to convert a DBFileTime value that contains an out of range date.
        /// </summary>
        private static readonly DateTime OutOfRangeDateTime = new DateTime(1970, 1, 2, 0, 0, 0, DateTimeKind.Utc).ToLocalTime();

        /// <summary>
        /// Initializes a new instance of the <see cref="DBFileTime"/> struct.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <param name="hour">The hour.</param>
        /// <param name="minute">The minute.</param>
        /// <param name="second">The second.</param>
        public DBFileTime(ushort year, ushort month, ushort day, ushort hour, ushort minute, ushort second)
        {
            Year = year;
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;
            Second = second;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DBFileTime"/> struct.
        /// </summary>
        /// <param name="fileTime">The file time stored in a DWord.</param>
        public DBFileTime(DBDWord fileTime)
        {
            Year = (fileTime.Low >> 9) + 1980;
            Month = (fileTime.Low >> 5) & 0xF;
            Day = fileTime.Low & 0x1F;

            Hour = fileTime.High >> 11;
            Minute = (fileTime.High >> 5) & 0x3F;
            Second = (fileTime.High & 0x1F) << 1;
        }

        /// <summary>
        /// Gets a value indicating whether the contained data is out of range (year exceeds 2099).
        /// </summary>
        public bool IsOutOfRange => Year > 2099;

        public static implicit operator DBFileTime(DBDWord value) => new DBFileTime(value);

        public static bool operator ==(DBFileTime left, DBFileTime right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DBFileTime left, DBFileTime right)
        {
            return !left.Equals(right);
        }

        public bool Equals(DBFileTime other)
        {
            return Year == other.Year
                   && Month == other.Month
                   && Day == other.Day
                   && Hour == other.Hour
                   && Minute == other.Minute
                   && Second == other.Second;
        }

        public override bool Equals(object obj)
        {
            return obj is DBFileTime other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Year;
                hashCode = (hashCode * 397) ^ Month;
                hashCode = (hashCode * 397) ^ Day;
                hashCode = (hashCode * 397) ^ Hour;
                hashCode = (hashCode * 397) ^ Minute;
                hashCode = (hashCode * 397) ^ Second;
                return hashCode;
            }
        }

        /// <summary>
        /// Converts the contained date and time to DateTime, making the assumption that the source date and time was stored in
        /// local time.
        /// </summary>
        /// <returns>A new DateTime value.</returns>
        public DateTime ToDateTime()
        {
            return IsOutOfRange
                ? OutOfRangeDateTime
                : new DateTime(Year, Month, Day, Hour, Minute, Second, DateTimeKind.Local);
        }

        public override string ToString()
        {
            return $"{nameof(Year)}: {Year}, {nameof(Month)}: {Month}, {nameof(Day)}: {Day}, {nameof(Hour)}: {Hour}, {nameof(Minute)}: {Minute}, {nameof(Second)}: {Second}";
        }
    }
}
