#if UNITY_EDITOR

using UnityEngine;
using Sirenix.OdinInspector;

namespace HegaCore.Editor
{
    public abstract class GameSettingsEditor<T> : MonoBehaviour where T : GameSettings<T>, new()
    {
        [SerializeField, InlineProperty, HideLabel]
        private T Settings = new T();

        public virtual void Set(T data)
        {
            this.Settings.CopyFrom(data);
        }

        public virtual void CopyTo(T data)
        {
            data.CopyFrom(this.Settings);
        }
    }
}

#endif