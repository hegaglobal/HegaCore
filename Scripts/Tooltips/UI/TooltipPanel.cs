using System;
using UnityEngine;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;

namespace HegaCore.UI
{
    [RequireComponent(typeof(Panel))]
    public sealed class TooltipPanel : SingletonBehaviour<TooltipPanel>
    {
        [HideInInspector]
        [SerializeField]
        private Panel panel = null;

        [SerializeField]
        private Canvas rootCanvas = null;

        [SerializeField]
        private RectTransform parent = null;

        [DictionaryDrawerSettings]
        [SerializeField]
        private TooltipDictionary tooltips = new TooltipDictionary();

        private TooltipModule tooltip;
        private RectTransform target;

        private void OnValidate()
        {
            this.panel = GetComponent<Panel>();
        }

        private void Awake()
        {
            if (!this.panel)
                this.panel = GetComponent<Panel>();
        }

        private void Start()
        {
            HideAllTooltips();
        }

        public bool IsShowing(RectTransform target)
        {
            if (this.target != target)
                return false;

            return !this.panel.IsHidden;
        }

        public T GetModule<T>(TooltipType type) where T : TooltipModule
        {
            if (!this.tooltips.ContainsKey(type))
            {
                UnuLogger.LogError($"Cannot find any tooltip module of type={type}");
                return null;
            }

            if (this.tooltips[type] is T module)
                return module;

            UnuLogger.LogError($"Tooltip module of type={type} is not an instance of {typeof(T)}");
            return null;
        }

        public TooltipPanel Set(TooltipType type, string l10nKey, IData data = null)
        {
            this.tooltips[type].Set(l10nKey, data);
            return this;
        }

        public void Show(TooltipType type, RectTransform target, Direction direction, Vector2 offset = default)
        {
            HideAllTooltips();

            if (!target)
                return;

            this.target = target;
            this.tooltip = this.tooltips[type];
            this.tooltip.Show();

            Show(direction, offset).Forget();
        }

        private async UniTaskVoid Show(Direction direction, Vector2 offset)
        {
            await UniTask.DelayFrame(1);

            if (!this.tooltip || !this.target)
                return;

            SetPosition(direction, offset, true);

            await UniTask.DelayFrame(1);

            AdjustPosition();
            this.panel.Show();
        }

        public void SetPosition(Direction direction, Vector2 offset, bool hidden = false)
        {
            if (!this.tooltip || !this.target)
                return;

            if (!hidden && this.panel.IsHidden)
                return;

            var targetRect = this.target.rect;
            var tooltipRect = this.tooltip.RectTransform.rect;

            var position = this.parent.InverseTransformPoint(this.target.position);
            var offsetX = (targetRect.width / 2f) + (tooltipRect.width / 2f) + offset.x;
            var offsetY = (targetRect.height / 2f) + (tooltipRect.height / 2f) + offset.y;
            var center = targetRect.center;

            switch (direction.Horizontal)
            {
                case Horizontal.Left:
                    position.x -= offsetX;
                    break;

                case Horizontal.Right:
                    position.x += offsetX;
                    break;
            }

            switch (direction.Vertical)
            {
                case Vertical.Top:
                    position.y += offsetY;
                    break;

                case Vertical.Bottom:
                    position.y -= offsetY;
                    break;
            }

            var newPosition = new Vector3(position.x + center.x, position.y + center.y, 0);
            this.tooltip.transform.localPosition = newPosition;
        }

        private void AdjustPosition()
        {
            var target = this.tooltip.rectTransform;
            var offset = target.CalcOutsideOffset(this.rootCanvas.worldCamera);

            var position = target.localPosition;
            position.x -= offset.x;
            position.y -= offset.y;

            target.localPosition = position;
        }

        public void Hide(RectTransform target)
        {
            if (this.target != target)
                return;

            this.target = null;
            this.panel.Hide();
            HideAllTooltips();
        }

        public void HideAllTooltips()
        {
            this.tooltip = null;

            foreach (var item in this.tooltips.Values)
            {
                item.Hide();
            }
        }

        [Serializable]
        private sealed class TooltipDictionary : SerializableDictionary<TooltipType, TooltipModule> { }
    }
}
