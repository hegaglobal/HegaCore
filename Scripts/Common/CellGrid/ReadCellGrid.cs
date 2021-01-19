using System.Collections.Generic;
using System.Collections.Pooling;
using System.Grid;
using System;

namespace HegaCore
{
    using UnityRandom = UnityEngine.Random;

    public class ReadCellGrid
    {
        public GridSize Size => this.data.Size;

        public ClampedGridSize ClampedSize { get; }

        private readonly Grid<Cell> data;
        private readonly IReadGridMarker marker;

        public ReadCellGrid(Grid<Cell> data, IReadGridMarker marker)
        {
            this.data = new Grid<Cell>(data);
            this.ClampedSize = this.data.Size;
            this.marker = marker ?? throw new ArgumentNullException();
        }

        public ReadCellGrid(in ReadGrid<Cell> data, IReadGridMarker marker)
        {
            var cache = Pool.Provider.List<GridValue<Cell>>();
            data.GetIndexedValues(cache);

            this.data = new Grid<Cell>(data.Size, cache);
            this.ClampedSize = this.data.Size;
            Pool.Provider.Return(cache);

            this.marker = marker ?? throw new ArgumentNullException();
        }

        private void RemoveDiagonal(in GridIndex pivot, List<Cell> cells)
        {
            for (var i = cells.Count - 1; i >= 0; i--)
            {
                var index = cells[i].Index;

                if (index.Row != pivot.Row && index.Column != pivot.Column)
                    cells.RemoveAt(i);
            }
        }

        private void OnlyDiagonal(in GridIndex pivot, List<Cell> cells)
        {
            for (var i = cells.Count - 1; i >= 0; i--)
            {
                var index = cells[i].Index;

                if (index.Row == pivot.Row || index.Column == pivot.Column)
                    cells.RemoveAt(i);
            }
        }

        private void RemoveMarked(List<Cell> cells, HashSet<GridIndex> marked)
        {
            for (var i = cells.Count - 1; i >= 0; i--)
            {
                if (marked.Contains(cells[i].Index))
                    cells.RemoveAt(i);
            }
        }

        private void OnlyMarked(List<Cell> cells, HashSet<GridIndex> marked)
        {
            for (var i = cells.Count - 1; i >= 0; i--)
            {
                if (!marked.Contains(cells[i]))
                    cells.RemoveAt(i);
            }
        }

        private void Filter(in GridIndex? pivot, List<Cell> cells, ICollection<Cell> output, in CellModes modes)
        {
            if (cells.Count > 0 && pivot.HasValue)
            {
                switch (modes.Diagonal)
                {
                    case CellMode.Exclude:
                        RemoveDiagonal(pivot.Value, cells);
                        break;

                    case CellMode.Only:
                        OnlyDiagonal(pivot.Value, cells);
                        break;
                }
            }

            if (cells.Count > 0 && modes.Marked != CellMode.Include)
            {
                var marked = Pool.Provider.HashSet<GridIndex>();
                this.marker.GetIndices(marked);

                switch (modes.Marked)
                {
                    case CellMode.Exclude:
                        RemoveMarked(cells, marked);
                        break;

                    case CellMode.Only:
                        OnlyMarked(cells, marked);
                        break;
                }

                Pool.Provider.Return(marked);
            }

            output.AddRange(cells, false, true);
            Pool.Provider.Return(cells);
        }

        public void Filter(ICollection<Cell> cells, in CellModes modes)
        {
            var cache = Pool.Provider.List<Cell>();
            cache.AddRange(cells);
            cells.Clear();

            Filter(null, cache, cells, modes);
            cells.Clear();
            cells.AddRange(cache);

            Pool.Provider.Return(cache);
        }

        public void Filter(ICollection<Cell> cells, in GridIndex pivot, in CellModes modes)
        {
            var cache = Pool.Provider.List<Cell>();
            cache.AddRange(cells);
            cells.Clear();

            Filter(pivot, cache, cells, modes);
            cells.Clear();
            cells.AddRange(cache);

            Pool.Provider.Return(cache);
        }

        public Cell GetCell(in GridIndex index)
            => this.data[index];

        public bool TryGetCell(in GridIndex index, out Cell cell)
            => this.data.TryGetValue(index, out cell);

        public void GetCells(in GridIndex pivot, int extend, ICollection<Cell> output, in CellModes? modes = null)
        {
            var cells = Pool.Provider.List<Cell>();
            this.data.GetValues(pivot, extend, cells);

            Filter(pivot, cells, output, modes ?? CellModes.Include);
        }

