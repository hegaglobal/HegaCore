using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace HegaCore.UI
{
    [RequireComponent(typeof(Graphic))]
    public class InteractableGraphic : MonoBehaviour,
        IPointerEnterHandler,  IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField, HideInInspector]
        private Graphic graphic = null;

        [SerializeField]
        private bool interactable = true;

        [SerializeField]
        private Event onHover = new Event();

        [SerializeField]
        private Event onLeave = new Event();

        [SerializeField]
        private ClickEvent onClick = new ClickEvent();

        public Graphic Graphic => this.graphic;

        public bool Interactable
        {
            get => this.interactable;
            set => this.interactable = value;
        }

        public Event OnHover => this.onHover;

        public Event OnLeave => this.onLeave;

        public ClickEvent OnClick => this.onClick;

        private void OnEnable()
        {
            this.graphic = GetComponent<Graphic>();
        }

        protected virtual void Awake()
        {
            this.graphic = GetComponent<Graphic>();
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (this.interactable)
                this.onClick.Invoke(this, eventData.button);
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (this.interactable)
                this.onHover.Invoke(this);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (this.interactable)
                this.onLeave.Invoke(this);
        }

        [Serializable]
        public sealed class Event : UnityEvent<InteractableGraphic> { }

        [Serializable]
        public sealed class ClickEvent : UnityEvent<InteractableGraphic, PointerEventData.InputButton> { }
    }
}