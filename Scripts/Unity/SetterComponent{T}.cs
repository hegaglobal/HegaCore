using UnityEngine;

namespace HegaCore
{
    public abstract class SetterComponent<T> : MonoBehaviour
    {
        public abstract void Set(T value);
    }
}