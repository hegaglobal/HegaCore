using UnityEngine;

namespace HegaCore
{
    public abstract class RemoveOnDestroy : MonoBehaviour
    {
        private void OnDestroy()
        {
            OnDestroyRemove();
        }

        protected abstract void OnDestroyRemove();
    }
}