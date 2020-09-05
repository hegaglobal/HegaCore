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
    }
}
