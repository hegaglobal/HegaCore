using System.Collections.Generic;
using System.Grid;
using System.Runtime.CompilerServices;

namespace HegaCore
{
    public readonly struct ReadGridMarker<T> : IReadGridMarker<T>
    {
        private readonly GridMarker<T> source;
        private readonly bool hasSource;

        public ReadGridMarker(GridMarker<T> source)
        {
            this.source = source ?? _empty;
            this.hasSource = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private GridMarker<T> GetSource()
            => this.hasSource ? (this.source ?? _empty) : _empty;

        public GridMarker<T>.Enumerator GetEnumerator()
            => GetSource().GetEnumerator();

        IEnumerator<GridValue<T>> IReadGridMarker<T>.GetEnumerator()
            => GetSource().GetEnumerator();

        public void GetIndices(ICollection<GridIndex> output)
            => GetSource().GetIndices(output);

        public void GetValues(ICollection<T> output)
            => GetSource().GetValues(output);

        public bool IsMarked(in GridIndex index)
            => GetSource().IsMarked(index);

        public bool TryGetValue(in GridIndex key, out T value)
            => GetSource().TryGetValue(key, out value);

        private static Dictionary<GridIndex, T> _emptyDict { get; } = new Dictionary<GridIndex, T>(0);

        private static GridMarker<T> _empty { get; } = new GridMarker<T>(_emptyDict, default, true);

        public static ReadGridMarker<T> Empty { get; } = new ReadGridMarker<T>(_empty);
    }
}