using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

namespace HegaCore
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(TMP_Text))]
    public class TMP_Resizer : UIBehaviour
    {
        [HideInInspector]
        [SerializeField]
        private TMP_Text text = null;

        [SerializeField]
        private bool autoResize = false;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            this.text = GetComponent<TMP_Text>();
        }
#endif // UNITY_EDITOR

        protected override void Awake()
        {
            base.Awake();

            if (!this.text)
                this.text = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            if (this.autoResize)
                Resize();
        }

        public void Resize()
        {
            Resize(this.text.text);
        }

        public void Resize(string value)
        {
            var size = this.text.GetPreferredValues(value);
            this.text.rectTransform.sizeDelta = size;
        }
    }
}