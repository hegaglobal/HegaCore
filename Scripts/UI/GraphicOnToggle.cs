using UnityEngine;
using UnityEngine.UI;

namespace HegaCore.UI
{
    [RequireComponent(typeof(Toggle))]
    public class GraphicOnToggle : MonoBehaviour
    {
        [SerializeField]
        private bool onUpdate = false;

        [SerializeField]
        private Graphic graphic = null;

        [SerializeField]
        private Color offColor = Color.white;

        [SerializeField]
        private Color onColor = Color.white;

        private Toggle toggle;
        private bool currentIsOn;

        private void Awake()
        {
            this.toggle = GetComponent<Toggle>();
            OnToggleChanged(this.toggle.isOn);

            if (!this.onUpdate)
                this.toggle.onValueChanged.AddListener(OnToggleChanged);
        }

        private void Update()
        {
            if (!this.onUpdate || this.currentIsOn == this.toggle.isOn)
                return;

            this.currentIsOn = this.toggle.isOn;
            OnToggleChanged(this.currentIsOn);
        }

        private void OnToggleChanged(bool value)
        {
            if (this.graphic)
                this.graphic.color = value ? this.onColor : this.offColor;
        }
    }
}