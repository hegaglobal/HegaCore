using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace HegaCore.UI
{
    [RequireComponent(typeof(Toggle))]
    public class MultipleGraphicsOnToggle : MonoBehaviour
    {
        [SerializeField]
        private Graphic[] graphics = new Graphic[0];

        [SerializeField, BoxGroup("Colors"), LabelText("Off")]
        private Color offColor = Color.white;

        [SerializeField, BoxGroup("Colors"), LabelText("On")]
        private Color onColor = Color.white;

        private Toggle toggle;

        private void Awake()
        {
            this.toggle = GetComponent<Toggle>();
            OnToggleChanged(this.toggle.isOn);

            this.toggle.onValueChanged.AddListener(OnToggleChanged);
        }

        private void OnToggleChanged(bool value)
            => SetColor(value ? this.onColor : this.offColor);

        private void SetColor(in Color color)
        {
            foreach (var graphic in this.graphics)
            {
                if (graphic)
                    graphic.color = color;
            }
        }
    }
}