using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace HegaCore
{
    public sealed class IdGidComponent : MonoBehaviour
    {
        [SerializeField, ReadOnly]
        private IdGidVector value = IdGid.None;

        private IdGid? m_value = null;

        public IdGid Value
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
        private struct IdGidVector
        {
            [HorizontalGroup, LabelText("Id"), LabelWidth(20)]
            public int Id;

            [HorizontalGroup, LabelText("Gid"), LabelWidth(28)]
            public uint Gid;

            public static implicit operator IdGidVector(in IdGid value)
                => new IdGidVector { Id = value.Id, Gid = value.Gid };

            public static implicit operator IdGid(in IdGidVector value)
                => new IdGid(value.Id, (Gid)value.Gid);
        }
    }
}