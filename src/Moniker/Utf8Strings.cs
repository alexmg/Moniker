#nullable enable

using System;
using System.Diagnostics;
using System.Text;

namespace Moniker;

readonly ref struct Utf8String(ReadOnlySpan<byte> bytes, int charCount)
{
    public readonly ReadOnlySpan<byte> Bytes = bytes;
    public readonly int CharCount = charCount;

    public int GetChars(Span<char> chars) => Encoding.UTF8.GetChars(this.Bytes, chars);

    public override string ToString() => Encoding.UTF8.GetString(this.Bytes);

    // Equality members

    public bool Equals(ReadOnlySpan<byte> other) => this.Bytes.SequenceEqual(other);

    public static bool operator ==(Utf8String left, ReadOnlySpan<byte> right) => left.Equals(right);
    public static bool operator !=(Utf8String left, ReadOnlySpan<byte> right) => !left.Equals(right);

    // Unsupported equality members because this type cannot be boxed

    public override int GetHashCode() => throw new NotSupportedException();
    public override bool Equals(object? obj) => throw new NotSupportedException();

    // Implicit conversions

    public static implicit operator ReadOnlySpan<byte>(Utf8String data) => data.Bytes;
}

readonly ref struct Utf8Strings
{
    public readonly ReadOnlySpan<byte> Data;
    public readonly ReadOnlySpan<int> Offsets;
    public readonly ReadOnlySpan<byte> CharCounts;
    public readonly int Count;

    public Utf8Strings(int count, ReadOnlySpan<byte> data, ReadOnlySpan<int> offsets, ReadOnlySpan<byte> charCounts)
    {
        Debug.Assert(offsets.Length == count + 1);
        Debug.Assert(offsets[^1] == data.Length);
        Debug.Assert(charCounts.Length == count);

        Count = count;
        Data = data;
        Offsets = offsets;
        CharCounts = charCounts;
    }

    public Utf8String this[int index] => new(this.Data[Offsets[index]..this.Offsets[index + 1]], CharCounts[index]);

    public override string ToString() => $"{{ Count = {this.Count}, Size = {this.Data.Length} }}";

    public Enumerator GetEnumerator() => new(this);

    public ref struct Enumerator(Utf8Strings strings)
    {
        int index = -1;
        readonly Utf8Strings strings = strings;

        /// <remarks>
        /// Behaviour is undefined if <see cref="MoveNext"/> has never been called or returned
        /// <see langword="false"/>.
        /// </remarks>
        public Utf8String Current => this.strings[index];

        public bool MoveNext()
        {
            var newIndex = this.index + 1;
            if (newIndex == this.strings.Count)
                return false;
            this.index = newIndex;
            return true;
        }
    }
}