        public void GetCells(in GridIndex pivot, int lowerExtend, int upperExtend, ICollection<Cell> output, in CellModes? modes = null)
        {
            var cells = Pool.Provider.List<Cell>();
            this.data.GetValues(pivot, lowerExtend, upperExtend, cells);

            Filter(pivot, cells, output, modes ?? CellModes.Include);
        }

        public void GetCells(in GridIndex pivot, bool byRow, ICollection<Cell> output, in CellModes? modes = null)
        {
            var cells = Pool.Provider.List<Cell>();
            this.data.GetValues(pivot, byRow, cells);

            Filter(pivot, cells, output, modes ?? CellModes.Include);
        }

        public void GetCells(in GridIndex pivot, in GridIndex extend, ICollection<Cell> output, in CellModes? modes = null)
        {
            var cells = Pool.Provider.List<Cell>();
            this.data.GetValues(pivot, extend, cells);

            Filter(pivot, cells, output, modes ?? CellModes.Include);
        }

        public void GetCells(in GridIndex pivot, in GridIndex lowerExtend, in GridIndex upperExtend, ICollection<Cell> output, in CellModes? modes = null)
        {
            var cells = Pool.Provider.List<Cell>();
            this.data.GetValues(pivot, lowerExtend, upperExtend, cells);

            Filter(pivot, cells, output, modes ?? CellModes.Include);
        }

        public void GetCells(in GridIndex pivot, ICollection<Cell> output, in CellModes? modes = null)
        {
            var cells = Pool.Provider.List<Cell>();
            this.data.GetValues(cells);

            Filter(pivot, cells, output, modes ?? CellModes.Include);
        }

        public void GetCells(in GridIndex pivot, in GridIndexRange range, ICollection<Cell> output, in CellModes? modes = null)
        {
            var cells = Pool.Provider.List<Cell>();
            this.data.GetValues(range, cells);

            Filter(pivot, cells, output, modes ?? CellModes.Include);
        }

        public void GetCells(in GridIndex pivot, IEnumerable<GridIndex> indices, ICollection<Cell> output, in CellModes? modes = null)
        {
            var cells = Pool.Provider.List<Cell>();
            this.data.GetValues(indices, cells);

            Filter(pivot, cells, output, modes ?? CellModes.Include);
        }

        public void GetCells(in GridIndex pivot, IEnumerator<GridIndex> enumerator, ICollection<Cell> output, in CellModes? modes = null)
        {
            var cells = Pool.Provider.List<Cell>();
            this.data.GetValues(enumerator, cells);

            Filter(pivot, cells, output, modes ?? CellModes.Include);
        }

        public void GetCells(ICollection<Cell> output, in CellModes? modes = null)
        {
            var cells = Pool.Provider.List<Cell>();
            this.data.GetValues(cells);

            Filter(null, cells, output, modes ?? CellModes.Include);
        }

        public void GetCells(in GridIndexRange range, ICollection<Cell> output, in CellModes? modes = null)
        {
            var cells = Pool.Provider.List<Cell>();
            this.data.GetValues(range, cells);

            Filter(null, cells, output, modes ?? CellModes.Include);
        }

        public void GetCells(IEnumerable<GridIndex> indices, ICollection<Cell> output, in CellModes? modes = null)
        {
            var cells = Pool.Provider.List<Cell>();
            this.data.GetValues(indices, cells);

            Filter(null, cells, output, modes ?? CellModes.Include);
        }

        public void GetCells(IEnumerator<GridIndex> enumerator, ICollection<Cell> output, in CellModes? modes = null)
        {
            var cells = Pool.Provider.List<Cell>();
            this.data.GetValues(enumerator, cells);

            Filter(null, cells, output, modes ?? CellModes.Include);
        }

        public void GetRanges(int size, int step, ICollection<GridIndexRange> output,
                              in CellModes? modes = null, bool fromEnd = false, GridDirection direction = default)
        {
            var ranges = Pool.Provider.List<GridIndexRange>();
            this.ClampedSize.Partitioner(fromEnd, direction).Partition(size, step, ranges);

            Filter(null, ranges, modes ?? CellModes.Include);
            output.AddRange(ranges);

            Pool.Provider.Return(ranges);
        }

        public void GetRanges(in GridIndexRange pivot, int size, int step, ICollection<GridIndexRange> output,
                              in CellModes? modes = null, bool fromEnd = false, GridDirection direction = default)
        {
            var slice = this.ClampedSize.IndexRange(pivot, size);
            var ranges = Pool.Provider.List<GridIndexRange>();
            this.ClampedSize.Partitioner(fromEnd, direction).Partition(slice, size, step, ranges);

            Filter(pivot, ranges, modes ?? CellModes.Include);
            output.AddRange(ranges);

            Pool.Provider.Return(ranges);
        }

