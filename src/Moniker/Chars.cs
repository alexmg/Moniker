using System;
using System.Text;

namespace Moniker;

/// <summary>
/// Represents a reference to a string of characters.
/// </summary>
public readonly ref struct Chars
{
    private readonly ReadOnlySpan<byte> _u8str;

    internal Chars(ReadOnlySpan<byte> u8str, int charCount)
    {
        _u8str = u8str;
        Length = charCount;
    }

    /// <summary>
    /// The length in characters.
    /// </summary>
    public int Length { get; }

    /// <summary>
    /// The length when encoded in UTF-8 bytes.
    /// </summary>
    public int Utf8Length => _u8str.Length;

    /// <summary>
    /// Writes characters to the specified buffer.
    /// </summary>
    /// <param name="buffer">The buffer to write to.</param>
    /// <returns>The number of characters written.</returns>
    public int Write(Span<char> buffer)
    {
        var count = Length;
        if (count > buffer.Length)
            return 0;

        Encoding.UTF8.GetChars(_u8str, buffer);
        return count;
    }

    /// <summary>
    /// Writes character in UTF-8 encoding to the specified buffer.
    /// </summary>
    /// <param name="buffer">The buffer to write to.</param>
    /// <returns>The number of characters written.</returns>
    public int WriteUtf8(Span<byte> buffer) =>
        _u8str.TryCopyTo(buffer) ? Utf8Length : 0;

    /// <summary>
    /// Returns a string with the same characters.
    /// </summary>
    public override string ToString()
    {
        var chars = Length <= 64 ? stackalloc char[Length] : new char[Length];
        _ = Write(chars);
        return new(chars);
    }

#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member

    /// <summary>
    /// This method is unsupported and throws <see cref="NotSupportedException"/>
    /// because <seealso cref="Chars"/> instances cannot be boxed. Use
    /// <see cref="op_Equality"/> instead.
    /// </summary>
    /// <exception cref="NotSupportedException">Always thrown.</exception>
    [Obsolete("This method is unsupported and will always throw an exception. Use the equality operator instead.")]
    public override bool Equals(object? obj) => throw new NotSupportedException();

    /// <summary>
    /// This method is unsupported and throws <see cref="NotSupportedException"/>.
    /// </summary>
    /// <exception cref="NotSupportedException">Always thrown.</exception>
    [Obsolete("This method is unsupported and will always throw an exception.")]
    public override int GetHashCode() => throw new NotSupportedException();

#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member

    /// <summary>
    /// Compares to another <see cref="Chars"/> for equality.
    /// </summary>
    /// <param name="other">The other <see cref="Chars"/> to compare to.</param>
    /// <returns><see langword="true" /> if this instance compares equals to <paramref name="other"/>.</returns>
    public bool Equals(Chars other) => _u8str.SequenceEqual(other._u8str);

    internal bool Equals(ReadOnlySpan<byte> other) => _u8str.SequenceEqual(other);

    /// <summary>
    /// Compares two <see cref="Chars"/> for equality.
    /// </summary>
    /// <param name="left">First operand to compare.</param>
    /// <param name="right">Second operand to compare.</param>
    /// <returns><see langword="true" /> if <paramref name="left"/> equals <paramref name="right"/>.</returns>
    public static bool operator ==(Chars left, Chars right) => left.Equals(right);

    /// <summary>
    /// Compares two <see cref="Chars"/> for inequality.
    /// </summary>
    /// <param name="left">First operand to compare.</param>
    /// <param name="right">Second operand to compare.</param>
    /// <returns><see langword="true" /> if <paramref name="left"/> does not equal <paramref name="right"/>.</returns>
    public static bool operator !=(Chars left, Chars right) => !left.Equals(right);
}
