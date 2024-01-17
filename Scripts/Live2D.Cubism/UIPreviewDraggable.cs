using System.Collections.Generic;
using HegaCore;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework.Raycasting;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPreviewDraggable : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [ShowInInspector]
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

    //public List<CubismDrawable> Drawables = new List<CubismDrawable>();
    public void OnPointerDown(PointerEventData eventData)
    {
        //Drawables.Clear();
        CubismRaycastHit[] Results = new CubismRaycastHit[5]; // init 5 slot
        int castCount = CubismManager.Instance.CurCharacter.live2DCharInteract.GetRayCastDrawableArtMesh(ref Results);
        isInteracting = castCount > 0 && CubismManager.Instance.CurCharacter.StartInteract(Results);
        // if (castCount > 0)
        // {
        //     foreach (var hit in Results)
        //     {
        //         Drawables.Add(hit.Drawable);
        //     }
        // }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isInteracting = false;
        CubismManager.Instance.CurCharacter.EndInteract();
    }

    public void OnClickToggleMove()
    {
        DataManager.GameSettings.allowDragPreview = !DataManager.GameSettings.allowDragPreview;
        
        if (dragToggle)
            dragToggle.isOn = !DataManager.GameSettings.allowDragPreview;
    }
}