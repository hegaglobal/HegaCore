using System.Collections.Generic;
using System.Grid;
using System;

namespace HegaCore
{
    using UnityRandom = UnityEngine.Random;

    public partial class ReadCellGrid
    {
        /// <summary>
        /// Default params:
        /// <para>IncludeOccupied = true</para>
        /// <para>IncludeDiagonal = true</para>
        /// </summary>
        public QueryParams Params { get; } = new QueryParams(true, true);

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

        private void RemoveOccupied(List<Cell> cells, ICollection<Cell> output)
        {
            var occupied = HashSetPool<GridIndex>.Get();
            this.occupier.GetOccupied(occupied);

            foreach (var cell in cells)
            {
                if (!occupied.Contains(cell))
                    output.Add(cell);
            }

            HashSetPool<GridIndex>.Return(occupied);
        }

        private void RemoveDiagonal(in GridIndex pivot, List<Cell> cells)
        {
            for (var i = cells.Count - 1; i >= 0; i--)
            {
                var index = cells[i].Index;

                if (index.Column != pivot.Column && index.Row != pivot.Row)
                    cells.RemoveAt(i);
            }
        }

        private void GetCells(in GridIndex? pivot, List<Cell> cells, ICollection<Cell> output, in QueryParams @params)
        {
            if (pivot.HasValue && !@params.IncludeDiagonal)
                RemoveDiagonal(pivot.Value, cells);

            if (!@params.IncludeOccupied)
                RemoveOccupied(cells, output);

            output.AddRange(cells, false, true);
            ListPool<Cell>.Return(cells);
        }

        public void GetCells(bool allowOccupied, ICollection<Cell> output)
        {
            if (allowOccupied)
            {
                this.data.GetValues(output);
                return;
            }

            var cells = ListPool<Cell>.Get();
            this.data.GetValues(cells);
            RemoveOccupied(cells, output);
        }

        public void GetCells(in GridIndex pivot, int extend, ICollection<Cell> output, in QueryParams? @params = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(pivot, extend, cells);

            GetCells(pivot, cells, output, @params ?? this.Params);
        }

        public void GetCells(in GridIndex pivot, int lowerExtend, int upperExtend, ICollection<Cell> output, in QueryParams? @params = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(pivot, lowerExtend, upperExtend, cells);

            GetCells(pivot, cells, output, @params ?? this.Params);
        }

        public void GetCells(in GridIndex pivot, bool byRow, ICollection<Cell> output, in QueryParams? @params = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(pivot, byRow, cells);

            GetCells(pivot, cells, output, @params ?? this.Params);
        }

        public void GetCells(in GridIndex pivot, in GridIndex extend, ICollection<Cell> output, in QueryParams? @params = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(pivot, extend, cells);

            GetCells(pivot, cells, output, @params ?? this.Params);
        }

        public void GetCells(in GridIndex pivot, in GridIndex lowerExtend, in GridIndex upperExtend, ICollection<Cell> output, in QueryParams? @params = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(pivot, lowerExtend, upperExtend, cells);

            GetCells(pivot, cells, output, @params ?? this.Params);
        }

        public void GetCells(in GridIndex pivot, ICollection<Cell> output, in QueryParams? @params = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(output);

            GetCells(pivot, cells, output, @params ?? this.Params);
        }

        public void GetCells(in GridIndex pivot, in GridRange range, ICollection<Cell> output, in QueryParams? @params = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(range, output);

            GetCells(pivot, cells, output, @params ?? this.Params);
        }

        public void GetCells(in GridIndex pivot, in GridIndexRange range, ICollection<Cell> output, in QueryParams? @params = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(range, output);

            GetCells(pivot, cells, output, @params ?? this.Params);
        }

        public void GetCells(in GridIndex pivot, IEnumerable<GridIndex> indices, ICollection<Cell> output, in QueryParams? @params = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(indices, output);

            GetCells(pivot, cells, output, @params ?? this.Params);
        }

        public void GetCells(in GridIndex pivot, IEnumerator<GridIndex> enumerator, ICollection<Cell> output, in QueryParams? @params = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(enumerator, output);

            GetCells(pivot, cells, output, @params ?? this.Params);
        }

        public void GetCells(ICollection<Cell> output, in QueryParams? @params = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(output);

            GetCells(null, cells, output, @params ?? this.Params);
        }

        public void GetCells(in GridRange range, ICollection<Cell> output, in QueryParams? @params = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(range, output);

            GetCells(null, cells, output, @params ?? this.Params);
        }

        public void GetCells(in GridIndexRange range, ICollection<Cell> output, in QueryParams? @params = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(range, output);

            GetCells(null, cells, output, @params ?? this.Params);
        }

        public void GetCells(IEnumerable<GridIndex> indices, ICollection<Cell> output, in QueryParams? @params = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(indices, output);

            GetCells(null, cells, output, @params ?? this.Params);
        }

        public void GetCells(IEnumerator<GridIndex> enumerator, ICollection<Cell> output, in QueryParams? @params = null)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(enumerator, output);

            GetCells(null, cells, output, @params ?? this.Params);
        }

        public bool TryGetRandomCell(out Cell output, in QueryParams? @params = null)
        {
            var cells = ListPool<Cell>.Get();
            GetCells(cells, @params);

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

        public void GetRandomCells(int count, ICollection<Cell> output, in QueryParams? @params = null)
        {
            if (count <= 0)
                return;

            var cells = ListPool<Cell>.Get();
            GetCells(cells, @params);

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

        public void GetRandomBlock(int size, ICollection<Cell> output, in QueryParams? @params = null)
        {
            if (size < 1)
                return;

            if (size == 1)
            {
                if (TryGetRandomCell(out var cell, @params))
                    output.Add(cell);

                return;
            }

            var blockSize = this.data.Size / size;
            var blocks = ListPool<GridRange>.Get();

            ListPool<GridRange>.Return(blocks);
        }

        public static implicit operator ReadGrid<Cell>(ReadCellGrid value)
            => value?.data ?? default;
    }
}