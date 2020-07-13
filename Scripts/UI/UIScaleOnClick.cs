using UnityEngine;
using UnityEngine.EventSystems;
using UnuGames;

namespace HegaCore
{
    public sealed class UIScaleOnClick : UIBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public float duration = 0.05f;
        public float from = 1f;
        public float to = 0.9f;

        [Space]
        public Transform customTarget = null;

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (!this.enabled)
                return;

            UITweener.Value(this.gameObject, this.duration, this.from, this.to)
                     .SetOnUpdate(Scale);
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            if (!this.enabled)
                return;

            UITweener.Value(this.gameObject, this.duration, this.to, this.from)
                     .SetOnUpdate(Scale);
        }

        private void Scale(float value)
        {
            var target = this.transform;

            if (this.customTarget)
                target = this.customTarget;

            target.localScale = Vector2.one * value;
        }
    }
}
