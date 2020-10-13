using UnityEngine;
using Cysharp.Threading.Tasks;

namespace HegaCore.UI
{
    [RequireComponent(typeof(Panel))]
    public sealed class AutoHidePanel : MonoBehaviour
    {
        [HideInInspector]
        [SerializeField]
        private Panel panel = null;

        [SerializeField]
        private float showingTime = 0f;

        private float autoHideAfter;
        private bool isShowing;
        private bool isAutoHiding;
        private float elapsed;

        private void OnValidate()
        {
            this.panel = GetComponent<Panel>();
        }

        private void Awake()
        {
            this.panel.OnShowComplete.AddListener(OnPanelShowComplete);
        }

        public void Show()
            => Show(this.showingTime);

        public void Show(float autoHideAfter)
        {
            this.elapsed = 0f;
            this.autoHideAfter = autoHideAfter;

            if (this.isShowing)
                return;

            this.isShowing = true;
            this.isAutoHiding = false;
            this.panel.Show();
        }

        private void OnPanelShowComplete()
        {
            this.isAutoHiding = true;
        }

        private void Update()
        {
            if (!this.isAutoHiding)
                return;

            this.elapsed += Time.smoothDeltaTime;

            if (this.elapsed < this.autoHideAfter)
                return;

            this.isAutoHiding = false;
            this.isShowing = false;
            this.elapsed = 0f;
            this.panel.Hide();
        }
    }
}