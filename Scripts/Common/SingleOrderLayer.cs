using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace HegaCore
{
    [Serializable, InlineProperty]
    public struct SingleOrderLayer
    {
        [HorizontalGroup, HideLabel]
        public SingleLayer Layer;

        [HorizontalGroup, LabelWidth(35)]
        public int Order;

        public void Deconstruct(out SingleLayer layer, out int order)
        {
            layer = this.Layer;
            order = this.Order;
        }

        public SingleOrderLayer With(in SingleLayer? Layer = null, int? Order = null)
            => new SingleOrderLayer {
                Layer = Layer ?? this.Layer,
                Order = Order ?? this.Order
            };
    }
}
