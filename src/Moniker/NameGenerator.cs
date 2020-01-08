using System;
using System.Collections.Generic;

namespace Moniker
{
    /// <summary>
    /// Generates fun random names that are readable and memorable.
    /// </summary>
    public static class NameGenerator
    {
        private static readonly Random Random = new Random();

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
            switch (monikerStyle)
            {
                case MonikerStyle.Moby:
                    return GenerateMoby(delimiter);
                case MonikerStyle.Moniker:
                    return GenerateMoniker(delimiter);
                default:
                    throw new ArgumentOutOfRangeException(nameof(monikerStyle));
            }
        }

        /// <summary>
        /// Generate a random name in the 'moniker' project style.
        /// </summary>
        /// <param name="delimiter">An optional delimiter to use between adjective and noun.</param>
        /// <returns>The generated random name.</returns>
        public static string GenerateMoniker(string delimiter = DefaultDelimiter)
            => BuildNamePair(Moniker.Descriptors, Moniker.Animals, delimiter);

        /// <summary>
        /// Generate a random name in the 'moby' project style.
        /// </summary>
        /// <param name="delimiter">An optional delimiter to use between adjective and noun.</param>
        /// <returns>The generated random name.</returns>
        public static string GenerateMoby(string delimiter = DefaultDelimiter)
        {
            var disallowed = ("boring", "wozniak"); // Steve Wozniak is not boring
            return BuildNamePair(Moby.Adjectives, Moby.Surname, delimiter, disallowed);
        }

        private static string BuildNamePair(
            IReadOnlyList<string> adjectives,
            IReadOnlyList<string> nouns,
            string delimiter,
            (string, string) disallowed = default)
        {
            if (string.IsNullOrEmpty(delimiter))
                throw new ArgumentException("The delimiter must not be null or empty.", nameof(delimiter));

            string result;
            var (disallowedAdjective, disallowedNoun) = disallowed;
            var disallowedResult = $"{disallowedAdjective}{delimiter}{disallowedNoun}";

            do
            {
                var adjective = GetRandomEntry(adjectives);
                var noun = GetRandomEntry(nouns);
                result = $"{adjective}{delimiter}{noun}";
            } while (result == disallowedResult);

            return result;
        }

        private static string GetRandomEntry(IReadOnlyList<string> entries)
        {
            var index = Random.Next(entries.Count);
            return entries[index];
        }
    }
}
