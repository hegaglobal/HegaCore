using UnityEngine;
using TMPro;

namespace HegaCore
{
    public class TextModule : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text text = null;

        public TMP_Text Text
        {
            get
            {
                if (!this.text)
                    this.text = GetComponentInChildren<TMP_Text>();

                return this.text;
            }
        }

        public string GetText()
            => this.Text.text;

        public void SetText(string value)
            => this.Text.SetText(value);
    }
}