        public bool TryGetRandomCell(out Cell output, in CellModes? modes = null)
        {
            var cells = Pool.Provider.List<Cell>();
            GetCells(cells, modes);

            return TryGetRandomCell(cells, out output);
        }

        public bool TryGetRandomCell(in GridIndexRange range, out Cell output, in CellModes? modes = null)
        {
            var cells = Pool.Provider.List<Cell>();
            GetCells(range, cells, modes);

            return TryGetRandomCell(cells, out output);
        }

        public bool TryGetRandomCell(in GridIndex pivot, in GridIndexRange range, out Cell output, in CellModes? modes = null)
        {
            var cells = Pool.Provider.List<Cell>();
            GetCells(pivot, range, cells, modes);

            return TryGetRandomCell(cells, out output);
        }

        private bool TryGetRandomCell(List<Cell> cells, out Cell output)
        {
            if (cells.Count <= 0)
            {
                Pool.Provider.Return(cells);
                output = default;
                return false;
            }

            var random = Pool.Provider.List<Cell>();
            random.AddRange(cells.Randomize());

            var index = UnityRandom.Range(0, random.Count);
            output = random[index];

            Pool.Provider.Return(cells, random);
            return true;
        }

        public void GetRandomCells(int count, ICollection<Cell> output, in CellModes? modes = null)
        {
            if (count <= 0)
                return;

            var cells = Pool.Provider.List<Cell>();
            GetCells(cells, modes);
            GetRandomCells(count, output, cells);
        }

        public void GetRandomCells(in GridIndexRange range, int count, ICollection<Cell> output, in CellModes? modes = null)
        {
            if (count <= 0)
                return;

            var cells = Pool.Provider.List<Cell>();
            GetCells(range, cells, modes);
            GetRandomCells(count, output, cells);
        }

        private static void GetRandomCells(int count, ICollection<Cell> output, List<Cell> cells)
        {
            if (cells.Count > 0)
            {
                var random = Pool.Provider.List<Cell>();
                random.AddRange(cells.Randomize());

                foreach (var _ in IntRange.FromSize(count))
                {
                    if (random.Count <= 0)
                        break;

                    var index = UnityRandom.Range(0, random.Count);
                    output.Add(random[index]);

                    random.RemoveAt(index);
                }

                Pool.Provider.Return(random);
            }

            Pool.Provider.Return(cells);
        }

        public bool TryGetRandomRange(int size, out GridIndexRange range, in CellModes? modes = null,
                                      GridDirection direction = default)
            => TryGetRandomRange(size, size, out range, modes, direction);

        public bool TryGetRandomRange(int size, int step, out GridIndexRange range, in CellModes? modes = null,
                                      GridDirection direction = default)
        {
            range = default;

            if (size < 1)
                return false;

            if (size == 1)
            {
                if (TryGetRandomCell(out var cell, modes))
                {
                    range = new GridIndexRange(cell, cell);
                    return true;
                }

                return false;
            }

            var cache = Pool.Provider.HashSet<GridIndexRange>();
            this.ClampedSize.Partitioner(false, direction).Partition(size, step, cache);
            this.ClampedSize.Partitioner(true, direction).Partition(size, step, cache);

            var ranges = Pool.Provider.List<GridIndexRange>();
            ranges.AddRange(cache);
            Pool.Provider.Return(cache);

            Filter(null, ranges, modes ?? CellModes.Include);
            return TryGetRandomRange(ref range, ranges);
        }

        public bool TryGetRandomRange(in GridIndex pivot, int extend, int size, int step,
                                      out GridIndexRange range, in CellModes? modes = null,
                                      GridDirection direction = default)
            => TryGetRandomRange(pivot, this.ClampedSize.IndexRange(pivot, extend), size, step,
                                 out range, modes, direction);

        public bool TryGetRandomRange(in GridIndex pivot, int lowerExtend, int upperExtend, int size, int step,
                                      out GridIndexRange range, in CellModes? modes = null,
                                      GridDirection direction = default)
            => TryGetRandomRange(pivot, this.ClampedSize.IndexRange(pivot, lowerExtend, upperExtend), size, step,
                                 out range, modes, direction);

        public bool TryGetRandomRange(in GridIndex pivot, in GridIndex lowerExtend, in GridIndex upperExtend, int size, int step,
                                      out GridIndexRange range, in CellModes? modes = null,
                                      GridDirection direction = default)
            => TryGetRandomRange(pivot, this.ClampedSize.IndexRange(pivot, lowerExtend, upperExtend), size, step,
                                 out range, modes, direction);

