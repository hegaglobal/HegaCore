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
        private int cellCount = 0;

        [HideInInspector]
        [SerializeField]
        private GridLayoutGroup gridLayout = null;

        [DictionaryDrawerSettings(KeyLabel = "Index", ValueLabel = "Cell")]
        [SerializeField]
        private CellMap map = new CellMap();

        public int CellCount
        {
            get => this.cellCount;
            set => this.cellCount = Mathf.Max(value, 0);
        }

        public ReadDictionary<GridIndex, RectTransform> Map => this.map;

        [Button]
        public void Generate()
        {
            Clear();

            if (!this.gridLayout)
                this.gridLayout = GetComponent<GridLayoutGroup>();

            switch (this.gridLayout.constraint)
            {
                case GridLayoutGroup.Constraint.FixedColumnCount:
                {
                    var column = Mathf.Max(this.gridLayout.constraintCount, 1);
                    var row = this.cellCount / column;
                    Generate(row, column);
                }
                    break;

                case GridLayoutGroup.Constraint.FixedRowCount:
                {
                    var row = Mathf.Max(this.gridLayout.constraintCount, 1);
                    var column = this.cellCount / row;
                    Generate(row, column);
                }
                    break;

                default:
                    Debug.LogError("Cannot generate grid with flexible constraint");
                    break;
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

        private void Generate(int row, int column)
        {
            for (var r = 0; r < row; r++)
            {
                for (var c = 0; c < column; c++)
                {
                    var index = new GridIndex(r, c);
                    var go = new GameObject($"{index}");
                    var rect = go.AddComponent<RectTransform>();
                    rect.SetParent(this.transform, false);
                    this.map.Add(index, rect);
                }
            }
        }

        [Serializable]
        private sealed class CellMap : SerializableDictionary<GridIndex, RectTransform> { }
    }
}