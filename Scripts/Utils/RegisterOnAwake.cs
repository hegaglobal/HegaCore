using UnityEngine;

namespace HegaCore
{
    public abstract class RegisterOnAwake : MonoBehaviour
    {
        [SerializeField]
        private bool overrideExisting = false;

        private void Awake()
        {
            OnAwakeRegister(this.overrideExisting);
        }

        protected abstract void OnAwakeRegister(bool @override);
    }
}