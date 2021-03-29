using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace HegaCore.UI
{
    [RequireComponent(typeof(Toggle))]
    public class CanvasGroupOnToggle : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup[] canvasGroups = new CanvasGroup[0];

        [SerializeField, BoxGroup("Alpha"), LabelText("Off")]
        private float offAlpha = 1f;

        [SerializeField, BoxGroup("Alpha"), LabelText("On")]
        private float onAlpha = 1f;

        private Toggle toggle;

        private void Awake()
        {
            this.toggle = GetComponent<Toggle>();
            OnToggleChanged(this.toggle.isOn);

            this.toggle.onValueChanged.AddListener(OnToggleChanged);
        }

        private void OnToggleChanged(bool value)
            => SetAlpha(value ? this.onAlpha : this.offAlpha);

        private void SetAlpha(float alpha)
        {
            foreach (var canvasGroup in this.canvasGroups)
            {
                if (canvasGroup)
                    canvasGroup.alpha = alpha;
            }
        }
    }
}