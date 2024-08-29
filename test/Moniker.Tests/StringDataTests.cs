using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using FluentAssertions;
using Xunit;

namespace Moniker.Tests;

public class StringDataTests
{
    [MethodImpl(MethodImplOptions.NoOptimization)]
    public StringDataTests()
    {
        _ = MobyAdjectives.Strings;
        _ = MobySurnames.Strings;
        _ = MonikerDescriptors.Strings;
        _ = MonikerAnimals.Strings;
    }

    [Fact]
    public void MobyAdjectivesData() =>
        Test(GetEmbeddedResource("MobyAdjectives.txt"),
            MobyAdjectives.Strings,
            MobyAdjectives.Count,
            expectedCount: 108);

    [Fact]
    public void MobySurnamesData() =>
        Test(GetEmbeddedResource("MobySurnames.txt"),
            MobySurnames.Strings,
            MobySurnames.Count,
            expectedCount: 235);

    [Fact]
    public void MonikerDescriptorsData()
    {
        Test(GetEmbeddedResource("MonikerDescriptors.txt"),
            MonikerDescriptors.Strings,
            MonikerDescriptors.Count,
            expectedCount: 389);
    }

    [Fact]
    public void MonikerAnimalsData()
    {
        Test(GetEmbeddedResource("MonikerAnimals.txt"),
            MonikerAnimals.Strings,
            MonikerAnimals.Count,
            expectedCount: 242);
    }

    private static void Test(string source, Utf8Strings strings, int count, int expectedCount)
    {
        count.Should().Be(expectedCount);
        strings.Count.Should().Be(expectedCount);

        var lines = from e in ReadLines(source)
                    select e.Trim()
                    into e
                    where e is [not '#', ..]
                    select e;

        var i = 0;
        using var line = lines.GetEnumerator();

        foreach (var u8Str in strings)
        {
            Assert.True(line.MoveNext());
            var str = Encoding.UTF8.GetString(u8Str);
            Assert.Equal(line.Current, str);
            Assert.Equal(line.Current, u8Str.ToString());
            Assert.Equal(u8Str, strings[i].Bytes);
            Assert.True(u8Str == strings[i]);

            // Except for some coincidental cases, ensure string isn't interned.
            // This list is not exhaustive and even may be brittle (subject to
            // test host).

            if (str is not "kind" and not "gopher" and not "left" and not "right")
                Assert.Null(string.IsInterned(str));

            i++;
        }

        Assert.Equal(i, strings.Count);
    }

    private static IEnumerable<string> ReadLines(string text)
    {
        using var reader = new StringReader(text);
        while (reader.ReadLine() is { } line)
            yield return line;
    }

    private static string GetEmbeddedResource(string fileName)
    {
        var assembly = typeof(StringDataTests).Assembly;
        var assemblyName = assembly.GetName().Name;
        using var stream = assembly.GetManifestResourceStream($"{assemblyName}.{fileName}");
        using var reader = new StreamReader(stream!);
        return reader.ReadToEnd();
    }
}
