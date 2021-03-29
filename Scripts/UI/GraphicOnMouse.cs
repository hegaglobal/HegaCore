using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace HegaCore.UI
{
    public class GraphicOnMouse : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private Graphic[] graphics = new Graphic[0];

        [SerializeField, BoxGroup("Colors"), LabelText("Default")]
        private Color defaultColor = Color.white;

        [SerializeField, BoxGroup("Colors"), LabelText("Hover")]
        private Color hoverColor = Color.white;

        [SerializeField, BoxGroup("Colors"), LabelText("Click")]
        private Color clickColor = Color.white;

        private void Awake()
        {
            SetColor(this.defaultColor);
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
            => SetColor(this.clickColor);

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
            => SetColor(this.hoverColor);

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
            => SetColor(this.hoverColor);

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
            => SetColor(this.defaultColor);

        private void SetColor(in Color color)
        {
            foreach (var graphic in this.graphics)
            {
                if (graphic)
                    graphic.color = color;
            }
        }
    }
}