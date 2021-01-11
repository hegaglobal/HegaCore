using UnityEngine;
using UnityEngine.UI;

namespace HegaCore.UI
{
    public sealed class UIEnergyModule : EnergyModuleBase
    {
        [SerializeField]
        private Image foreground = null;

        protected override void SetForegroundColor(in Color color)
            => this.foreground.color = color;
    }
}