using System;

namespace HegaCore
{
    [Serializable]
    public abstract class PlayerData<T> : PlayerData where T : PlayerData<T>
    {
        public virtual void CopyFrom(T data)
        {
            base.CopyFrom(data);
        }
    }
}