using UnityEngine;
using Live2D.Cubism.Rendering;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HegaCore
{
    public sealed class CubismRendererController : RendererController
    {
        [Header("Cubism")]
        [SerializeField, InlineButton(nameof(FindCubismRenderController), "Find")]
        private CubismRenderController cubismRenderController = null;

        protected override void Awake()
        {
            base.Awake();

            FindCubismRenderController();
        }

        private void FindCubismRenderController()
            => this.cubismRenderController = GetComponentInChildren<CubismRenderController>();

        protected override void OnSetLayer(in SortingLayerId layerId)
        {
            if (this.cubismRenderController)
                this.cubismRenderController.SortingLayerId = layerId.id;
        }

        protected override void OnSetAlpha(float value)
        {
            if (!this.cubismRenderController)
                return;

            value = Mathf.Clamp(value, 0f, 1f);

            foreach (var renderer in this.cubismRenderController.Renderers)
            {
                if (!renderer)
                    continue;

                var color = renderer.Color.With(a: value);
                renderer.Color = color;
            }
        }

#if UNITY_EDITOR
        [MenuItem("Tools/Renderer Controller/Replace With Cubism")]
        public static void Replace()
        {
            foreach (var obj in Selection.gameObjects)
            {
                var renderer = obj.GetComponent<RendererController>();

                if (!renderer || renderer is CubismRendererController)
                    continue;

                var sortingLayer = renderer.SortingLayer;
                var sortingOrder = renderer.SortingOrder;

                DestroyImmediate(renderer, true);

                var cubism = obj.AddComponent<CubismRendererController>();
                cubism.Init(sortingLayer, sortingOrder);
                cubism.FindCubismRenderController();
            }
        }
#endif
    }
}