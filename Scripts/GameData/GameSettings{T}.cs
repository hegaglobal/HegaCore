using System;

namespace HegaCore
{
    [Serializable]
    public abstract class GameSettings<T> : GameSettings where T : GameSettings<T>
    {
        public virtual void CopyFrom(T data)
        {
            base.CopyFrom(data);
        }
    }
}