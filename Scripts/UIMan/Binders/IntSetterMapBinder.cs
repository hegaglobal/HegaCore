using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using HegaCore;

namespace UnuGames.MVVM
{
    public class IntSetterMapBinder : BinderBase
    {
        public List<MapItem> map = new List<MapItem>();

        [HideInInspector]
        public BindingField valueField = new BindingField("int");

        [HideInInspector]
        public IntConverter valueConverter = new IntConverter("int");

        public override void Initialize(bool forceInit)
        {
            if (!CheckInitialize(forceInit))
                return;

            SubscribeOnChangedEvent(this.valueField, OnUpdateValue);
        }

        private void OnUpdateValue(object val)
        {
            var valChange = this.valueConverter.Convert(val, this);
            var index = this.map.FindIndex(x => x.Value == valChange);

            if (index < 0)
                return;

            var setters = this.map[index].Setters;

            if (setters == null || setters.Length <= 0)
                return;

            for (var i = 0; i < setters.Length; i++)
            {
                if (setters[i])
                    setters[i].Set(valChange);
            }
        }

        [InlineProperty]
        [Serializable]
        public class MapItem
        {
            public int Value;
            public SetterComponentInt[] Setters;
        }
    }
}