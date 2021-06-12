using UnityEngine;

namespace HegaCore
{
    public class ForcedSortingGroup : MonoBehaviour
    {
        [SerializeField]
        private SortingLayerId sortingLayer;

        [SerializeField, Min(0)]
        private int orderInLayer = 0;

        private Renderer[] renderers = new Renderer[0];

        private void Awake()
        {
            this.renderers = GetComponentsInChildren<Renderer>().OrEmpty();

            foreach (var renderer in this.renderers)
            {
                renderer.sortingLayerID = this.sortingLayer.id;
                renderer.sortingOrder = this.orderInLayer;
            }
        }
    }
}
