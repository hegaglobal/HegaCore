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

        public bool IsShowing { get; private set; }

        private IData data = Data.None;
        private bool locked = false;

        protected override void Awake()
        {
            if (!this.rectTransform)
                this.rectTransform = GetComponent<RectTransform>();
        }

        public void SetData(IData data)
        {
            this.l10nKey = string.Empty;
            this.data = data ?? Data.None;
        }

        public void SetData(string l10nKey, IData data = null)
        {
            this.l10nKey = l10nKey ?? string.Empty;
            this.data = data ?? Data.None;
        }

        public void ResetData()
            => TooltipPanel.Instance.Set(this.type, this.l10nKey, this.data);

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (string.IsNullOrEmpty(this.l10nKey) && !this.ignoreEmptyKey)
            {
                if (!this.silent)
                    UnuLogger.LogWarning($"Key is empty");

                return;
            }

            TooltipPanel.Instance.Set(this.type, this.l10nKey, this.data)
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