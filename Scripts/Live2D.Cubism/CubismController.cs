using UnityEngine;
using Live2D.Cubism.Rendering;
using Cysharp.Threading.Tasks;

namespace HegaCore
{
    public sealed class CubismController : MonoBehaviour
    {
        [SerializeField]
        private bool useScaleOne = true;

        [SerializeField]
        private Animator animator = null;

        [SerializeField]
        private CubismRenderController cubismRenderer = null;

        [SerializeField]
        private SpriteRenderer spriteRenderer = null;

        public Animator Animator => this.animator;

        public float TempAlpha { get; set; }

        public Vector3 LocalScale { get; private set; }

        public string Id { get; private set; }

        public bool IsActive => this.gameObject && this.gameObject.activeSelf;

        private bool hasIdAnim;
        private bool hasBodyAnim;
        private bool hasEmoAnim;
        private Color color;

        [ContextMenu("Get Components")]
        private void GetComponents()
        {
            this.animator = GetComponentInChildren<Animator>();
            this.cubismRenderer = GetComponentInChildren<CubismRenderController>();
            this.spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void Awake()
        {
            GetComponents();

            this.LocalScale = this.useScaleOne ? Vector3.one : this.transform.localScale;
        }

        public void Initialize(string id, bool hasIdAnim, bool hasBodyAnim, bool hasEmoAnim)
        {
            this.Id = id;
            this.hasIdAnim = hasIdAnim;
            this.hasBodyAnim = hasBodyAnim;
            this.hasEmoAnim = hasEmoAnim;
        }

        public void Hide()
        {
            SetLayer(0);

            this.color = Color.white.With(a: 0f);
            SetColor(in this.color);

            this.transform.localScale = this.LocalScale;

            LateHide().Forget();
        }

        private async UniTaskVoid LateHide()
        {
            await UniTask.DelayFrame(2);

            if (this.gameObject.activeSelf)
                this.gameObject.SetActive(false);
        }

        public void Hide(in Vector3 position)
        {
            this.transform.position = position;
            Hide();
        }

        public void Set(in Vector3 position)
        {
            this.transform.position = position;
        }

        public void Set(in Vector3 position, in Color color)
        {
            this.transform.position = position;
            SetColor(color);
        }

        public void Show(float alpha = 1f)
        {
            if (!this.gameObject.activeSelf)
                this.gameObject.SetActive(true);

            SetAlpha(alpha);
        }

        public void Show(in Color color)
        {
            if (!this.gameObject.activeSelf)
                this.gameObject.SetActive(true);

            SetColor(in color);
        }

        public void Show(in Vector3 position, float alpha = 1f)
        {
            this.transform.position = position;
            Show(alpha);
        }

        public void Show(in Vector3 position, in Color color)
        {
            this.transform.position = position;
            Show(color);
        }

        public void SetScale(float value)
            => this.transform.localScale = this.LocalScale * value;

        public void SetLayer(int layer, int sortingOrder = 0)
        {
            if (this.cubismRenderer)
            {
                foreach (var renderer in this.cubismRenderer.Renderers)
                {
                    renderer.gameObject.layer = layer;
                }

                this.cubismRenderer.SortingOrder = sortingOrder;
            }

            if (this.spriteRenderer)
            {
                this.spriteRenderer.gameObject.layer = layer;
                this.spriteRenderer.sortingOrder = sortingOrder;
            }
        }

        public void SetLayer(in SingleOrderLayer orderLayer)
        {
            if (this.cubismRenderer)
            {
                foreach (var renderer in this.cubismRenderer.Renderers)
                {
                    renderer.gameObject.layer = orderLayer.Layer.value;
                }

                this.cubismRenderer.SortingOrder = orderLayer.Order;
            }

            if (this.spriteRenderer)
            {
                this.spriteRenderer.gameObject.layer = orderLayer.Layer.value;
                this.spriteRenderer.sortingOrder = orderLayer.Order;
            }
        }

        public void PlayAnimation(int id)
        {
            if (this.animator && this.hasIdAnim)
                this.animator.SetInteger(Params.ID, id);
        }

        public void PlayBodyAnimation(int id)
        {
            if (this.animator && this.hasBodyAnim)
                this.animator.SetInteger(Params.Body, id);
        }

        public void PlayEmoAnimation(int id)
        {
            if (this.animator && this.hasEmoAnim)
                this.animator.SetInteger(Params.Emo, id);
        }

        public Color GetColor()
            => this.color;

        public void SetColor(in Color value)
        {
            this.color = value;

            if (this.cubismRenderer)
            {
                foreach (var renderer in this.cubismRenderer.Renderers)
                {
                    renderer.Color = value;
                }
            }

            if (this.spriteRenderer)
            {
                this.spriteRenderer.color = value;
            }
        }

        public void SetColor(Color value)
            => SetColor(in value);

        public float GetAlpha()
            => this.TempAlpha;

        public void SetAlpha(float value)
        {
            this.TempAlpha = Mathf.Clamp(value, 0f, 1f);

            if (this.cubismRenderer)
            {
                foreach (var renderer in this.cubismRenderer.Renderers)
                {
                    var color = renderer.Color.With(a: this.TempAlpha);
                    renderer.Color = color;
                }
            }

            if (this.spriteRenderer)
            {
                var color = this.spriteRenderer.color.With(a: this.TempAlpha);
                this.spriteRenderer.color = color;
            }
        }

        private static class Params
        {
            public const string ID = nameof(ID);
            public const string Body = nameof(Body);
            public const string Emo = nameof(Emo);
        }
    }
}