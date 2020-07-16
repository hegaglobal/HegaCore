using System.Collections;
using UnityEngine;
using Live2D.Cubism.Rendering;

namespace HegaCore
{
    public sealed class CubismController : MonoBehaviour
    {
        [SerializeField]
        private GameObject model = null;

        [SerializeField]
        private Animator animator = null;

        [SerializeField]
        private CubismRenderController cubismRenderer = null;

        [SerializeField]
        private CubismRenderer[] renderers = null;

        public float TempAlpha { get; set; }

        [ContextMenu("Refresh")]
        private void OnValidate()
        {
            if (!this.model)
                return;

            this.animator = this.model.GetComponent<Animator>();
            this.cubismRenderer = this.model.GetComponent<CubismRenderController>();
            this.renderers = this.model.GetComponentsInChildren<CubismRenderer>();
        }

        public void Hide()
        {
            SetLayer(0);

            var color = Color.white;
            color.a = 0f;
            SetColor(color);

            if (this.gameObject.activeSelf)
                StartCoroutine(HideInternal());
        }

        private IEnumerator HideInternal()
        {
            yield return new WaitForSeconds(0.02f);

            if (this.gameObject.activeSelf)
                this.gameObject.SetActive(false);
        }

        public void Hide(Vector3 position)
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

        public void Show(Vector3 position, float alpha = 1f)
        {
            this.transform.position = position;
            Show(alpha);
        }

        public void SetLayer(int layer, int sortingOrder = 0)
        {
            foreach (var renderer in this.renderers)
            {
                renderer.gameObject.layer = layer;
            }

            this.cubismRenderer.SortingOrder = sortingOrder;
        }

        public void PlayAnimation(int id)
        {
            this.animator.SetInteger(Params.ID, id);
        }

        public void SetColor(Color value)
        {
            foreach (var renderer in this.renderers)
            {
                renderer.Color = value;
            }
        }

        public float GetAlpha()
            => this.TempAlpha;

        public void SetAlpha(float value)
        {
            this.TempAlpha = value;

            foreach (var renderer in this.renderers)
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