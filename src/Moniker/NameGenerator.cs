using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Moniker
{
    /// <summary>
    /// Generates fun random names that are readable and memorable.
    /// </summary>
    public static class NameGenerator
    {
        /// <summary>
        /// The default delimiter between adjective and noun.
        /// </summary>
        public const string DefaultDelimiter = "-";

        /// <summary>
        /// Generate a random name in the specified style.
        /// </summary>
        /// <param name="monikerStyle">The style of random name.</param>
        /// <param name="delimiter">An optional delimiter to use between adjective and noun.</param>
        /// <returns>The generated random name.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string Generate(MonikerStyle monikerStyle, string delimiter = DefaultDelimiter)
        {
            ValidateDelimiterArgument(delimiter);
            Generate(monikerStyle, out var adjective, out var noun);
            return Join(adjective, delimiter, noun);
        }

        /// <summary>
        /// Generate a random name in the specified style.
        /// </summary>
        /// <param name="monikerStyle">The style of random name.</param>
        /// <param name="adjective">The adjective part of the random name.</param>
        /// <param name="noun">The noun part of the random name.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void Generate(MonikerStyle monikerStyle, out Chars adjective, out Chars noun)
        {
            switch (monikerStyle)
            {
                case MonikerStyle.Moby: GenerateMoby(out adjective, out noun); break;
                case MonikerStyle.Moniker: GenerateMoniker(out adjective, out noun); break;
                default: throw new ArgumentOutOfRangeException(nameof(monikerStyle));
            }
        }

        /// <summary>
        /// Generate a random name in the 'moniker' project style.
        /// </summary>
        /// <param name="delimiter">An optional delimiter to use between adjective and noun.</param>
        /// <returns>The generated random name.</returns>
        public static string GenerateMoniker(string delimiter = DefaultDelimiter)
        {
            ValidateDelimiterArgument(delimiter);
            GenerateMoniker(out var adjective, out var noun);
            return Join(adjective, delimiter, noun);
        }

        /// <summary>
        /// Generate a random name in the 'moniker' project style.
        /// </summary>
        /// <param name="adjective">The adjective part of the random name.</param>
        /// <param name="noun">The noun part of the random name.</param>
        public static void GenerateMoniker(out Chars adjective, out Chars noun)
            => BuildNamePair(MonikerDescriptors.Strings, out adjective, MonikerAnimals.Strings, out noun);

        /// <summary>
        /// Generate a random name in the 'moby' project style.
        /// </summary>
        /// <param name="delimiter">An optional delimiter to use between adjective and noun.</param>
        /// <returns>The generated random name.</returns>
        public static string GenerateMoby(string delimiter = DefaultDelimiter)
        {
            ValidateDelimiterArgument(delimiter);
            GenerateMoby(out var adjective, out var noun);
            return Join(adjective, delimiter, noun);
        }

        /// <summary>
        /// Generate a random name in the 'moby' project style.
        /// </summary>
        /// <returns>The generated random name.</returns>
        public static void GenerateMoby(out Chars adjective, out Chars noun)
            => BuildNamePair(MobyAdjectives.Strings, out adjective, MobySurnames.Strings, out noun);

        private static void ValidateDelimiterArgument(
            string delimiter,
            [CallerArgumentExpression(nameof(delimiter))] string? paramName = null)
        {
            if (string.IsNullOrEmpty(delimiter))
                throw new ArgumentException("The delimiter must not be null or empty.", paramName);
        }

        private static void BuildNamePair(
            Utf8Strings adjectives, out Chars adjective,
            Utf8Strings nouns, out Chars noun)
        {
            do
            {
                adjective = GetRandomEntry(adjectives);
                noun = GetRandomEntry(nouns);
            }
            while (adjective.Equals("boring"u8) && noun.Equals("wozniak"u8)); // Steve Wozniak is not boring
        }

        private static Chars GetRandomEntry(Utf8Strings entries)
        {
            var index = Random.Shared.Next(entries.Count);
            return entries[index];
        }

        private static string Join(Chars adjective, ReadOnlySpan<char> delimiter, Chars noun)
        {
            var length = adjective.Length + delimiter.Length + noun.Length;
            var chars = length <= 64 ? stackalloc char[length] : new char[length];

            var writeCount = adjective.Write(chars);
            Debug.Assert(writeCount == adjective.Length);

            delimiter.CopyTo(chars[adjective.Length..]);

            writeCount = noun.Write(chars[(adjective.Length + delimiter.Length)..]);
            Debug.Assert(writeCount == noun.Length);

            return new(chars);
        }
    }
}
