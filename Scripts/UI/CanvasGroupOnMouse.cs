using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

namespace HegaCore.UI
{
    public class CanvasGroupOnMouse : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private CanvasGroup[] canvasGroups = new CanvasGroup[0];

        [SerializeField, BoxGroup("Alpha"), LabelText("Default")]
        private float defaultAlpha = 1f;

        [SerializeField, BoxGroup("Alpha"), LabelText("Hover")]
        private float hoverAlpha = 1f;

        [SerializeField, BoxGroup("Alpha"), LabelText("Click")]
        private float clickAlpha = 1f;

        private void Awake()
        {
            SetAlpha(this.defaultAlpha);
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
            => SetAlpha(this.clickAlpha);

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
            => SetAlpha(this.hoverAlpha);

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
            => SetAlpha(this.hoverAlpha);

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
            => SetAlpha(this.defaultAlpha);

        private void SetAlpha(float alpha)
        {
            foreach (var canvasGroup in this.canvasGroups)
            {
                if (canvasGroup)
                    canvasGroup.alpha = alpha;
            }
        }
    }
}