using System.Collections.Generic;
using UnityEngine;
using UnuGames.MVVM;

namespace HegaCore.MVVM
{
    using UI;

    public class PanelsBinder : BinderBase
    {
        public List<Panel> showOnTrue = new List<Panel>();
        public List<Panel> hideOnTrue = new List<Panel>();

        [HideInInspector]
        public BindingField valueField = new BindingField("bool");

        [HideInInspector]
        public BoolConverter valueConverter = new BoolConverter("bool");

        public override void Initialize(bool forceInit)
        {
            if (!CheckInitialize(forceInit))
                return;

            SubscribeOnChangedEvent(this.valueField, OnUpdateValue);
        }

        private void OnUpdateValue(object val)
        {
            var valChange = this.valueConverter.Convert(val, this);

            if (this.showOnTrue != null && this.showOnTrue.Count > 0)
            {
                for (var i = 0; i < this.showOnTrue.Count; i++)
                {
                    if (!this.showOnTrue[i])
                        continue;

                    if (valChange)
                        this.showOnTrue[i].Show();
                    else
                        this.showOnTrue[i].Hide();
                }
            }

            if (this.hideOnTrue != null && this.hideOnTrue.Count > 0)
            {
                for (var i = 0; i < this.hideOnTrue.Count; i++)
                {
                    if (!this.hideOnTrue[i])
                        continue;

                    if (valChange)
                        this.hideOnTrue[i].Hide();
                    else
                        this.hideOnTrue[i].Show();
                }
            }
        }
    }
}