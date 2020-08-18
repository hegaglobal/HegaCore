using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

namespace HegaCore.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CanvasGroup))]
    public class Panel : MonoBehaviour
    {
        public enum BehaviorAtAwake { Show, Hide }

        [FoldoutGroup("Panel"), VerticalGroup("Panel/At Awake", PaddingTop = 10)]
        [BoxGroup("Panel/At Awake/At Awake")]
        [LabelText("Behaviour")]
        [UnityEngine.Serialization.FormerlySerializedAs("behaviourAtAwake")]
        public BehaviorAtAwake BehaviourAtAwake = BehaviorAtAwake.Show;

        [FoldoutGroup("Panel"), VerticalGroup("Panel/At Awake")]
        [BoxGroup("Panel/At Awake/At Awake")]
        [LabelText("Is Instant")]
        public bool InstantBehaviour = false;

        [FoldoutGroup("Panel"), VerticalGroup("Panel/On Show", PaddingTop = 10)]
        [BoxGroup("Panel/On Show/On Show")]
        [LabelText("Interactable")]
        [UnityEngine.Serialization.FormerlySerializedAs("interactableOnShow")]
        public bool InteractableOnShow = true;

        [FoldoutGroup("Panel"), VerticalGroup("Panel/On Show")]
        [BoxGroup("Panel/On Show/On Show")]
        [LabelText("Block Raycasts")]
        [UnityEngine.Serialization.FormerlySerializedAs("blocksRaycastsOnShow")]
        public bool BlocksRaycastsOnShow = true;

        [FoldoutGroup("Panel"), VerticalGroup("Panel/Duration", PaddingTop = 10)]
        [BoxGroup("Panel/Duration/Duration")]
        [LabelText("Show")]
        public float ShowDuration = 0f;

        [FoldoutGroup("Panel"), VerticalGroup("Panel/Duration")]
        [BoxGroup("Panel/Duration/Duration")]
        [LabelText("Hide")]
        public float HideDuration = 0f;

        #region Custom Container

        [FoldoutGroup("Panel"), VerticalGroup("Panel/Container", PaddingTop = 10)]
        [BoxGroup("Panel/Container/Container"), HorizontalGroup("Panel/Container/Container/Custom")]
        [LabelText("Use Custom")]
        [UnityEngine.Serialization.FormerlySerializedAs("customContainer")]
        public bool CustomContainer = false;

        [FoldoutGroup("Panel"), VerticalGroup("Panel/Container")]
        [BoxGroup("Panel/Container/Container"), HorizontalGroup("Panel/Container/Container/Custom")]
        [HideLabel]
        [ShowIf(nameof(CustomContainer), false), Required("Required a custom container")]

        #endregion Custom Container
        [SerializeField]
        private GameObject container = null;

        [FoldoutGroup("Panel"), VerticalGroup("Panel/Events", PaddingTop = 10)]
        [BoxGroup("Panel/Events/Events")]
        [SerializeField]
        private UnityEvent onShow = new UnityEvent();

        [FoldoutGroup("Panel"), VerticalGroup("Panel/Events")]
        [BoxGroup("Panel/Events/Events")]
        [SerializeField]
        private UnityEvent onShowComplete = new UnityEvent();

        [FoldoutGroup("Panel"), VerticalGroup("Panel/Events")]
        [BoxGroup("Panel/Events/Events")]
        [SerializeField]
        private UnityEvent onHide = new UnityEvent();

        [FoldoutGroup("Panel"), VerticalGroup("Panel/Events")]
        [BoxGroup("Panel/Events/Events")]
        [SerializeField]
        private UnityEvent onHideComplete = new UnityEvent();

        public UnityEvent OnShow
            => this.onShow;

        public UnityEvent OnShowComplete
            => this.onShowComplete;

        public UnityEvent OnHide
            => this.onHide;

        public UnityEvent OnHideComplete
            => this.onHideComplete;

        public bool IsHidden { get; private set; }

        private CanvasGroup canvasGroup;
        private Canvas canvas;
        private GraphicRaycaster graphicRaycaster;
        private Tweener showTween;
        private Tweener hideTween;

        private void Awake()
        {
            #region Custom Container Validation

            if (this.CustomContainer && !this.container)
            {
                Debug.LogException(new System.InvalidOperationException("Container must not be null."), this.container);
            }

            #endregion Custom Container Validation

            if (!this.container)
            {
                this.container = this.gameObject;
            }

            this.canvasGroup = GetComponent<CanvasGroup>();
            this.canvas = GetComponent<Canvas>();
            this.graphicRaycaster = GetComponent<GraphicRaycaster>();

            OnAwakeBehavior();
            OnAwake();
        }

        private void OnAwakeBehavior()
        {
            switch (this.BehaviourAtAwake)
            {
                case BehaviorAtAwake.Show:
                {
                    this.IsHidden = true;
                    Show(this.InstantBehaviour);
                    break;
                }

                case BehaviorAtAwake.Hide:
                {
                    this.IsHidden = false;
                    Hide(this.InstantBehaviour);
                    break;
                }
            }
        }

        public void SetPosition(Vector3 position)
        {
            this.container.transform.position = position;
        }

        public void SetLocalPosition(Vector3 localPosition)
        {
            this.container.transform.localPosition = localPosition;
        }

        public void Toggle(bool value)
        {
            if (value)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        public void Toggle(bool value, bool instant)
        {
            if (value)
            {
                Show(instant);
            }
            else
            {
                Hide(instant);
            }
        }

        public void Show(bool instant = false)
        {
            this.hideTween?.Kill();
            this.showTween?.Kill();

            if (this.canvas)
                this.canvas.enabled = true;

            if (this.graphicRaycaster)
                this.graphicRaycaster.enabled = true;

            if (instant)
            {
                OnShowMe();
                this.onShow.Invoke();

                this.canvasGroup.alpha = 1f;

                ShowCompleted();
                return;
            }

            this.showTween = this.canvasGroup.DOFade(1f, this.ShowDuration);
            this.showTween.SetEase(Ease.Linear)
                          .OnComplete(ShowCompleted);

            OnShowMe();
            this.onShow.Invoke();
        }

        public void Show(float duration)
        {
            this.hideTween?.Kill();
            this.showTween?.Kill();

            if (this.canvas)
                this.canvas.enabled = true;

            if (this.graphicRaycaster)
                this.graphicRaycaster.enabled = true;

            this.showTween = this.canvasGroup.DOFade(1f, duration);
            this.showTween.SetEase(Ease.Linear)
                          .OnComplete(ShowCompleted);

            OnShowMe();
            this.onShow.Invoke();
        }

        private void ShowCompleted()
        {
            this.canvasGroup.interactable = true;
            this.canvasGroup.blocksRaycasts = true;

            this.IsHidden = false;

            OnShowMeCompleted();
            this.onShowComplete.Invoke();
        }

        public void Hide(bool instant = false)
        {
            this.showTween?.Kill();
            this.hideTween?.Kill();

            if (instant)
            {
                OnHideMe();
                this.onHide.Invoke();

                this.canvasGroup.alpha = 0f;

                HideCompleted();
                return;
            }

            this.hideTween = this.canvasGroup.DOFade(0f, this.HideDuration);
            this.hideTween.SetEase(Ease.Linear)
                          .OnComplete(HideCompleted);

            OnHideMe();
            this.onHide.Invoke();
        }

        public void Hide(float duration)
        {
            this.showTween?.Kill();
            this.hideTween?.Kill();

            this.hideTween = this.canvasGroup.DOFade(0f, duration);
            this.hideTween.SetEase(Ease.Linear)
                          .OnComplete(HideCompleted);

            OnHideMe();
            this.onHide.Invoke();
        }

        private void HideCompleted()
        {
            this.canvasGroup.interactable = false;
            this.canvasGroup.blocksRaycasts = false;

            if (this.canvas)
                this.canvas.enabled = false;

            if (this.graphicRaycaster)
                this.graphicRaycaster.enabled = false;

            this.IsHidden = true;

            OnHideMeCompleted();
            this.onHideComplete.Invoke();
        }

        public void ToggleInteractable(bool value)
        {
            if (!this.canvasGroup)
                return;

            this.canvasGroup.interactable = value;
            this.canvasGroup.blocksRaycasts = value;
        }

        protected virtual void OnAwake() { }

        protected virtual void OnShowMe() { }

        protected virtual void OnShowMeCompleted() { }

        protected virtual void OnHideMe() { }

        protected virtual void OnHideMeCompleted() { }
    }
}