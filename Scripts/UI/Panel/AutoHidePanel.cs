using UnityEngine;

namespace HegaCore.UI
{
    [RequireComponent(typeof(Panel))]
    public sealed class AutoHidePanel : MonoBehaviour
    {
        [HideInInspector]
        [SerializeField]
        private Panel panel = null;

        [SerializeField, UnityEngine.Serialization.FormerlySerializedAs("showingTime")]
        private float showingDuration = 0f;

        private float duration;
        private bool isShowing;
        private bool isAutoHiding;
        private float elapsed;

        private void OnValidate()
        {
            this.panel = GetComponent<Panel>();
        }

        private void Awake()
        {
            if (!this.panel)
                this.panel = GetComponent<Panel>();

            this.panel.OnShowComplete.AddListener(OnPanelShowComplete);
        }

        public void Show()
            => Show(this.showingDuration);

        public void Show(float duration)
        {
            this.elapsed = 0f;
            this.duration = duration;

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

        public void Hide()
        {
            this.isShowing = false;
            this.isAutoHiding = false;
            this.elapsed = 0f;
            this.panel.Hide();
        }

        private void Update()
        {
            if (!this.isAutoHiding)
                return;

            this.elapsed += GameTime.Provider.DeltaTime;

            if (this.elapsed < this.duration)
                return;

            Hide();
        }
    }
}