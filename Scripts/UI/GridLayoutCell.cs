using UnityEngine;
using Sirenix.OdinInspector;

namespace HegaCore.UI
{
    public class GridLayoutCell : MonoBehaviour
    {
        [SerializeField, ReadOnly]
        private GridVector gridIndex = default;

        public GridVector GridIndex
        {
            get => this.gridIndex;
            set => this.gridIndex = value;
        }
    }
}
