using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace HegaCore
{
    public sealed class DigidComponent : MonoBehaviour
    {
        [SerializeField, ReadOnly]
        private UnityDigid digid = Digid.None;

        private Digid? m_digid = null;

        public Digid Digid
        {
            get
            {
                if (!this.m_digid.HasValue)
                    this.m_digid = this.digid;

                return this.m_digid.Value;
            }

            set
            {
                this.digid = value;
                this.m_digid = value;
            }
        }

        [InlineProperty]
        [Serializable]
        private struct UnityDigid
        {
            [HorizontalGroup, LabelText("Id"), LabelWidth(20)]
            public int Id;

            [HorizontalGroup, LabelText("Gid"), LabelWidth(28)]
            public uint Gid;

            public static implicit operator UnityDigid(in Digid value)
                => new UnityDigid { Id = value.Id, Gid = value.Gid };

            public static implicit operator Digid(in UnityDigid value)
                => new Digid(value.Id, (Gid)value.Gid);
        }
    }
}