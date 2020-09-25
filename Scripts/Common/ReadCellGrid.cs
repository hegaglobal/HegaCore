using System.Collections.Generic;
using System.Grid;
using System;

namespace HegaCore
{
    using UnityRandom = UnityEngine.Random;

    public class ReadCellGrid
    {
        private readonly Grid<Cell> data;
        private readonly IGridIndexOccupier occupier;

        public ReadCellGrid(Grid<Cell> data, IGridIndexOccupier occupier)
        {
            this.data = new Grid<Cell>(data);
            this.occupier = occupier ?? throw new ArgumentNullException();
        }

        public ReadCellGrid(in ReadGrid<Cell> data, IGridIndexOccupier occupier)
        {
            var cache = ListPool<GridValue<Cell>>.Get();
            data.GetIndexedValues(cache);

            this.data = new Grid<Cell>(data.Size, cache);
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
                if (occupied.Contains(cells[i]))
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
            if (pivot.HasValue)
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

            output.AddRange(cells, false, true);

            HashSetPool<GridIndex>.Return(occupied);
            ListPool<Cell>.Return(cells);
        }

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
            this.data.GetValues(output);

            Filter(pivot, cells, output, modes ?? CellModes.Include);
        }

        public void GetCells(in GridIndex pivot, in GridRange range, ICollection<Cell> output, in CellModes? modes = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(range, output);

            Filter(pivot, cells, output, modes ?? CellModes.Include);
        }

        public void GetCells(in GridIndex pivot, in GridIndexRange range, ICollection<Cell> output, in CellModes? modes = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(range, output);

            Filter(pivot, cells, output, modes ?? CellModes.Include);
        }

        public void GetCells(in GridIndex pivot, IEnumerable<GridIndex> indices, ICollection<Cell> output, in CellModes? modes = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(indices, output);

            Filter(pivot, cells, output, modes ?? CellModes.Include);
        }

        public void GetCells(in GridIndex pivot, IEnumerator<GridIndex> enumerator, ICollection<Cell> output, in CellModes? modes = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(enumerator, output);

            Filter(pivot, cells, output, modes ?? CellModes.Include);
        }

        public void GetCells(ICollection<Cell> output, in CellModes? modes = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(output);

            Filter(null, cells, output, modes ?? CellModes.Include);
        }

        public void GetCells(in GridRange range, ICollection<Cell> output, in CellModes? modes = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(range, output);

            Filter(null, cells, output, modes ?? CellModes.Include);
        }

        public void GetCells(in GridIndexRange range, ICollection<Cell> output, in CellModes? modes = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(range, output);

            Filter(null, cells, output, modes ?? CellModes.Include);
        }

        public void GetCells(IEnumerable<GridIndex> indices, ICollection<Cell> output, in CellModes? modes = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(indices, output);

            Filter(null, cells, output, modes ?? CellModes.Include);
        }

        public void GetCells(IEnumerator<GridIndex> enumerator, ICollection<Cell> output, in CellModes? modes = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(enumerator, output);

            Filter(null, cells, output, modes ?? CellModes.Include);
        }

        public bool TryGetRandomCell(out Cell output, in CellModes? modes = null)
        {
            var cells = ListPool<Cell>.Get();
            GetCells(cells, modes);

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

        public void GetRandomBlock(int size, ICollection<Cell> output, in CellModes? modes = null)
            => GetRandomBlock(size, size, output, modes);

        public void GetRandomBlock(int size, int step, ICollection<Cell> output, in CellModes? modes = null)
        {
            if (size < 1)
                return;

            if (size == 1)
            {
                if (TryGetRandomCell(out var cell, modes))
                    output.Add(cell);

                return;
            }

            var ranges = ListPool<GridIndexRange>.Get();
            var gridSize = (ClampedGridSize)this.data.Size;
            gridSize.IndexRanges(size, step, ranges);

            if (ranges.Count > 0)
            {
                var occupied = ListPool<GridIndex>.Get();
                this.occupier.GetOccupied(occupied);
                occupied.Sort(new Index1Comparer(this.data.Size, true));

                var cModes = modes ?? CellModes.Include;

                switch (cModes.Occupied)
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

            if (ranges.Count > 0)
            {
                var random = ListPool<GridIndexRange>.Get();
                random.AddRange(ranges.Randomize());

                var range = random[UnityRandom.Range(0, random.Count)];
                this.data.GetValues(range, output);

                ListPool<GridIndexRange>.Return(random);
            }

            ListPool<GridIndexRange>.Return(ranges);
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

        private readonly struct Index1Comparer : IComparer<GridIndex>
        {
            private readonly GridSize size;
            private readonly bool reversed;

            public Index1Comparer(in GridSize size) : this(size, false)
            {
            }

            public Index1Comparer(in GridSize size, bool reversed)
            {
                this.size = size;
                this.reversed = reversed;
            }

            public int Compare(GridIndex x, GridIndex y)
                => this.reversed
                   ? this.size.Index1Of(y).CompareTo(this.size.Index1Of(x))
                   : this.size.Index1Of(x).CompareTo(this.size.Index1Of(y));
        }
    }
}