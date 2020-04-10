using System.Runtime.InteropServices;

namespace Locate64.LocateDB.Reader
{
    /// <summary>
    /// Structure representing a DWord with easy access to its low and high bits.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct DBDWord
    {
        /// <summary>
        /// Raw dword value.
        /// </summary>
        [FieldOffset(0)]
        public readonly uint Value;

        /// <summary>
        /// Access low bits value.
        /// </summary>
        [FieldOffset(0)]
        public readonly ushort Low;

        /// <summary>
        /// Access the high bits value.
        /// </summary>
        [FieldOffset(2)]
        public readonly ushort High;

        /// <summary>
        /// Initializes a new instance of the <see cref="DBDWord"/> struct.
        /// </summary>
        /// <param name="value">uint value to initialize from.</param>
        public DBDWord(uint value)
            : this()
        {
            Value = value;
        }

        public static implicit operator DBDWord(uint value) => new DBDWord(value);

        public static implicit operator uint(DBDWord value) => value.Value;

        public static bool operator ==(DBDWord left, DBDWord right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DBDWord left, DBDWord right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return $"{Value}";
        }

        public bool Equals(DBDWord other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is DBDWord other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (int)Value;
        }
    }
}
