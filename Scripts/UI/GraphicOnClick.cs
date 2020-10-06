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
        private Color color = Color.white;

        private Color backupColor;

        private void Awake()
        {
            this.backupColor = this.graphic.color;
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            this.graphic.color = this.backupColor;
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            this.graphic.color = this.color;
        }
    }
}