        public bool TryGetRandomRange(in GridIndex pivot, in GridIndexRange pivotRange, int size, int step,
                                      out GridIndexRange range, in CellModes? modes = null, GridDirection direction = default)
        {
            range = default;

            if (size < 1)
                return false;

            if (size == 1)
            {
                var slice = this.ClampedSize.IndexRange(pivotRange, size);

                if (TryGetRandomCell(pivot, slice, out var cell, modes))
                {
                    range = new GridIndexRange(cell, cell);
                    return true;
                }

                return false;
            }

            var cache = Pool.Provider.HashSet<GridIndexRange>();
            GetRanges(pivotRange, size, step, cache, modes, false, direction);
            GetRanges(pivotRange, size, step, cache, modes, true, direction);

            var ranges = Pool.Provider.List<GridIndexRange>();
            ranges.AddRange(cache);
            Pool.Provider.Return(cache);

            return TryGetRandomRange(ref range, ranges);
        }

        private static bool TryGetRandomRange(ref GridIndexRange range, List<GridIndexRange> ranges)
        {
            var result = false;

            if (ranges.Count > 0)
            {
                var random = Pool.Provider.List<GridIndexRange>();
                random.AddRange(ranges.Randomize());
                range = random[UnityRandom.Range(0, random.Count)];

                Pool.Provider.Return(random);
                result = true;
            }

            Pool.Provider.Return(ranges);
            return result;
        }

        public void Filter(ICollection<GridIndexRange> ranges, in CellModes modes)
        {
            var cache = Pool.Provider.List<GridIndexRange>();
            cache.AddRange(ranges);
            ranges.Clear();

            Filter(null, cache, modes);
            ranges.AddRange(cache);

            Pool.Provider.Return(cache);
        }

        public void Filter(ICollection<GridIndexRange> ranges, in GridIndexRange pivot, in CellModes modes)
        {
            var cache = Pool.Provider.List<GridIndexRange>();
            cache.AddRange(ranges);
            ranges.Clear();

            Filter(pivot, cache, modes);
            ranges.AddRange(cache);

            Pool.Provider.Return(cache);
        }

        private void Filter(in GridIndexRange? pivot, List<GridIndexRange> ranges, in CellModes modes)
        {
            if (ranges.Count > 0 && pivot.HasValue)
            {
                switch (modes.Diagonal)
                {
                    case CellMode.Exclude:
                        RemoveDiagonal(pivot.Value, ranges);
                        break;

                    case CellMode.Only:
                        OnlyDiagonal(pivot.Value, ranges);
                        break;
                }
            }

            if (ranges.Count > 0 && modes.Marked != CellMode.Include)
            {
                var marked = Pool.Provider.List<GridIndex>();
                this.marker.GetIndices(marked);
                marked.Sort(new GridIndex1Comparer(this.data.Size, true));

                switch (modes.Marked)
                {
                    case CellMode.Exclude:
                        RemoveMarked(ranges, marked);
                        break;

                    case CellMode.Only:
                        OnlyMarked(ranges, marked);
                        break;
                }

                Pool.Provider.Return(marked);
            }
        }

        private void RemoveDiagonal(in GridIndexRange pivot, List<GridIndexRange> ranges)
        {
            var normalPivot = pivot.Normalize();

            for (var i = ranges.Count - 1; i >= 0; i--)
            {
                var normal = ranges[i].Normalize();

                if (normal.Start.Row != normalPivot.Start.Row && normal.Start.Column != normalPivot.Start.Column &&
                    normal.End.Row != normalPivot.End.Row && normal.End.Column != normalPivot.End.Column)
                    ranges.RemoveAt(i);
            }
        }

        private void OnlyDiagonal(in GridIndexRange pivot, List<GridIndexRange> ranges)
        {
            var normalPivot = pivot.Normalize();

            for (var i = ranges.Count - 1; i >= 0; i--)
            {
                var normal = ranges[i].Normalize();

                if (normal.Start.Row == normalPivot.Start.Row || normal.Start.Column == normalPivot.Start.Column ||
                    normal.End.Row == normalPivot.End.Row || normal.End.Column == normalPivot.End.Column)
                    ranges.RemoveAt(i);
            }
        }

        private void RemoveMarked(List<GridIndexRange> ranges, List<GridIndex> marked)
        {
            for (var i = ranges.Count - 1; i >= 0; i--)
            {
                foreach (var index in marked)
                {
                    if (ranges[i].Contains(index))
                    {
                        ranges.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        private void OnlyMarked(List<GridIndexRange> ranges, List<GridIndex> marked)
        {
            for (var i = ranges.Count - 1; i >= 0; i--)
            {
                foreach (var index in marked)
                {
                    if (!ranges[i].Contains(index))
                    {
                        ranges.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        public static implicit operator ReadGrid<Cell>(ReadCellGrid value)
            => value?.data ?? ReadGrid<Cell>.Empty;
    }
}