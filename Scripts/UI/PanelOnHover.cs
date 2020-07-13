﻿using UnityEngine;
using UnityEngine.EventSystems;

namespace HegaCore
{
    public sealed class PanelOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private bool show = true;

        [SerializeField]
        private Panel[] panels = null;

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            foreach (var panel in this.panels)
            {
                if (panel.gameObject.activeSelf)
                {
                    if (this.show)
                        panel.Show();
                    else
                        panel.Hide();
                }
            }
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            foreach (var panel in this.panels)
            {
                if (panel.gameObject.activeSelf)
                {
                    if (this.show)
                        panel.Hide();
                    else
                        panel.Show();
                }
            }
        }
    }
}