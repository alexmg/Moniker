using System;
using FluentAssertions;
using Xunit;

namespace Moniker.Tests
{
    public class NameGeneratorTests
    {
        [Theory]
        [InlineData(MonikerStyle.Moby, null, "[a-zA-Z]+\x2D[a-zA-Z]+")]
        [InlineData(MonikerStyle.Moby, "_", "[a-zA-Z]+\x5F[a-zA-Z]+")]
        [InlineData(MonikerStyle.Moniker, null, "[a-zA-Z]+\x2D[a-zA-Z]+")]
        [InlineData(MonikerStyle.Moniker, "_", "[a-zA-Z]+\x5F[a-zA-Z]+")]
        public void GenerateWithSpecificMonikerStyleMethods(
            MonikerStyle monikerStyle,
            string separator,
            string expected)
        {
            string moniker;
            switch (monikerStyle)
            {
                case MonikerStyle.Moniker:
                    moniker = separator == null 
                        ? NameGenerator.GenerateMoniker()
                        : NameGenerator.GenerateMoniker(separator);
                    break;
                case MonikerStyle.Moby:
                    moniker = separator == null
                        ? NameGenerator.GenerateMoby()
                        : NameGenerator.GenerateMoby(separator);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(monikerStyle), monikerStyle, null);
            }

            moniker.Should().MatchRegex(expected);
        }

        [Theory]
        [InlineData(MonikerStyle.Moby, null, "[a-zA-Z]+\x2D[a-zA-Z]+")]
        [InlineData(MonikerStyle.Moby, "_", "[a-zA-Z]+\x5F[a-zA-Z]+")]
        [InlineData(MonikerStyle.Moniker, null, "[a-zA-Z]+\x2D[a-zA-Z]+")]
        [InlineData(MonikerStyle.Moniker, "_", "[a-zA-Z]+\x5F[a-zA-Z]+")]
        public void GenerateWithMonikerStyleParameter(
            MonikerStyle monikerStyle,
            string separator,
            string expected)
        {
            var moniker = separator == null
                ? NameGenerator.Generate(monikerStyle)
                : NameGenerator.Generate(monikerStyle, separator);

            moniker.Should().MatchRegex(expected);
        }
    }
}
