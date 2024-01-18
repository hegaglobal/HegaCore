using System;
using System.Collections.Generic;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;
using Live2D.Cubism.Framework.Raycasting;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace HegaCore
{
    [RequireComponent(typeof(CubismRaycaster))]
    public class Live2DCharInteract : MonoBehaviour
    {
        [ShowInInspector, ReadOnly] private Vector2 dragDelta;

        private CubismController _cubismController;

        public List<InteractPart> InteractParts;

        
        private CubismRaycaster cubismRaycaster;
        private bool isInteracting = false;
        private bool needReset = false;
        private InteractPart curPart;

        public Action OnInteractEnoughToGetReward;

        void Start()
        {
            _cubismController = GetComponentInParent<CubismController>();
            cubismRaycaster = GetComponent<CubismRaycaster>();
        }

        void OnEnable()
        {
            isInteracting = false;
        }

        void LateUpdate()
        {
            if (isInteracting)
            {
                curPart.DoDrag(dragDelta);

                if (curPart.NeedReact())
                {
                    EndInteract();
                    _cubismController.Animator.SetTrigger("Interact");
                    return;
                }
                
                if (curPart.currentParamValue >= 0.8f)
                {
                    OnInteractEnoughToGetReward?.Invoke();
                }

                dragDelta = Vector2.zero;
            }
            else if (needReset)
            {
                foreach (var part in InteractParts)
                {
                    part.BlendPrameter(part.normalValue);
                }

                needReset = false;
            }
            else
            {
                foreach (var part in InteractParts)
                {
                    part.DoReturn();
                }
            }
        }

        public bool StartInteract(CubismRaycastHit[] hits)
        {
            foreach (var part in InteractParts)
            {
                if (part.allowedClotheIDs.Count == 0 || part.allowedClotheIDs.Contains(_cubismController.curClothesID))
                {
                    foreach (var hit in hits)
                    {
                        if (hit.Drawable != null)
                        {
                            foreach (var meshName in part.artMeshNames)
                            {
                                if (string.Equals(hit.Drawable.name, meshName))
                                {
                                    isInteracting = true;
                                    curPart = part;
                                    curPart.StartDrag();
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        public void UpdateInteractDrag(Vector2 delta)
        {
            dragDelta = delta;
        }

        public void EndInteract()
        {
            if (isInteracting)
            {
                curPart.EndDrag();
                curPart = null;
            }

            isInteracting = false;
        }

        public Dictionary<string, float> GetInteractPartValues()
        {
            Dictionary<string, float> result = new Dictionary<string, float>();

            foreach (var part in InteractParts)
            {
                if (part.returnWeight <= 0 && part.allowedClotheIDs.Contains(_cubismController.curClothesID))
                {
                    result.Add(part.Parameter.name, part.currentParamValue);
                }
            }

            return result;
        }

        public void ResetInteractValue()
        {
            //Debug.Log("Reset ------------------ ");
            needReset = true;
        }

        public void LoadInteractPartValues(Dictionary<string, float> savedDict)
        {
            if (savedDict == null || savedDict.Count == 0)
            {
                return;
            }

            foreach (var savedPair in savedDict)
            {
                foreach (var part in InteractParts)
                {
                    if (part.returnWeight > 0 || !part.allowedClotheIDs.Contains(_cubismController.curClothesID))
                    {
                        continue;
                    }

                    if (string.Equals(part.Parameter.name, savedPair.Key))
                    {
                        part.BlendPrameter(savedPair.Value);
                    }
                }
            }
        }
        
        public int GetRayCastDrawableArtMesh(ref CubismRaycastHit[] Results)
        {
            if (!DataManager.Instance.DarkLord)
            {
                return 0;
            }

            var mouse = Input.mousePosition;
            var camera = Camera.main;
            Vector2 screen = new Vector2(camera.pixelRect.width, camera.pixelRect.height);
            Vector2 mouseConvert = new Vector2(mouse.x / screen.x * 1920, mouse.y / screen.y * 1080);
            
            var ray = DataManager.Instance.live2DCamera.ScreenPointToRay(mouseConvert);
            //Debug.DrawRay(ray.origin, ray.direction * 2000,Color.green, 20);
            
            var hitCount = cubismRaycaster.Raycast(ray, Results);
            return hitCount;
        }
    }
}

[Serializable]
public class InteractPart
{
    [Title("$partName", " ============================== ",TitleAlignments.Centered)]
    public string partName = string.Empty;
    
    [InlineButton("Rename")]
    public CubismParameter Parameter;
    public List<string> artMeshNames;
    public List<int> allowedClotheIDs;
    
    [Title("Stats")]
    public float normalValue = 0;
    public float dragValue = 1;
    public Vector2 dragDirection;
    public float dragMultiplier = 0.01f;

    [Title("React")] 
    public float reactValue;
    
    [Title("Return")] 
    //Use Anim to Return part to normal
    [InfoBox("Set returnWeight to 0 to reject return and save value.")]
    [Space(10)]
    [FoldoutGroup("RETURN SETTING", false)]
    public float returnWeight = 5;
    [FoldoutGroup("RETURN SETTING",false)]
    private float returnSpeed;
    [FoldoutGroup("RETURN SETTING",false)]
    public UnityEvent OnReturnCompleted;
    
    [ReadOnly]
    public float currentParamValue;
    
    [ShowInInspector, ReadOnly]
    private bool isInNormal = true;
    
    public void DoReturn()
    {
        if (returnWeight > 0 && !isInNormal)
        {
            currentParamValue += returnSpeed;
    
            BlendPrameter();
            if (Mathf.Abs(currentParamValue - normalValue) < 0.01f)
            {
                isInNormal = true;
                OnReturnCompleted?.Invoke();
            }
        }
    }

    public void StartDrag()
    {
        currentParamValue = Parameter.Value;
        isInNormal = false;
    }

    public void DoDrag(Vector2 delta)
    {
        var dragAngle = Vector2.Angle(dragDirection, delta);

        if (dragAngle < 90)
        {
            currentParamValue += dragMultiplier * delta.magnitude;
        }
        else
        {
            currentParamValue -= dragMultiplier * delta.magnitude;
        }
        
        BlendPrameter();
    }

    public bool NeedReact()
    {
        if (reactValue < normalValue && reactValue > dragValue)
        {
            if (currentParamValue > reactValue)
            {
                return true;
            }
        }
        else if (reactValue > normalValue && reactValue < dragValue)
        {
            if (currentParamValue > reactValue)
            {
                return true;
            }
        }

        return false;
    }
    
    public void EndDrag()
    {
        returnSpeed = dragMultiplier * returnWeight * (normalValue - dragValue);
    }

    //[Button("Blend Param", ButtonSizes.Large)]
    private void BlendPrameter()
    {
        currentParamValue = normalValue > dragValue 
            ? Mathf.Clamp(currentParamValue, dragValue, normalValue) 
            : Mathf.Clamp(currentParamValue, normalValue,dragValue);
            
        //Debug.Log($"Blend: {Parameter.name} -- {currentParamValue}");
        Parameter.BlendToValue(CubismParameterBlendMode.Override, currentParamValue);
    }

    public void BlendPrameter(float newValue)
    {
        currentParamValue = newValue;
        BlendPrameter();
    }

    public void Rename()
    {
        partName = Parameter != null ? Parameter.gameObject.name : string.Empty;
    }
}
