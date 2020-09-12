﻿using UnityEngine;
using Live2D.Cubism.Rendering;
using Sirenix.OdinInspector;

namespace HegaCore
{
    public sealed class CubismRendererController : RendererController
    {
        [Header("Cubism")]
        [SerializeField, LabelText("Render Controller"), InlineButton(nameof(FindCubismRenderController), "Find")]
        private CubismRenderController cubismRenderController = null;

        private void Awake()
        {
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
    }
}