using System.Collections.Generic;
using System.Grid;
using System;

namespace HegaCore
{
    using UnityRandom = UnityEngine.Random;

    public class ReadCellGrid
    {
        private readonly Grid<Cell> data;
        private readonly IGetOccupiedGridIndex occupiedGetter;

        public ReadCellGrid(Grid<Cell> data, IGetOccupiedGridIndex occupiedGetter)
        {
            this.data = new Grid<Cell>(data);
            this.occupiedGetter = occupiedGetter ?? throw new ArgumentNullException();
        }

        public ReadCellGrid(in ReadGrid<Cell> data, IGetOccupiedGridIndex occupiedGetter)
        {
            var cache = ListPool<GridValue<Cell>>.Get();
            data.GetIndexedValues(cache);

            this.data = new Grid<Cell>(data.Size, cache);
            ListPool<GridValue<Cell>>.Return(cache);

            this.occupiedGetter = occupiedGetter ?? throw new ArgumentNullException();
        }

        private void GetUnoccupiedCells(List<Cell> cells, ICollection<Cell> output)
        {
            var occupied = HashSetPool<GridIndex>.Get();
            this.occupiedGetter.GetOccupiedGridIndex(occupied);

            foreach (var cell in cells)
            {
                if (!occupied.Contains(cell))
                    output.Add(cell);
            }

            HashSetPool<GridIndex>.Return(occupied);
            ListPool<Cell>.Return(cells);
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

        public void GetCells(bool allowOccupied, ICollection<Cell> output)
        {
            if (allowOccupied)
            {
                this.data.GetValues(output);
                return;
            }

            var cells = ListPool<Cell>.Get();
            this.data.GetValues(cells);
            GetUnoccupiedCells(cells, output);
        }

        public void GetCells(in GridIndex pivot, int extend, bool allowOccupied, ICollection<Cell> output)
        {
            if (allowOccupied)
            {
                this.data.GetValues(pivot, extend, output);
                return;
            }

            var cells = ListPool<Cell>.Get();
            this.data.GetValues(pivot, extend, cells);
            GetUnoccupiedCells(cells, output);
        }

        public void GetCells(in GridIndex pivot, int extend, bool allowDiagonal, bool allowOccupied, ICollection<Cell> output)
        {
            var cells = ListPool<Cell>.Get();
            this.data.GetValues(pivot, extend, cells);

            if (!allowDiagonal)
                RemoveDiagonal(pivot, cells);

            if (!allowOccupied)
                GetUnoccupiedCells(cells, output);

            output.AddRange(cells, false, false);
            ListPool<Cell>.Return(cells);
        }

        public void GetCells(in GridIndex pivot, int lowerExtend, int upperExtend, bool allowOccupied, ICollection<Cell> output)
        {
            var range = this.data.IndexRange(pivot, GridIndex.One * lowerExtend, GridIndex.One * upperExtend);

            if (allowOccupied)
            {
                this.data.GetValues(range, output);
                return;
            }

            var cells = ListPool<Cell>.Get();
            this.data.GetValues(range, cells);
            GetUnoccupiedCells(cells, output);
        }

        public void GetCells(in GridIndex pivot, bool byRow, bool allowOccupied, ICollection<Cell> output)
        {
            if (allowOccupied)
            {
                this.data.GetValues(pivot, byRow, output);
                return;
            }

            var cells = ListPool<Cell>.Get();
            this.data.GetValues(pivot, byRow, cells);
            GetUnoccupiedCells(cells, output);
        }

        public void GetCells(in GridIndex pivot, in GridIndex extend, bool allowOccupied, ICollection<Cell> output)
        {
            if (allowOccupied)
            {
                this.data.GetValues(pivot, extend, output);
                return;
            }

            var cells = ListPool<Cell>.Get();
            this.data.GetValues(pivot, extend, cells);
            GetUnoccupiedCells(cells, output);
        }

        public void GetCells(in GridIndex pivot, in GridIndex lowerExtend, in GridIndex upperExtend, bool allowOccupied, ICollection<Cell> output)
        {
            if (allowOccupied)
            {
                this.data.GetValues(pivot, lowerExtend, upperExtend, output);
                return;
            }

            var cells = ListPool<Cell>.Get();
            this.data.GetValues(pivot, lowerExtend, upperExtend, cells);
            GetUnoccupiedCells(cells, output);
        }

        public void GetCells(in GridRange range, bool allowOccupied, ICollection<Cell> output)
        {
            if (allowOccupied)
            {
                this.data.GetValues(range, output);
                return;
            }

            var cells = ListPool<Cell>.Get();
            this.data.GetValues(range, cells);
            GetUnoccupiedCells(cells, output);
        }

        public void GetCells(in GridIndexRange range, bool allowOccupied, ICollection<Cell> output)
        {
            if (allowOccupied)
            {
                this.data.GetValues(range, output);
                return;
            }

            var cells = ListPool<Cell>.Get();
            this.data.GetValues(range, cells);
            GetUnoccupiedCells(cells, output);
        }

        public void GetCells(IEnumerable<GridIndex> indices, bool allowOccupied, ICollection<Cell> output)
        {
            if (allowOccupied)
            {
                this.data.GetValues(indices, output);
                return;
            }

            var cells = ListPool<Cell>.Get();
            this.data.GetValues(indices, cells);
            GetUnoccupiedCells(cells, output);
        }

        public void GetCells(IEnumerator<GridIndex> enumerator, bool allowOccupied, ICollection<Cell> output)
        {
            if (allowOccupied)
            {
                this.data.GetValues(enumerator, output);
                return;
            }

            var cells = ListPool<Cell>.Get();
            this.data.GetValues(enumerator, cells);
            GetUnoccupiedCells(cells, output);
        }

        public bool TryGetRandomCell(bool allowOccupied, out Cell output)
        {
            var cells = ListPool<Cell>.Get();

            GetCells(allowOccupied, cells);

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

        public void GetRandomCells(int count, bool allowOccupied, ICollection<Cell> output)
        {
            if (count <= 0)
                return;

            var cells = ListPool<Cell>.Get();

            GetCells(allowOccupied, cells);

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

        public static implicit operator ReadGrid<Cell>(ReadCellGrid value)
            => value?.data ?? default;
    }
}