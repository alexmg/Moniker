using System;

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
            return monikerStyle switch
            {
                MonikerStyle.Moby => GenerateMoby(delimiter),
                MonikerStyle.Moniker => GenerateMoniker(delimiter),
                _ => throw new ArgumentOutOfRangeException(nameof(monikerStyle))
            };
        }

        /// <summary>
        /// Generate a random name in the 'moniker' project style.
        /// </summary>
        /// <param name="delimiter">An optional delimiter to use between adjective and noun.</param>
        /// <returns>The generated random name.</returns>
        public static string GenerateMoniker(string delimiter = DefaultDelimiter)
            => BuildNamePair(MonikerDescriptors.Strings, MonikerAnimals.Strings, delimiter);

        /// <summary>
        /// Generate a random name in the 'moby' project style.
        /// </summary>
        /// <param name="delimiter">An optional delimiter to use between adjective and noun.</param>
        /// <returns>The generated random name.</returns>
        public static string GenerateMoby(string delimiter = DefaultDelimiter)
            => BuildNamePair(MobyAdjectives.Strings, MobySurnames.Strings, delimiter);

        private static string BuildNamePair(
            Utf8Strings adjectives,
            Utf8Strings nouns,
            string delimiter)
        {
            if (string.IsNullOrEmpty(delimiter))
                throw new ArgumentException("The delimiter must not be null or empty.", nameof(delimiter));

            BuildNamePair(adjectives, out var adjective, nouns, out var noun);

            var length = adjective.CharCount + delimiter.Length + noun.CharCount;

            var chars = length <= 64 ? stackalloc char[length] : new char[length];

            _ = adjective.GetChars(chars);
            delimiter.CopyTo(chars[adjective.CharCount..]);
            noun.GetChars(chars[(adjective.CharCount + delimiter.Length)..]);

            return new(chars);
        }

        private static void BuildNamePair(
            Utf8Strings adjectives, out Utf8String adjective,
            Utf8Strings nouns, out Utf8String noun)
        {
            do
            {
                adjective = GetRandomEntry(adjectives);
                noun = GetRandomEntry(nouns);
            }
            while (adjective == "boring"u8 && noun == "wozniak"u8); // Steve Wozniak is not boring
        }

        private static Utf8String GetRandomEntry(Utf8Strings entries)
        {
            var index = Random.Shared.Next(entries.Count);
            return entries[index];
        }
    }
}
