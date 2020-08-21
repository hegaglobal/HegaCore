using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace HegaCore.UI
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public sealed class GridLayoutGenerator : MonoBehaviour, IGetCellPosition
    {
        [SerializeField]
        private GridVector size = default;

        [HideInInspector]
        [SerializeField]
        private GridLayoutGroup gridLayout = null;

        [DictionaryDrawerSettings(KeyLabel = "Index", ValueLabel = "Cell")]
        [SerializeField]
        private CellMap map = new CellMap();

        public GridVector Size
        {
            get => this.size;
            set => this.size = value;
        }

        public ReadDictionary<GridVector, RectTransform> Map => this.map;

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
            this.gridLayout.constraintCount = this.size.Column;

            for (var r = 0; r < this.size.Row; r++)
            {
                for (var c = 0; c < this.size.Column; c++)
                {
                    var index = new GridVector(r, c);
                    var go = new GameObject($"{index}");
                    var rect = go.AddComponent<RectTransform>();
                    rect.SetParent(this.transform, false);
                    this.map.Add(index, rect);
                }
            }
        }

        [Button]
        public void Clear()
        {
            if (this.transform.childCount > 0)
            {
                for (var i = this.transform.childCount - 1; i >= 0; i--)
                {
#if UNITY_EDITOR
                    DestroyImmediate(this.transform.GetChild(i).gameObject);
#else
                    Destroy(this.transform.GetChild(i).gameObject);
#endif
                }
            }

            this.map.Clear();
        }

        public RectTransform GetCell(in GridVector index)
            => this.map.ContainsKey(index) ? this.map[index] : null;

        public bool TryGetCell(in GridVector index, out RectTransform cell)
            => this.map.TryGetValue(index, out cell);

        public Vector3 GetCellPosition(in GridVector index)
            => this.map.ContainsKey(index) ? this.map[index].position : this.transform.position;

        public bool TryGetCellPosition(in GridVector index, out Vector3 position)
        {
            if (!this.map.TryGetValue(index, out var cell))
            {
                position = this.transform.position;
                return false;
            }

            position = cell.position;
            return true;
        }

        [Serializable]
        private sealed class CellMap : SerializableDictionary<GridVector, RectTransform> { }
    }
}