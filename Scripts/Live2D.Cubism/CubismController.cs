using System.Collections;
using UnityEngine;
using Live2D.Cubism.Rendering;

namespace HegaCore
{
    public sealed class CubismController : MonoBehaviour
    {
        [SerializeField]
        private Animator animator = null;

        [SerializeField]
        private CubismRenderController cubismRenderer = null;

        public float TempAlpha { get; set; }

        private Color color;

        [ContextMenu("Get Components")]
        private void GetComponents()
        {
            this.animator = this.gameObject.GetComponentInChildren<Animator>();
            this.cubismRenderer = this.gameObject.GetComponentInChildren<CubismRenderController>();
        }

        private void Awake()
        {
            GetComponents();
        }

        public void Hide()
        {
            SetLayer(0);

            this.color = Color.white;
            this.color.a = 0f;

            SetColor(in this.color);

            if (this.gameObject.activeSelf)
                StartCoroutine(HideInternal());
        }

        private IEnumerator HideInternal()
        {
            yield return new WaitForSeconds(0.02f);

            if (this.gameObject.activeSelf)
                this.gameObject.SetActive(false);
        }

        public void Hide(in Vector3 position)
        {
            this.transform.position = position;
            Hide();
        }

        public void Show(float alpha = 1f)
        {
            if (!this.gameObject.activeSelf)
                this.gameObject.SetActive(true);

            SetAlpha(alpha);
        }

        public void Show(in Vector3 position, float alpha = 1f)
        {
            this.transform.position = position;
            Show(alpha);
        }

        public void SetLayer(int layer, int sortingOrder = 0)
        {
            foreach (var renderer in this.cubismRenderer.Renderers)
            {
                renderer.gameObject.layer = layer;
            }

            this.cubismRenderer.SortingOrder = sortingOrder;
        }

        public void SetLayer(in SingleOrderLayer orderLayer)
        {
            foreach (var renderer in this.cubismRenderer.Renderers)
            {
                renderer.gameObject.layer = orderLayer.Layer.value;
            }

            this.cubismRenderer.SortingOrder = orderLayer.Order;
        }

        public void PlayAnimation(int id)
        {
            this.animator.SetInteger(Params.ID, id);
        }

        public Color GetColor()
            => this.color;

        public void SetColor(in Color value)
        {
            this.color = value;

            foreach (var renderer in this.cubismRenderer.Renderers)
            {
                renderer.Color = value;
            }
        }

        public void SetColor(Color value)
            => SetColor(in value);

        public float GetAlpha()
            => this.TempAlpha;

        public void SetAlpha(float value)
        {
            this.TempAlpha = value;

            foreach (var renderer in this.cubismRenderer.Renderers)
            {
                var color = renderer.Color;
                color.a = Mathf.Clamp(value, 0f, 1f);
                renderer.Color = color;
            }
        }

        private static class Params
        {
            public const string ID = nameof(ID);
        }
    }
}