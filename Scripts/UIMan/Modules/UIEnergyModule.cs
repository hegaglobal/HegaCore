using UnityEngine;
using UnityEngine.UI;

namespace HegaCore.UI
{
    public sealed class UIEnergyModule : BaseEnergyModule
    {
        [SerializeField]
        private Image foreground = null;

        protected override void SetForegroundColor(in Color color)
            => this.foreground.color = color;
    }
}