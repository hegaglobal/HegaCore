using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

namespace HegaCore.UI
{
    public class CanvasGroupOnClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField]
        private CanvasGroup[] canvasGroups = new CanvasGroup[0];

        [SerializeField, BoxGroup("Alpha"), LabelText("Default")]
        private float defaultAlpha = 1f;

        [SerializeField, BoxGroup("Alpha"), LabelText("Click")]
        private float clickAlpha = 1f;

        private void Awake()
        {
            SetAlpha(this.defaultAlpha);
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
            => SetAlpha(this.defaultAlpha);

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
            => SetAlpha(this.clickAlpha);

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