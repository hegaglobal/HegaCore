using UnityEngine;
using UnityEngine.UI;

namespace HegaCore.UI
{
    [RequireComponent(typeof(Toggle))]
    public class GraphicOnToggle : MonoBehaviour
    {
        [SerializeField]
        private Graphic graphic = null;

        [SerializeField]
        private Color color = Color.white;

        private Toggle toggle;
        private Color backupColor;

        private void Awake()
        {
            this.toggle = GetComponent<Toggle>();
            this.backupColor = this.graphic.color;

            OnToggleChanged(this.toggle.isOn);

            this.toggle.onValueChanged.AddListener(OnToggleChanged);
        }

        private void OnToggleChanged(bool value)
            => this.graphic.color = value ? this.color : this.backupColor;
    }
}