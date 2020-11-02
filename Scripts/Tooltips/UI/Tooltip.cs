using UnityEngine;
using UnityEngine.EventSystems;

namespace HegaCore.UI
{
    [DisallowMultipleComponent]
    public sealed class Tooltip : UIBehaviour, ITooltip,
        IPointerEnterHandler, IPointerExitHandler,
        IDragHandler, IEndDragHandler
    {
        [SerializeField]
        private RectTransform rectTransform = null;

        [Space]
        [SerializeField]
        private Direction direction = default;

        [SerializeField]
        private Vector2 offset = default;

        [SerializeField]
        private bool draggable = false;

        [Space]
        [SerializeField]
        private TooltipType type = TooltipType.Simple;

        [SerializeField]
        private string l10nKey = string.Empty;

        [SerializeField]
        private bool ignoreEmptyKey = false;

        [SerializeField]
        private bool silent = false;

        public string L10nKey => this.l10nKey;

        public bool IsShowing { get; private set; }

        private IToTemplatedString template = TooltipData.None;
        private bool locked = false;

        protected override void Awake()
        {
            if (!this.rectTransform)
                this.rectTransform = GetComponent<RectTransform>();
        }

        public void Set(string l10nKey)
            => this.l10nKey = l10nKey ?? string.Empty;

        public void Set(IToTemplatedString template)
            => this.template = template ?? TooltipData.None;

        public void Set(string l10nKey, IToTemplatedString template)
        {
            this.l10nKey = l10nKey ?? string.Empty;
            this.template = template ?? TooltipData.None;
        }

        public void Unset()
            => TooltipPanel.Instance.Set(this.type, this.l10nKey, this.template);

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (string.IsNullOrEmpty(this.l10nKey) && !this.ignoreEmptyKey)
            {
                if (!this.silent)
                    UnuLogger.LogWarning($"Key is empty");

                return;
            }

            TooltipPanel.Instance.Set(this.type, this.l10nKey, this.template)
                                 .Show(this.type, this.rectTransform, this.direction, this.offset);
            this.IsShowing = true;
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (this.locked)
                return;

            TooltipPanel.Instance.Hide(this.rectTransform);
            this.IsShowing = false;
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (!this.draggable)
                return;

            this.locked = true;
            TooltipPanel.Instance.SetPosition(this.direction, this.offset, false);
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            if (!this.draggable)
                return;

            this.locked = false;
            TooltipPanel.Instance.Hide(this.rectTransform);
        }
    }
}