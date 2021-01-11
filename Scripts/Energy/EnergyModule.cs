using System;
using UnityEngine;
using UnityEngine.Rendering;
using Sirenix.OdinInspector;

namespace HegaCore
{
    public sealed class EnergyModule : EnergyModuleBase
    {
        [SerializeField]
        private EnergyGraphics graphics = new EnergyGraphics();

        [SerializeField]
        private SpriteRenderer foreground = null;

        [SerializeField]
        private SortingGroup sortingGroup = null;

        public void SetSortingOrder(int value)
            => this.sortingGroup.sortingOrder = value;

        protected override void OnSetProgress(float value)
            => this.graphics.TryShow(value >= 1f);

        protected override void SetForegroundColor(in Color color)
            => this.foreground.color = color;

        [Serializable, InlineProperty]
        private sealed class EnergyGraphics
        {
            [HorizontalGroup(PaddingLeft = 6), LabelWidth(95)]
            public bool HideWhenMax = false;

            [HorizontalGroup, HideLabel, ShowIf(nameof(HideWhenMax))]
            public GameObject Graphics = null;

            public void TryShow(bool isMax)
            {
                if (!this.Graphics)
                    return;

                var active = !(this.HideWhenMax && isMax);

                if ((active && !this.Graphics.activeSelf) ||
                    (!active && this.Graphics.activeSelf))
                    this.Graphics.SetActive(active);
            }
        }
    }
}