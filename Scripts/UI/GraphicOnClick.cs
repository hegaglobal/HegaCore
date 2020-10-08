﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace HegaCore.UI
{
    public class GraphicOnClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField]
        private Graphic graphic = null;

        [SerializeField, BoxGroup("Colors"), LabelText("Default")]
        private Color defaultColor = Color.white;

        [SerializeField, BoxGroup("Colors"), LabelText("Click")]
        private Color clickColor = Color.white;

        private void Awake()
        {
            SetColor(this.defaultColor);
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
            => SetColor(this.defaultColor);

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
            => SetColor(this.clickColor);

        private void SetColor(in Color color)
        {
            if (this.graphic)
                this.graphic.color = color;
        }
    }
}