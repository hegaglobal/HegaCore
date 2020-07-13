using UnityEngine;

namespace HegaCore
{
    public abstract class TutorialStep : MonoBehaviour
    {
        public void Show(params object[] values)
        {
            if (!this)
                return;

            if (!this.gameObject.activeSelf)
                this.gameObject.SetActive(true);

            OnShow(values);
        }

        public void Hide()
        {
            if (!this)
                return;

            if (this.gameObject.activeSelf)
                this.gameObject.SetActive(false);

            OnHide();
        }

        protected virtual void OnShow(params object[] values) { }

        protected virtual void OnHide() { }
    }
}