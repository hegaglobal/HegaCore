using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace HegaCore.UI
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public sealed class GridLayoutGenerator : MonoBehaviour
    {
        [SerializeField]
        private GridVector gridSize = default;

        [SerializeField]
        private bool useCellPrefab = false;

        [SerializeField, ShowIf(nameof(useCellPrefab)), Required]
        private GameObject cellPrefab = null;

        [SerializeField]
        private bool usePool = false;

        [SerializeField, ShowIf(nameof(usePool)), Required]
        private Transform poolRoot = null;

        [HideInInspector]
        [SerializeField]
        private GridLayoutGroup gridLayout = null;

        [DictionaryDrawerSettings(KeyLabel = "Index", ValueLabel = "Cell")]
        [SerializeField]
        private CellMap cells = new CellMap();

        public GridVector GridSize
        {
            get => this.gridSize;
            set => this.gridSize = value;
        }

        public Vector2 CellSize
        {
            get => this.gridLayout.cellSize;
            set => this.gridLayout.cellSize = value;
        }

        public Vector2 CellSpacing
        {
            get => this.gridLayout.spacing;
            set => this.gridLayout.spacing = value;
        }

        public ReadDictionary<GridVector, GridLayoutCell> Cells => this.cells;

        private void EnsureGridLayout()
        {
            if (!this.gridLayout)
                this.gridLayout = GetComponent<GridLayoutGroup>();

            if (!this.gridLayout)
                this.gridLayout = this.gameObject.AddComponent<GridLayoutGroup>();
        }

        [Button]
        public void Generate()
        {
            Clear();
            EnsureGridLayout();

            this.gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            this.gridLayout.constraintCount = this.gridSize.Column;

            if (this.useCellPrefab && this.cellPrefab)
                GeneratePrefab();
            else
                GenerateDefault();
        }

        private void GenerateDefault()
        {
            for (var r = 0; r < this.gridSize.Row; r++)
            {
                for (var c = 0; c < this.gridSize.Column; c++)
                {
                    GenerateDefault(new GridVector(r, c));
                }
            }
        }

        private void GeneratePrefab()
        {
            for (var r = 0; r < this.gridSize.Row; r++)
            {
                for (var c = 0; c < this.gridSize.Column; c++)
                {
                    GeneratePrefab(new GridVector(r, c));
                }
            }
        }

        private void GenerateDefault(in GridVector index)
        {
            if (!TryGetFromPool(out var go))
            {
                go = new GameObject();
                go.AddComponent<RectTransform>();
            }

            AddToMap(go, index);
        }

        private void GeneratePrefab(in GridVector index)
        {
            if (!TryGetFromPool(out var go))
            {
                go = Instantiate(this.cellPrefab, Vector3.zero, Quaternion.identity);
            }

            AddToMap(go, index);
        }

        private void AddToMap(GameObject go, in GridVector index)
        {
            go.name = $"{index}";
            go.transform.SetParent(this.transform, false);

            var cell = go.GetOrAddComponent<GridLayoutCell>();
            cell.GridIndex = index;

            this.cells.Add(index, cell);
        }

        private bool TryGetFromPool(out GameObject go)
        {
            if (this.usePool && this.poolRoot &&
                this.poolRoot.childCount > 0)
            {
                go = this.poolRoot.GetChild(0).gameObject;
            }
            else
            {
                go = null;
            }

            return go;
        }

        [Button]
        public void Clear()
        {
            if (this.transform.childCount > 0)
            {
                var pooling = this.usePool && this.poolRoot;

                for (var i = this.transform.childCount - 1; i >= 0; i--)
                {
                    TryDestroy(this.transform.GetChild(i), pooling);
                }
            }

            this.cells.Clear();
        }

        private void TryDestroy(Transform cell, bool pooling)
        {
            if (pooling)
            {
                cell.SetParent(this.poolRoot, false);
                return;
            }

#if UNITY_EDITOR
            if (Application.isPlaying)
                Destroy(cell.gameObject);
            else
                DestroyImmediate(cell.gameObject);
#else
                Destroy(cell.gameObject);
#endif
        }


        [Serializable]
        private sealed class CellMap : SerializableDictionary<GridVector, GridLayoutCell> { }
    }
}