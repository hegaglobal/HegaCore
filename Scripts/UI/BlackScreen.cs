using System;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

namespace HegaCore.UI
{
    [RequireComponent(typeof(Panel))]
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(GraphicRaycaster))]
    public sealed class BlackScreen : SingletonBehaviour<BlackScreen>
    {
        [HideInInspector]
        [SerializeField]
        private Panel panel = null;

        [HideInInspector]
        [SerializeField]
        private Canvas canvas = null;

        [SerializeField]
        private int sortingOrder = 0;

        [SerializeField]
        private float autoHideAfter = 1f;

        private float autoHideAfterDuration;
        private float? hideDuration;
        private Action onShowCompleted;

        private void OnValidate()
        {
            this.panel = GetComponent<Panel>();
            this.canvas = GetComponent<Canvas>();
            this.canvas.overrideSorting = true;
        }

        private void Awake()
        {
            this.panel.OnShowComplete.AddListener(OnPanelShowComplete);
            this.canvas.overrideSorting = true;
        }

        public void Show(Action onShowCompleted = null, float? autoHideDuration = null, int? sortingOrder = null, float? showDuration = null, float? hideDuration = null)
        {
            this.onShowCompleted = onShowCompleted;
            this.canvas.sortingOrder = sortingOrder ?? this.sortingOrder;
            this.hideDuration = hideDuration;
            this.autoHideAfterDuration = autoHideDuration.HasValue && autoHideDuration.Value > 0f
                                          ? autoHideDuration.Value : this.autoHideAfter;

            if (showDuration.HasValue)
                this.panel.Show(showDuration.Value);
            else
                this.panel.Show();
        }

        private void OnPanelShowComplete()
        {
            this.onShowCompleted?.Invoke();
            this.onShowCompleted = null;

            AutoHide().Forget();
        }

        private async UniTaskVoid AutoHide()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(this.autoHideAfterDuration));

            if (this.hideDuration.HasValue)
                this.panel.Hide(this.hideDuration.Value);
            else
                this.panel.Hide();
        }
    }
}