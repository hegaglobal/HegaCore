using System.Collections.Generic;
using System.Grid;
using System;
using System.Diagnostics;

namespace HegaCore
{
    using UnityRandom = UnityEngine.Random;

    public class ReadCellGrid
    {
        public GridSize Size => this.data.Size;

        public ClampedGridSize ClampedSize => this.clampedSize;

        private readonly Grid<Cell> data;
        private readonly ClampedGridSize clampedSize;
        private readonly IGridIndexOccupier occupier;

        public ReadCellGrid(Grid<Cell> data, IGridIndexOccupier occupier)
        {
            this.data = new Grid<Cell>(data);
            this.clampedSize = this.data.Size;
            this.occupier = occupier ?? throw new ArgumentNullException();
        }

        public ReadCellGrid(in ReadGrid<Cell> data, IGridIndexOccupier occupier)
        {
            var cache = ListPool<GridValue<Cell>>.Get();
            data.GetIndexedValues(cache);

            this.data = new Grid<Cell>(data.Size, cache);
            this.clampedSize = this.data.Size;
            ListPool<GridValue<Cell>>.Return(cache);

            this.occupier = occupier ?? throw new ArgumentNullException();
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

        private void RemoveOccupied(List<Cell> cells, HashSet<GridIndex> occupied)
        {
            for (var i = cells.Count - 1; i >= 0; i--)
            {
                if (occupied.Contains(cells[i].Index))
                    cells.RemoveAt(i);
            }
        }

        private void OnlyOccupied(List<Cell> cells, HashSet<GridIndex> occupied)
        {
            for (var i = cells.Count - 1; i >= 0; i--)
            {
                if (!occupied.Contains(cells[i]))
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

            if (cells.Count > 0 && modes.Occupied != CellMode.Include)
            {
                var occupied = HashSetPool<GridIndex>.Get();
                this.occupier.GetOccupied(occupied);

                switch (modes.Occupied)
                {
                    case CellMode.Exclude:
                        RemoveOccupied(cells, occupied);
                        break;

                    case CellMode.Only:
                        OnlyOccupied(cells, occupied);
                        break;
                }

                HashSetPool<GridIndex>.Return(occupied);
            }

            output.AddRange(cells, false, true);
            ListPool<Cell>.Return(cells);
        }

        public Cell GetCell(in GridIndex index)
            => this.data[index];

        public bool TryGetCell(in GridIndex index, out Cell cell)
            => this.data.TryGetValue(index, out cell);

        public void GetCells(in GridIndex pivot, int extend, ICollection<Cell> output, in CellModes? modes = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(pivot, extend, cells);

            Filter(pivot, cells, output, modes ?? CellModes.Include);
        }

        public void GetCells(in GridIndex pivot, int lowerExtend, int upperExtend, ICollection<Cell> output, in CellModes? modes = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(pivot, lowerExtend, upperExtend, cells);

            Filter(pivot, cells, output, modes ?? CellModes.Include);
        }

        public void GetCells(in GridIndex pivot, bool byRow, ICollection<Cell> output, in CellModes? modes = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(pivot, byRow, cells);

            Filter(pivot, cells, output, modes ?? CellModes.Include);
        }

        public void GetCells(in GridIndex pivot, in GridIndex extend, ICollection<Cell> output, in CellModes? modes = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(pivot, extend, cells);

            Filter(pivot, cells, output, modes ?? CellModes.Include);
        }

        public void GetCells(in GridIndex pivot, in GridIndex lowerExtend, in GridIndex upperExtend, ICollection<Cell> output, in CellModes? modes = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(pivot, lowerExtend, upperExtend, cells);

            Filter(pivot, cells, output, modes ?? CellModes.Include);
        }

        public void GetCells(in GridIndex pivot, ICollection<Cell> output, in CellModes? modes = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(cells);

            Filter(pivot, cells, output, modes ?? CellModes.Include);
        }

        public void GetCells(in GridIndex pivot, in GridIndexRange range, ICollection<Cell> output, in CellModes? modes = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(range, cells);

            Filter(pivot, cells, output, modes ?? CellModes.Include);
        }

        public void GetCells(in GridIndex pivot, IEnumerable<GridIndex> indices, ICollection<Cell> output, in CellModes? modes = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(indices, cells);

            Filter(pivot, cells, output, modes ?? CellModes.Include);
        }

        public void GetCells(in GridIndex pivot, IEnumerator<GridIndex> enumerator, ICollection<Cell> output, in CellModes? modes = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(enumerator, cells);

            Filter(pivot, cells, output, modes ?? CellModes.Include);
        }

        public void GetCells(ICollection<Cell> output, in CellModes? modes = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(cells);

            Filter(null, cells, output, modes ?? CellModes.Include);
        }

        public void GetCells(in GridIndexRange range, ICollection<Cell> output, in CellModes? modes = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(range, cells);

            Filter(null, cells, output, modes ?? CellModes.Include);
        }

        public void GetCells(IEnumerable<GridIndex> indices, ICollection<Cell> output, in CellModes? modes = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(indices, cells);

            Filter(null, cells, output, modes ?? CellModes.Include);
        }

        public void GetCells(IEnumerator<GridIndex> enumerator, ICollection<Cell> output, in CellModes? modes = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(enumerator, cells);

            Filter(null, cells, output, modes ?? CellModes.Include);
        }

        public void GetRanges(in GridIndexRange pivot, int size, int step, ICollection<GridIndexRange> output,
                              in CellModes? modes = null)
        {
            var slice = this.clampedSize.IndexRange(pivot, size);
            var ranges = ListPool<GridIndexRange>.Get();
            this.clampedSize.IndexRanges(slice, size, step, ranges);

            Filter(pivot, ranges, modes ?? CellModes.Include);
            output.AddRange(ranges);

            ListPool<GridIndexRange>.Return(ranges);
        }

        public bool TryGetRandomCell(out Cell output, in CellModes? modes = null)
        {
            var cells = ListPool<Cell>.Get();
            GetCells(cells, modes);

            return TryGetRandomCell(cells, out output);
        }

        public bool TryGetRandomCell(in GridIndexRange range, out Cell output, in CellModes? modes = null)
        {
            var cells = ListPool<Cell>.Get();
            GetCells(range, cells, modes);

            return TryGetRandomCell(cells, out output);
        }

        public bool TryGetRandomCell(in GridIndex pivot, in GridIndexRange range, out Cell output, in CellModes? modes = null)
        {
            var cells = ListPool<Cell>.Get();
            GetCells(pivot, range, cells, modes);

            return TryGetRandomCell(cells, out output);
        }

        private bool TryGetRandomCell(List<Cell> cells, out Cell output)
        {
            if (cells.Count <= 0)
            {
                ListPool<Cell>.Return(cells);
                output = default;
                return false;
            }

            var random = ListPool<Cell>.Get();
            random.AddRange(cells.Randomize());

            var index = UnityRandom.Range(0, random.Count);
            output = random[index];

            ListPool<Cell>.Return(cells, random);
            return true;
        }

        public void GetRandomCells(int count, ICollection<Cell> output, in CellModes? modes = null)
        {
            if (count <= 0)
                return;

            var cells = ListPool<Cell>.Get();
            GetCells(cells, modes);
            GetRandomCells(count, output, cells);
        }

        public void GetRandomCells(in GridIndexRange range, int count, ICollection<Cell> output, in CellModes? modes = null)
        {
            if (count <= 0)
                return;

            var cells = ListPool<Cell>.Get();
            GetCells(range, cells, modes);
            GetRandomCells(count, output, cells);
        }

        private static void GetRandomCells(int count, ICollection<Cell> output, List<Cell> cells)
        {
            if (cells.Count > 0)
            {
                var random = ListPool<Cell>.Get();
                random.AddRange(cells.Randomize());

                foreach (var _ in IntRange.Count(count))
                {
                    if (random.Count <= 0)
                        break;

                    var index = UnityRandom.Range(0, random.Count);
                    output.Add(random[index]);

                    random.RemoveAt(index);
                }

                ListPool<Cell>.Return(random);
            }

            ListPool<Cell>.Return(cells);
        }

        public bool TryGetRandomRange(int size, out GridIndexRange range, in CellModes? modes = null)
            => TryGetRandomRange(size, size, out range, modes);

        public bool TryGetRandomRange(int size, int step, out GridIndexRange range, in CellModes? modes = null)
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

            var ranges = ListPool<GridIndexRange>.Get();
            this.clampedSize.IndexRanges(size, step, ranges);
            Filter(null, ranges, modes ?? CellModes.Include);

            return TryGetRandomRange(ref range, ranges);
        }

        public bool TryGetRandomRange(in GridIndex pivot, int extend, int size, int step,
                                      out GridIndexRange range, in CellModes? modes = null)
            => TryGetRandomRange(pivot, this.clampedSize.IndexRange(pivot, extend), size, step,
                                 out range, modes);

        public bool TryGetRandomRange(in GridIndex pivot, int lowerExtend, int upperExtend, int size, int step,
                                      out GridIndexRange range, in CellModes? modes = null)
            => TryGetRandomRange(pivot, this.clampedSize.IndexRange(pivot, lowerExtend, upperExtend), size, step,
                                 out range, modes);

        public bool TryGetRandomRange(in GridIndex pivot, in GridIndex lowerExtend, in GridIndex upperExtend, int size, int step,
                                      out GridIndexRange range, in CellModes? modes = null)
            => TryGetRandomRange(pivot, this.clampedSize.IndexRange(pivot, lowerExtend, upperExtend), size, step,
                                 out range, modes);

        public bool TryGetRandomRange(in GridIndex pivot, in GridIndexRange pivotRange, int size, int step,
                                      out GridIndexRange range, in CellModes? modes = null)
        {
            range = default;

            if (size < 1)
                return false;

            if (size == 1)
            {
                var slice = this.clampedSize.IndexRange(pivotRange, size);

                if (TryGetRandomCell(pivot, slice, out var cell, modes))
                {
                    range = new GridIndexRange(cell, cell);
                    return true;
                }

                return false;
            }

            var ranges = ListPool<GridIndexRange>.Get();
            GetRanges(pivotRange, size, step, ranges, modes);

            return TryGetRandomRange(ref range, ranges);
        }

        private static bool TryGetRandomRange(ref GridIndexRange range, List<GridIndexRange> ranges)
        {
            var result = false;

            if (ranges.Count > 0)
            {
                var random = ListPool<GridIndexRange>.Get();
                random.AddRange(ranges.Randomize());
                range = random[UnityRandom.Range(0, random.Count)];

                ListPool<GridIndexRange>.Return(random);
                result = true;
            }

            ListPool<GridIndexRange>.Return(ranges);
            return result;
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

            if (ranges.Count > 0 && modes.Occupied != CellMode.Include)
            {
                var occupied = ListPool<GridIndex>.Get();
                this.occupier.GetOccupied(occupied);
                occupied.Sort(new GridIndex1Comparer(this.data.Size, true));

                switch (modes.Occupied)
                {
                    case CellMode.Exclude:
                        RemoveOccupied(ranges, occupied);
                        break;

                    case CellMode.Only:
                        OnlyOccupied(ranges, occupied);
                        break;
                }

                ListPool<GridIndex>.Return(occupied);
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

        private void RemoveOccupied(List<GridIndexRange> ranges, List<GridIndex> occupied)
        {
            for (var i = ranges.Count - 1; i >= 0; i--)
            {
                foreach (var index in occupied)
                {
                    if (ranges[i].Contains(index))
                    {
                        ranges.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        private void OnlyOccupied(List<GridIndexRange> ranges, List<GridIndex> occupied)
        {
            for (var i = ranges.Count - 1; i >= 0; i--)
            {
                foreach (var index in occupied)
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