using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HegaCore.UI
{
    public class GraphicOnClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField]
        private Graphic graphic = null;

        [SerializeField]
        private Color defaultColor = Color.white;

        [SerializeField]
        private Color clickColor = Color.white;

        private void Awake()
        {
            this.graphic.color = this.defaultColor;
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
            => this.graphic.color = this.defaultColor;

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
            => this.graphic.color = this.clickColor;
    }
}