using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;

namespace HegaCore
{
    [RequireComponent(typeof(TMP_Text))]
    public class TMP_L10n : MonoBehaviour, IL10n
    {
        [SerializeField]
        private string key = string.Empty;

        [SerializeField]
        private bool silent = false;

        [HideInInspector]
        [SerializeField]
        private TMP_Text text = null;

        private void OnValidate()
        {
            this.text = GetComponent<TMP_Text>();
        }

        private void Start()
        {
            L10n.Register(this);

            LazyLocalize().Forget();
        }

        private async UniTaskVoid LazyLocalize()
        {
            await UniTask.WaitUntil(() => L10n.IsInitialized);

            Localize();
        }

        private void OnDestroy()
        {
            if (SingletonBehaviour.Quitting)
                return;

            L10n.Deregister(this);
        }

        public void Localize()
            => this.text.SetText(L10n.Localize(this.key, this.silent));
    }
}