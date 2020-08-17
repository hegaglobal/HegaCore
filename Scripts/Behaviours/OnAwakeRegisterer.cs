using UnityEngine;

namespace HegaCore
{
    public abstract class OnAwakeRegisterer : MonoBehaviour
    {
        [SerializeField]
        private bool overrideExisting = false;

        protected void Awake()
        {
            Register(this.overrideExisting);
        }

        protected abstract void Register(bool @override);
    }
}