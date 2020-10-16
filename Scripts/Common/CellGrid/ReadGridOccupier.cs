using System.Collections.Generic;
using System.Grid;
using System.Runtime.CompilerServices;

namespace HegaCore
{
    public readonly struct ReadGridOccupier<T> : IReadGridOccupier<T>
    {
        private readonly GridOccupier<T> source;
        private readonly bool hasSource;

        public ReadGridOccupier(GridOccupier<T> source)
        {
            this.source = source ?? _empty;
            this.hasSource = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private GridOccupier<T> GetSource()
            => this.hasSource ? (this.source ?? _empty) : _empty;

        public GridOccupier<T>.Enumerator GetEnumerator()
            => GetSource().GetEnumerator();

        IEnumerator<GridValue<T>> IReadGridOccupier<T>.GetEnumerator()
            => GetSource().GetEnumerator();

        public void GetIndices(ICollection<GridIndex> output)
            => GetSource().GetIndices(output);

        public void GetValues(ICollection<T> output)
            => GetSource().GetValues(output);

        public bool IsOccupied(in GridIndex index)
            => GetSource().IsOccupied(index);

        public bool TryGetValue(in GridIndex key, out T value)
            => GetSource().TryGetValue(key, out value);

        private static Dictionary<GridIndex, T> _emptyDict { get; } = new Dictionary<GridIndex, T>(0);

        private static GridOccupier<T> _empty { get; } = new GridOccupier<T>(_emptyDict, default, true);

        public static ReadGridOccupier<T> Empty { get; } = new ReadGridOccupier<T>(_empty);
    }
}