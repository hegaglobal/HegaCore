using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace HegaCore
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Graphic))]
    public sealed class UIGrayscaleEffect : MonoBehaviour
    {
        private const string _key = "_EffectAmount";
        private const string _shaderName = "UI/Grayscale";

        [HideInInspector]
        [SerializeField]
        private Graphic targetGraphic = null;

        [OnValueChanged(nameof(UpdateGrayscaleAmount))]
        [SerializeField, Range(0f, 1f)]
        private float grayScaleAmount = 1f;

        /// <summary>
        /// Grayscale amount from 0 to 1f
        /// </summary>
        public float GrayScaleAmount
        {
            get => this.hasMaterial ? this.grayScaleAmount : 1f;

            set
            {
                if (!this.hasMaterial)
                    return;

                this.grayScaleAmount = Mathf.Clamp(value, 0f, 1f);
                this.targetGraphic.material.SetFloat(_key, this.grayScaleAmount);

                ApplyChanged();
            }
        }

        private bool hasMaterial;

        private void OnValidate()
        {
            this.targetGraphic = GetComponent<Graphic>();
        }

        private void Awake()
        {
            this.hasMaterial = false;

            if (!this.targetGraphic)
                this.targetGraphic = GetComponent<Graphic>();

            if (!this.targetGraphic)
            {
                Debug.LogError("Cannot find any Graphic component.", this.gameObject);
                return;
            }

            var material = this.targetGraphic.material;

            if (!material || !string.Equals(material.shader.name, _shaderName))
            {
                Debug.LogError("Graphic must contain a UI/Grayscale material.", this.targetGraphic);
                return;
            }

            this.hasMaterial = true;
            var clonedMaterial = new Material(material.shader);
            this.targetGraphic.material = clonedMaterial;

            UpdateGrayscaleAmount();
        }

        private void UpdateGrayscaleAmount()
        {
            if (!this.targetGraphic)
                return;

            var material = this.targetGraphic.material;

            if (!material || !string.Equals(material.shader.name, _shaderName))
            {
                Debug.LogError("Graphic must contain a UI/Grayscale material.", this.targetGraphic);
                return;
            }

            this.grayScaleAmount = Mathf.Clamp(this.grayScaleAmount, 0f, 1f);
            material.SetFloat(_key, this.grayScaleAmount);
        }

        private void ApplyChanged()
        {
            this.targetGraphic.enabled = false;
            this.targetGraphic.enabled = true;
        }
    }
}