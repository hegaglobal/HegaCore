using HegaCore;
using Live2D.Cubism.Framework.Raycasting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPreviewDraggable : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private bool isInteracting = true;
    
    public float Multiplier = 1;
    public Toggle dragToggle;

    void Start()
    {
        if (dragToggle)
            dragToggle.isOn = DataManager.GameSettings.allowDragPreview;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (isInteracting)
            CubismManager.Instance.CurCharacter.UpdateInteractDrag(eventData.delta);
        else if (DataManager.GameSettings.allowDragPreview)
            CubismManager.Instance.CurCharacter.transform.position += (Vector3) eventData.delta * Multiplier;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        CubismRaycastHit[] Results = new CubismRaycastHit[5]; // init 5 slot
        int castCount = CubismManager.Instance.CurCharacter.live2DCharInteract.GetRayCastDrawableArtMesh(ref Results);
        if (castCount > 0)
        {
            isInteracting = CubismManager.Instance.CurCharacter.StartInteract(Results);
        }
        else
        {
            isInteracting = false;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        CubismManager.Instance.CurCharacter.EndInteract();
    }

    public void OnClickToggleMove()
    {
        DataManager.GameSettings.allowDragPreview = !DataManager.GameSettings.allowDragPreview;
        
        if (dragToggle)
            dragToggle.isOn = !DataManager.GameSettings.allowDragPreview;
    }
}