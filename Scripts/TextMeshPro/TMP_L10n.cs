using UnityEngine;
using TMPro;

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

        public string Key
            => this.key;

        private void OnValidate()
        {
            this.text = GetComponent<TMP_Text>();
        }

        private void Start()
        {
            L10n.Register(this);
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

        public void SetKey(string value)
        {
            if (string.Equals(this.key, value))
                return;

            this.key = value ?? string.Empty;
            Localize();
        }
    }
}