using UnityEngine;

namespace HegaCore
{
    public abstract class OnDestroyRemover : MonoBehaviour
    {
        protected void OnDestroy()
        {
            Remove();
        }

        protected abstract void Remove();
    }
}