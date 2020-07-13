using UnityEngine;
using UnuGames;

namespace HegaCore
{
    using Model;

    [RequireComponent(typeof(CanvasGroup))]
    public abstract class TooltipModule : UIManModule<TooltipModel>
    {
        [SerializeField]
        private TMP_Resizer resizer = null;

        public RectTransform rectTransform { get; private set; }

        private CanvasGroup canvasGroup;

        public TooltipModule()
        {
            SubscribeAction(nameof(this.DataInstance.Content), OnContentChanged);
        }

        private void Awake()
        {
            this.rectTransform = GetComponent<RectTransform>();
            this.canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnContentChanged(object value)
            => this.resizer.Resize();

        public void Hide()
            => this.canvasGroup.alpha = 0f;

        public void Show()
            => this.canvasGroup.alpha = 1f;

        public abstract void Set(string l10nKey, IData data = null);

        protected bool TrySetDefaultData(IData data)
        {
            if (data is TooltipData tooltipData)
            {
                SetTitle(tooltipData.Title);
                SetContent(tooltipData.Content);
                return true;
            }

            return false;
        }

        public void SetContent(string value)
            => this.DataInstance.Content = value;

        public void SetTitle(string value)
            => this.DataInstance.Title = value;
    }
}