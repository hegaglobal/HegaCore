using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace HegaCore
{
    public sealed class LuidComponent : MonoBehaviour
    {
        [SerializeField, ReadOnly]
        private SerializableLuid value = Luid.None;

        private Luid? m_value = null;

        public Luid Value
        {
            get
            {
                if (!this.m_value.HasValue)
                    this.m_value = this.value;

                return this.m_value.Value;
            }

            set
            {
                this.value = value;
                this.m_value = value;
            }
        }

        [InlineProperty]
        [Serializable]
        private struct SerializableLuid
        {
            [HorizontalGroup, LabelText("Id"), LabelWidth(20)]
            public int Id;

            [HorizontalGroup, LabelText("Gid"), LabelWidth(28)]
            public uint Gid;

            public static implicit operator SerializableLuid(in Luid value)
                => new SerializableLuid { Id = value.Id, Gid = value.Uid };

            public static implicit operator Luid(in SerializableLuid value)
                => new Luid(value.Id, (Uid)value.Gid);
        }
    }
}