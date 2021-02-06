using System;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace HegaCore.UI
{
    public class FlexibleGridLayoutGroup : LayoutGroup
    {
        public enum Alignment
        {
            Horizontal,
            Vertical
        }

        public enum FitType
        {
            Uniform,
            Width,
            Height,
            FixedRowCount,
            FixedColumnCount,
            FixedBoth
        }

        public Alignment alignment = Alignment.Horizontal;
        public FitType fitType = FitType.Uniform;
        public GridSize gridSize = GridSize.Zero;
        public Vector2 cellSize = Vector2.zero;

        [Indent(1)]
        public FitVector fit = FitVector.True;
        public Vector2 spacing = Vector2.zero;

        public override void CalculateLayoutInputVertical()
        {
            base.CalculateLayoutInputHorizontal();

            if (this.fitType == FitType.Width || this.fitType == FitType.Height || this.fitType == FitType.Uniform)
            {
                if (this.fitType == FitType.Uniform)
                    this.fit = FitVector.True;
                else
                {
                    this.fit.X = this.fitType == FitType.Width;
                    this.fit.Y = this.fitType == FitType.Height;
                }

                var sqrRt = Mathf.Sqrt(this.transform.childCount);
                this.gridSize.Row = Mathf.CeilToInt(sqrRt);
                this.gridSize.Column = Mathf.CeilToInt(sqrRt);

            }

            if (this.fitType == FitType.Width || this.fitType == FitType.FixedColumnCount || this.fitType == FitType.Uniform)
                this.gridSize.Row = Mathf.CeilToInt(this.transform.childCount / (float)this.gridSize.Column);

            if (this.fitType == FitType.Height || this.fitType == FitType.FixedRowCount || this.fitType == FitType.Uniform)
                this.gridSize.Column = Mathf.CeilToInt(this.transform.childCount / (float)this.gridSize.Row);

            var rect = this.rectTransform.rect;
            var parentWidth = rect.width;
            var parentHeight = rect.height;

            float cellWidth;
            float cellHeight;

            if (this.alignment == Alignment.Horizontal)
            {
                cellWidth = (parentWidth / this.gridSize.Column) -
                            ((this.spacing.x / this.gridSize.Column) * (this.gridSize.Column - 1)) -
                            (this.padding.left / (float)this.gridSize.Column) -
                            (this.padding.right / (float)this.gridSize.Column);

                cellHeight = (parentHeight / this.gridSize.Row) -
                             ((this.spacing.y / this.gridSize.Row) * (this.gridSize.Row - 1)) -
                             (this.padding.top / (float)this.gridSize.Row) -
                             (this.padding.bottom / (float)this.gridSize.Row);
            }
            else
            {
                cellHeight = (parentWidth / this.gridSize.Column) -
                             ((this.spacing.x / this.gridSize.Column) * (this.gridSize.Column - 1)) -
                             (this.padding.left / (float)this.gridSize.Column) -
                             (this.padding.right / (float)this.gridSize.Column);

                cellWidth = (parentHeight / this.gridSize.Row) -
                            ((this.spacing.y / this.gridSize.Row) * (this.gridSize.Row - 1)) -
                            (this.padding.top / (float)this.gridSize.Row) -
                            (this.padding.bottom / (float)this.gridSize.Row);
            }

            this.cellSize.x = this.fit.X ? cellWidth : this.cellSize.x;
            this.cellSize.y = this.fit.Y ? cellHeight : this.cellSize.y;

            for (var i = 0; i < this.rectChildren.Count; i++)
            {
                var item = this.rectChildren[i];

                int columnCount;
                int rowCount;

                if (this.alignment == Alignment.Horizontal)
                {
                    rowCount = i / this.gridSize.Column;
                    columnCount = i % this.gridSize.Column;

                    var xPos = (this.cellSize.x * columnCount) + (this.spacing.x * columnCount) + this.padding.left;
                    var yPos = (this.cellSize.y * rowCount) + (this.spacing.y * rowCount) + this.padding.top;

                    SetChildAlongAxis(item, 0, xPos, this.cellSize.x);
                    SetChildAlongAxis(item, 1, yPos, this.cellSize.y);
                }
                else
                {
                    rowCount = i / this.gridSize.Row;
                    columnCount = i % this.gridSize.Row;

                    var xPos = (this.cellSize.x * columnCount) + (this.spacing.x * columnCount) + this.padding.left;
                    var yPos = (this.cellSize.y * rowCount) + (this.spacing.y * rowCount) + this.padding.top;

                    SetChildAlongAxis(item, 0, yPos, this.cellSize.y);
                    SetChildAlongAxis(item, 1, xPos, this.cellSize.x);
                }
            }
        }

        public override void SetLayoutHorizontal() { }

        public override void SetLayoutVertical() { }

        [InlineProperty]
        [Serializable]
        public struct GridSize
        {
            [HorizontalGroup(PaddingLeft = 6), LabelText("R"), LabelWidth(12), Tooltip(nameof(Row))]
            public int Row;

            [HorizontalGroup, LabelText("C"), LabelWidth(12), Tooltip(nameof(Column))]
            public int Column;

            public GridSize(int row, int column)
            {
                this.Row = row;
                this.Column = column;
            }

            public static GridSize Zero { get; } = new GridSize(0, 0);
        }

        [InlineProperty]
        [Serializable]
        public struct FitVector
        {
            [HorizontalGroup(PaddingLeft = 6), LabelWidth(12)]
            public bool X;

            [HorizontalGroup, LabelWidth(12)]
            public bool Y;

            public FitVector(bool x, bool y)
            {
                this.X = x;
                this.Y = y;
            }

            public static FitVector False { get; } = new FitVector(false, false);

            public static FitVector True { get; } = new FitVector(true, true);
        }
    }
}