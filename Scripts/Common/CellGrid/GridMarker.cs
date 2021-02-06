using System;
using System.Collections.Generic;
using System.Collections.Pooling;
using System.Grid;
using System.Linq;

namespace HegaCore
{
    public class GridMarker<T> : IGridMarker<T>
    {
        private readonly Dictionary<GridIndex, T> map;
        private readonly T defaultValue;

        public GridMarker(T defaultValue)
        {
            this.map = new Dictionary<GridIndex, T>();
            this.defaultValue = defaultValue;
        }

        public GridMarker(IDictionary<GridIndex, T> map, T defaultValue)
        {
            if (map == null)
                throw new ArgumentNullException(nameof(map));

            this.map = new Dictionary<GridIndex, T>(map);
            this.defaultValue = defaultValue;
        }

        public GridMarker(IReadOnlyDictionary<GridIndex, T> map, T defaultValue)
        {
            if (map == null)
                throw new ArgumentNullException(nameof(map));

            this.map = new Dictionary<GridIndex, T>();
            this.defaultValue = defaultValue;

            this.map.AddRange(map);
        }

        public GridMarker(Dictionary<GridIndex, T> map, T defaultValue, bool useMapReference = false)
        {
            if (map == null)
                throw new ArgumentNullException(nameof(map));

            this.map = useMapReference ? map : new Dictionary<GridIndex, T>(map);
            this.defaultValue = defaultValue;
        }

        public bool IsMarked(in GridIndex index)
            => this.map.ContainsKey(index);

        public void GetIndices(ICollection<GridIndex> output)
            => output?.AddRange(this.map.Keys);

        public void GetValues(ICollection<T> output)
            => output?.AddRange(this.map.Keys);

        public void Mark(in GridIndex index)
        {
            this.map[index] = this.defaultValue;
        }

        public void Mark(in GridIndexRange range)
        {
            foreach (var index in range)
            {
                this.map[index] = this.defaultValue;
            }
        }

        public void Mark(IEnumerable<GridIndex> indices)
        {
            foreach (var index in indices)
            {
                this.map[index] = this.defaultValue;
            }
        }

        public void Mark(in GridIndex index, in GridIndex valueIndex)
        {
            if (!this.map.TryGetValue(valueIndex, out var value))
                value = this.defaultValue;

            this.map[index] = value;
        }

        public void Mark(in GridIndexRange range, in GridIndex valueIndex)
        {
            if (!this.map.TryGetValue(valueIndex, out var value))
                value = this.defaultValue;

            foreach (var index in range)
            {
                this.map[index] = value;
            }
        }

        public void Mark(IEnumerable<GridIndex> indices, in GridIndex valueIndex)
        {
            if (!this.map.TryGetValue(valueIndex, out var value))
                value = this.defaultValue;

            foreach (var index in indices)
            {
                this.map[index] = value;
            }
        }

        public void Mark(in GridIndex index, T value)
        {
            this.map[index] = value;
        }

        public void Mark(in GridIndexRange range, T value)
        {
            foreach (var index in range)
            {
                this.map[index] = value;
            }
        }

        public void Mark(IEnumerable<GridIndex> indices, T value)
        {
            foreach (var index in indices)
            {
                this.map[index] = value;
            }
        }

        public void Unmark(in GridIndex index)
        {
            this.map.Remove(index);
        }

        public void Unmark(in GridIndexRange range)
        {
            foreach (var index in range)
            {
                this.map.Remove(index);
            }
        }

        public void Unmark(IEnumerable<GridIndex> indices)
        {
            foreach (var index in indices)
            {
                this.map.Remove(index);
            }
        }

        public void Unmark(in GridIndexRange range, in GridIndexRange ignore)
        {
            foreach (var index in range)
            {
                if (ignore.Contains(index))
                    continue;

                this.map.Remove(index);
            }
        }

        public void Unmark(in GridIndexRange range, in ReadCollection<GridIndex> ignore)
        {
            foreach (var index in range)
            {
                if (ignore.Contains(index))
                    continue;

                this.map.Remove(index);
            }
        }

        public void Unmark(IEnumerable<GridIndex> indices, in GridIndexRange ignore)
        {
            foreach (var index in indices)
            {
                if (ignore.Contains(index))
                    continue;

                this.map.Remove(index);
            }
        }

        public void Unmark(IEnumerable<GridIndex> indices, in ReadCollection<GridIndex> ignore)
        {
            foreach (var index in indices)
            {
                if (ignore.Contains(index))
                    continue;

                this.map.Remove(index);
            }
        }

        public void Unmark(T value)
        {
            if (value == null)
                return;

            var query = this.map.Where(x => value.Equals(x.Value))
                                .Select(x => x.Key);

            var keys = Pool.Provider.List<GridIndex>();
            keys.AddRange(query);

            foreach (var key in keys)
            {
                this.map.Remove(key);
            }

            Pool.Provider.Return(keys);
        }

        public bool TryGetValue(in GridIndex key, out T value)
            => this.map.TryGetValue(key, out value);

        public void Clear()
            => this.map.Clear();

        public Enumerator GetEnumerator()
            => new Enumerator(this.map);

        IEnumerator<GridValue<T>> IReadGridMarker<T>.GetEnumerator()
            => GetEnumerator();

        public readonly struct Enumerator : IEnumerator<GridValue<T>>
        {
            private readonly IEnumerator<KeyValuePair<GridIndex, T>> source;
            private readonly bool hasSource;

            public Enumerator(in ReadDictionary<GridIndex, T> map)
            {
                this.source = map.GetEnumerator();
                this.hasSource = true;
            }

            public GridValue<T> Current
            {
                get
                {
                    (var key, var value) = this.source.Current;
                    return new GridValue<T>(in key, value);
                }
            }

            object System.Collections.IEnumerator.Current
                => this.Current;

            public bool MoveNext()
                => this.hasSource && this.source.MoveNext();

            public void Reset()
                => this.source?.Reset();

            public void Dispose()
                => this.source?.Dispose();
        }
    }
}