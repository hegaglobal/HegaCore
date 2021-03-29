using UnityEngine;
using TMPro;

namespace HegaCore
{
    public class TextModule : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text text = null;

        private void Awake()
        {
            this.text = GetComponentInChildren<TMP_Text>();
        }

        public string GetText()
            => this.text ? this.text.text : string.Empty;

        public void SetText(string value)
        {
            if (this.text)
                this.text.SetText(value);
        }

        public void SetColor(in Color color)
        {
            if (!this.text)
                return;

            this.text.color = color;
        }

        public void SetFontSize(float fontSize)
        {
            if (!this.text)
                return;

            this.text.fontSize = fontSize;
        }
    }
}