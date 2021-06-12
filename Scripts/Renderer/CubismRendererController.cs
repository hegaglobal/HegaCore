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
        [SerializeField, InlineButton(nameof(FindAllCubismRenderControllers), "Find")]
        private CubismRenderController[] cubismRenderControllers = null;

        [SerializeField, Indent]
        private bool useOpacity = true;

        protected override void Awake()
        {
            base.Awake();

            FindAllCubismRenderControllers();
        }

        private void FindAllCubismRenderControllers()
            => this.cubismRenderControllers = GetComponentsInChildren<CubismRenderController>().OrEmpty();

        protected override void OnSetColor(in Color value)
        {
            foreach (var controller in this.cubismRenderControllers)
            {
                foreach (var renderer in controller.Renderers)
                {
                    if (!renderer)
                        continue;

                    renderer.Color = value;
                }
            }
        }

        protected override void OnSetLayer(in SortingLayerId layerId)
        {
            foreach (var controller in this.cubismRenderControllers)
            {
                controller.SortingLayerId = layerId.id;
            }
        }

        protected override void OnSetAlpha(float value)
        {
            if (this.cubismRenderControllers.Length <= 0)
                return;

            value = Mathf.Clamp(value, 0f, 1f);

            if (this.useOpacity)
            {
                foreach (var controller in this.cubismRenderControllers)
                {
                    controller.Opacity = value;
                }

                return;
            }

            foreach (var controller in this.cubismRenderControllers)
            {
                foreach (var renderer in controller.Renderers)
                {
                    if (!renderer)
                        continue;

                    var color = renderer.Color.With(a: value);
                    renderer.Color = color;
                }
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
                cubism.FindAllCubismRenderControllers();

                EditorUtility.SetDirty(obj);
            }
        }
#endif
    }
}