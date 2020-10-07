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
        private Color offColor = Color.white;

        [SerializeField]
        private Color onColor = Color.white;

        private Toggle toggle;

        private void Awake()
        {
            this.toggle = GetComponent<Toggle>();
            OnToggleChanged(this.toggle.isOn);

            this.toggle.onValueChanged.AddListener(OnToggleChanged);
        }

        private void OnToggleChanged(bool value)
            => this.graphic.color = value ? this.onColor : this.offColor;
    }
}