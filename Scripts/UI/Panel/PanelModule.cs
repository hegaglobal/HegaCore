using UnityEngine;
using UnityEngine.Events;
using UnuGames;

namespace HegaCore.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Panel))]
    public abstract class PanelModule<T> : UIManModule<T> where T : new()
    {
        private Panel m_panel;

        private Panel panel
        {
            get
            {
                EnsurePanel();
                return this.m_panel;
            }
        }

        private void EnsurePanel()
        {
            if (this.m_panel)
                return;

            this.m_panel = GetComponent<Panel>();

            if (!this.m_panel)
            {
                this.m_panel = this.gameObject.AddComponent<Panel>();

                UnuLogger.LogWarning($"The required {nameof(Panel)} component is added to {this.gameObject.name}", this.gameObject);
            }

            this.m_panel.OnShow.RemoveAllListeners();
            this.m_panel.OnShow.AddListener(OnShowMe);

            this.m_panel.OnShowComplete.RemoveAllListeners();
            this.m_panel.OnShowComplete.AddListener(OnShowMeCompleted);

            this.m_panel.OnHide.RemoveAllListeners();
            this.m_panel.OnHide.AddListener(OnHideMe);

            this.m_panel.OnHideComplete.RemoveAllListeners();
            this.m_panel.OnHideComplete.AddListener(OnHideMeCompleted);
        }

        public Panel.BehaviorAtAwake BehaviourAtAwake
        {
            get => this.panel.BehaviourAtAwake;
            set => this.panel.BehaviourAtAwake = value;
        }

        public bool InstantBehaviour
        {
            get => this.panel.InstantBehaviour;
            set => this.panel.InstantBehaviour = value;
        }

        public bool InteractableOnShow
        {
            get => this.panel.InteractableOnShow;
            set => this.panel.InteractableOnShow = value;
        }

        public bool BlocksRaycastsOnShow
        {
            get => this.panel.BlocksRaycastsOnShow;
            set => this.panel.BlocksRaycastsOnShow = value;
        }

        public float ShowDuration
        {
            get => this.panel.ShowDuration;
            set => this.panel.ShowDuration = value;
        }

        public float HideDuration
        {
            get => this.panel.HideDuration;
            set => this.panel.HideDuration = value;
        }

        public bool CustomContainer
        {
            get => this.panel.CustomContainer;
            set => this.panel.CustomContainer = value;
        }

        public UnityEvent OnShowComplete
            => this.panel.OnShowComplete;

        public UnityEvent OnShow
            => this.panel.OnShow;

        public UnityEvent OnHideComplete
            => this.panel.OnHideComplete;

        public UnityEvent OnHide
            => this.panel.OnHide;

        public bool IsHidden
            => this.panel.IsHidden;

        public void SetPosition(Vector3 position)
            => this.panel.SetPosition(position);

        public void SetLocalPosition(Vector3 localPosition)
            => this.panel.SetLocalPosition(localPosition);

        public void Toggle(bool value)
            => this.panel.Toggle(value);

        public void Toggle(bool value, bool instant)
            => this.panel.Toggle(value, instant);

        public void Show(bool instant = false)
            => this.panel.Show(instant);

        public void Show(float duration)
            => this.panel.Show(duration);

        public void Hide(bool instant = false)
            => this.panel.Hide(instant);

        public void Hide(float duration)
            => this.panel.Hide(duration);

        public void ToggleInteractable(bool value)
            => this.panel.ToggleInteractable(value);

        protected virtual void OnAwake() { }

        protected virtual void OnShowMe() { }

        protected virtual void OnShowMeCompleted() { }

        protected virtual void OnHideMe() { }

        protected virtual void OnHideMeCompleted() { }
    }
}