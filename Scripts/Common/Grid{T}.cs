using System.Collections;
using System.Collections.Generic;

namespace HegaCore
{
    public class Grid<T> : IReadGrid<T>,
                           IReadOnlyDictionary<GridIndex, T>,
                           IReadOnlyCollection<KeyValuePair<GridIndex, T>>
    {
        public GridIndex Size { get; private set; }

        private readonly Dictionary<GridIndex, T> data = new Dictionary<GridIndex, T>();

        public int Count => this.data.Count;

        public IEnumerable<GridIndex> Indices => this.data.Keys;

        public IEnumerable<T> Cells => this.data.Values;

        public T this[in GridIndex key] => this.data[key];

        public Grid() { }

        public Grid(in GridIndex size, IReadOnlyDictionary<GridVector, T> data)
        {
            Initialize(size, data);
        }

        public Grid(in GridIndex size, IReadOnlyDictionary<GridIndex, T> data)
        {
            Initialize(size, data);
        }

        public void Clear()
        {
            this.Size = GridIndex.Zero;
            this.data.Clear();
        }

        public void Initialize(in GridIndex size, IReadOnlyDictionary<GridVector, T> data)
        {
            Clear();

            this.Size = size;

            foreach (var kv in data)
            {
                this.data[kv.Key] = kv.Value;
            }
        }

        public void Initialize(in GridIndex size, IReadOnlyDictionary<GridIndex, T> data)
        {
            Clear();

            this.Size = size;

            foreach (var kv in data)
            {
                this.data[kv.Key] = kv.Value;
            }
        }

        public bool ContainsIndex(in GridIndex index)
            => this.data.ContainsKey(index);

        public bool TryGetCell(in GridIndex index, out T cell)
            => this.data.TryGetValue(index, out cell);

        public IEnumerator<KeyValuePair<GridIndex, T>> GetEnumerator()
            => this.data.GetEnumerator();

        public void GetIndices(ICollection<GridIndex> indices, in GridIndex position, int extend)
            => GetIndices(indices, position, GridIndex.One * extend, this.Size);

        public void GetIndices(ICollection<GridIndex> indices, in GridIndex position, in GridIndex extend)
            => GetIndices(indices, position, extend, this.Size);

        private void GetIndices(ICollection<GridIndex> indices, in GridIndex position, in GridIndex extend, in GridIndex size)
        {
            var lastIndex = size - GridIndex.One;
            var min = GridIndex.Clamp(position - extend, GridIndex.Zero, lastIndex);
            var max = GridIndex.Clamp(position + extend, GridIndex.Zero, lastIndex);

            for (var r = min.Row; r <= max.Row; r++)
            {
                for (var c = min.Column; c <= max.Column; c++)
                {
                    var index = new GridIndex(r, c);

                    if (!indices.Contains(index))
                        indices.Add(index);
                }
            }
        }

        public void GetCells(ICollection<T> cells, in GridIndex position, int extend)
            => GetCells(cells, position, GridIndex.One * extend, this.Size);

        public void GetCells(ICollection<T> cells, in GridIndex position, in GridIndex extend)
            => GetCells(cells, position, extend, this.Size);

        private void GetCells(ICollection<T> cells, in GridIndex position, in GridIndex extend, in GridIndex size)
        {
            var lastIndex = size - GridIndex.One;
            var min = GridIndex.Clamp(position - extend, GridIndex.Zero, lastIndex);
            var max = GridIndex.Clamp(position + extend, GridIndex.Zero, lastIndex);

            for (var r = min.Row; r <= max.Row; r++)
            {
                for (var c = min.Column; c <= max.Column; c++)
                {
                    var index = new GridIndex(r, c);

                    if (this.data.TryGetValue(index, out var cell) && !cells.Contains(cell))
                        cells.Add(cell);
                }
            }
        }

        public void GetCells(ICollection<GridCell<T>> cells, in GridIndex position, int extend)
            => GetCells(cells, position, GridIndex.One * extend, this.Size);

        public void GetCells(ICollection<GridCell<T>> cells, in GridIndex position, in GridIndex extend)
            => GetCells(cells, position, extend, this.Size);

        private void GetCells(ICollection<GridCell<T>> cells, in GridIndex position, in GridIndex extend, in GridIndex size)
        {
            var lastIndex = size - GridIndex.One;
            var min = GridIndex.Clamp(position - extend, GridIndex.Zero, lastIndex);
            var max = GridIndex.Clamp(position + extend, GridIndex.Zero, lastIndex);

            for (var r = min.Row; r <= max.Row; r++)
            {
                for (var c = min.Column; c <= max.Column; c++)
                {
                    var index = new GridIndex(r, c);

                    if (!this.data.TryGetValue(index, out var cell))
                        continue;

                    var data = new GridCell<T>(index, cell);

                    if (!cells.Contains(data))
                        cells.Add(data);
                }
            }
        }

        T IReadOnlyDictionary<GridIndex, T>.this[GridIndex key] => this.data[key];

        IEnumerable<GridIndex> IReadOnlyDictionary<GridIndex, T>.Keys => this.data.Keys;

        IEnumerable<T> IReadOnlyDictionary<GridIndex, T>.Values => this.data.Values;

        bool IReadOnlyDictionary<GridIndex, T>.ContainsKey(GridIndex key)
            => this.data.ContainsKey(key);

        IEnumerator IEnumerable.GetEnumerator()
            => this.data.GetEnumerator();

        bool IReadOnlyDictionary<GridIndex, T>.TryGetValue(GridIndex key, out T value)
            => this.data.TryGetValue(key, out value);
    }

    public interface IReadGrid<T> : IReadOnlyCollection<KeyValuePair<GridIndex, T>>
    {
        T this[in GridIndex key] { get; }

        IEnumerable<GridIndex> Indices { get; }

        IEnumerable<T> Cells { get; }

        bool ContainsIndex(in GridIndex index);

        bool TryGetCell(in GridIndex index, out T cell);
    }
}