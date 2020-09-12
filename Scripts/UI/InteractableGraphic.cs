using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace HegaCore.UI
{
    public delegate void InteractableGraphicAction(InteractableGraphic sender);

    [RequireComponent(typeof(Graphic))]
    public class InteractableGraphic : MonoBehaviour,
        IPointerEnterHandler,  IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField, HideInInspector]
        private Graphic graphic = null;

        [SerializeField]
        private bool interactable = true;

        public Graphic Graphic => this.graphic;

        public bool Interactable
        {
            get => this.interactable;
            set => this.interactable = value;
        }

        public event InteractableGraphicAction OnHover;

        public event InteractableGraphicAction OnLeave;

        public event InteractableGraphicAction OnClick;

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
                this.OnClick?.Invoke(this);
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (this.interactable)
                this.OnHover?.Invoke(this);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (this.interactable)
                this.OnLeave?.Invoke(this);
        }
    }
}