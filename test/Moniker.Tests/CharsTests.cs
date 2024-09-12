using System;
using System.Linq;
using System.Text;
using FluentAssertions;
using Xunit;

namespace Moniker.Tests;

public class CharsTests
{
    private static Chars Chars(string str) =>
        new(Encoding.UTF8.GetBytes(str), str.Length);

    [Theory]
    [InlineData("foobar", 6, 6)]
    [InlineData("☀️", 2, 6)]
    [InlineData("🐟", 2, 4)]
    [InlineData("⭐", 1, 3)]
    [InlineData("🐕", 2, 4)]
    public void Lengths(string str, int length, int utf8Length)
    {
        var chars = Chars(str);
        chars.Length.Should().Be(length);
        chars.Utf8Length.Should().Be(utf8Length);
    }

    [Theory]
    [InlineData(6, "foobar")]
    [InlineData(7, "foobar-")]
    [InlineData(8, "foobar--")]
    public void Write(int length, string expected)
    {
        var chars = Chars("foobar");
        var buffer = new char[length];
        buffer.AsSpan().Fill('-');

        var result = chars.Write(buffer);

        result.Should().Be(6);
        new string(buffer).Should().Be(expected);
    }

    [Theory]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void WriteWhenBufferIsTooSmall(int length)
    {
        var chars = Chars("foobar");
        var buffer = new char[length];
        buffer.AsSpan().Fill('-');

        var result = chars.Write(buffer);

        result.Should().Be(0);
        new string('-', length).Should().Be(new string(buffer));
    }

    [Theory]
    [InlineData(6, "foobar")]
    [InlineData(7, "foobar-")]
    [InlineData(8, "foobar--")]
    public void WriteUtf8(int length, string expected)
    {
        var chars = Chars("foobar");
        var buffer = new byte[length];
        buffer.AsSpan().Fill((byte)'-');

        var result = chars.WriteUtf8(buffer);

        result.Should().Be(6);
        Encoding.UTF8.GetString(buffer).Should().Be(expected);
    }

    [Theory]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void WriteUtf8WhenBufferIsTooSmall(int length)
    {
        var chars = Chars("foobar");
        var buffer = new byte[length];
        buffer.AsSpan().Fill((byte)'-');

        var result = chars.WriteUtf8(buffer);

        result.Should().Be(0);
        Encoding.UTF8.GetString(buffer).Should().Be(new string('-', length));
    }

    [Fact]
    public void ToStringReturnsStringWithAllCharacters()
    {
        var samples = Enumerable.Range(0, 100)
                                .Select(n => string.Join(string.Empty, Enumerable.Repeat("foobar", n)))
                                .TakeWhile(s => s.Length <= 256);

        foreach (var str in samples)
            Chars(str).ToString().Should().Be(str);
    }

    [Fact]
    public void EqualsComparesValues()
    {
        var chars1 = Chars("foobar");
        var chars2 = Chars("foobar");
        var chars3 = Chars("FOOBAR");
        chars1.Equals(chars2).Should().BeTrue();
        chars1.Equals(chars3).Should().BeFalse();
    }

    [Fact]
    public void EqualityOperatorComparesValues()
    {
        var chars1 = Chars("foobar");
        var chars2 = Chars("foobar");
        var chars3 = Chars("FOOBAR");
        (chars1 == chars2).Should().BeTrue();
        (chars1 == chars3).Should().BeFalse();
    }

    [Fact]
    public void InequalityOperatorComparesValues()
    {
        var chars1 = Chars("foobar");
        var chars2 = Chars("foobar");
        var chars3 = Chars("FOOBAR");
        (chars1 != chars2).Should().BeFalse();
        (chars1 != chars3).Should().BeTrue();
    }

    [Fact]
    public void GetHashCodeIsUnsupported()
    {
        var act = () =>
        {
            var chars = Chars("foobar");
            _ = chars.GetHashCode();
        };

        act.Should().Throw<NotSupportedException>();
    }

    [Fact]
    public void EqualsIsUnsupported()
    {
        var act = () =>
        {
            var chars = Chars("foobar");
            _ = chars.Equals(42);
        };

        act.Should().Throw<NotSupportedException>();
    }
}
