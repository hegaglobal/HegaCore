#if UNITY_EDITOR

using UnityEngine;
using Sirenix.OdinInspector;

namespace HegaCore.Editor
{
    public abstract class PlayerDataEditor<T> : MonoBehaviour where T : PlayerData<T>, new()
    {
        [SerializeField, InlineProperty, HideLabel]
        private T Player = new T();

        public virtual void Set(T data)
        {
            this.Player.CopyFrom(data);
        }

        public virtual void CopyTo(T data)
        {
            data.CopyFrom(this.Player);
        }
    }
}

#endif