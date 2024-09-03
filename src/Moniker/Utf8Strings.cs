#nullable enable

using System;
using System.Diagnostics;
using System.Text;

namespace Moniker;

internal readonly ref struct Utf8Strings
{
    private readonly ReadOnlySpan<byte> _data;
    private readonly ReadOnlySpan<int> _offsets;
    private readonly ReadOnlySpan<byte> _charCounts;

    public readonly int Count;

    public Utf8Strings(int count, ReadOnlySpan<byte> data, ReadOnlySpan<int> offsets, ReadOnlySpan<byte> charCounts)
    {
        Debug.Assert(offsets.Length == count + 1);
        Debug.Assert(offsets[^1] == data.Length);
        Debug.Assert(charCounts.Length == count);

        Count = count;
        _data = data;
        _offsets = offsets;
        _charCounts = charCounts;
    }

    public Chars this[int index] => new(_data[_offsets[index].._offsets[index + 1]], _charCounts[index]);

    public override string ToString() => $"{{ Count = {Count}, Size = {_data.Length} }}";

    public Enumerator GetEnumerator() => new(this);

    public ref struct Enumerator(Utf8Strings strings)
    {
        private int _index = -1;
        private readonly Utf8Strings _strings = strings;

        /// <remarks>
        /// Behaviour is undefined if <see cref="MoveNext"/> has never been called or returned
        /// <see langword="false"/>.
        /// </remarks>
        public Chars Current => _strings[_index];

        public bool MoveNext()
        {
            var newIndex = _index + 1;
            if (newIndex == _strings.Count)
                return false;
            _index = newIndex;
            return true;
        }
    }
}
