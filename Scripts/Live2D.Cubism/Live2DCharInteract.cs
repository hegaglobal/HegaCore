using System;
using System.Collections.Generic;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;
using Live2D.Cubism.Framework.Raycasting;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace HegaCore
{
    [RequireComponent(typeof(CubismRaycaster))]
    public class Live2DCharInteract : MonoBehaviour
    {
        [ShowInInspector, ReadOnly] private Vector2 dragDelta;

        private CubismController _cubismController;
        private CharacterVoice _characterVoice;
        
        [ListDrawerSettings(HideAddButton = false,Expanded = true,DraggableItems = true,HideRemoveButton = false)]
        [Searchable(FilterOptions = SearchFilterOptions.ISearchFilterableInterface)]
        public List<InteractPart> InteractParts;

        
        private CubismRaycaster cubismRaycaster;
        private bool isInteracting = false;
        private bool needReset = false;
        private InteractPart curPart;

        public Action OnInteractEnoughToGetReward;
        
#if UNITY_EDITOR
        [Button(ButtonSizes.Large), GUIColor(1f,0f,1f)]
        public void SetUpRaycast()
        {
            var drawables = transform.Find("Drawables");
            
            foreach (var part in InteractParts)
            {
                foreach (var meshName in part.artMeshNames)
                {
                    var obj = drawables.Find(meshName);
                    if (obj != null)
                    {
                        if (!obj.TryGetComponent<CubismRaycastable>(out CubismRaycastable r))
                        {
                            var newR = obj.gameObject.AddComponent<CubismRaycastable>();
                            newR.Precision = CubismRaycastablePrecision.Triangles;
                            EditorUtility.SetDirty(obj);
                        }
                    }
                }
            }
        }
#endif
        void Awake()
        {
            _cubismController = GetComponentInParent<CubismController>();
            cubismRaycaster = GetComponent<CubismRaycaster>();
            _characterVoice = GetComponentInParent<CharacterVoice>();
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
                    if (curPart.ignoreDelay > 0)
                    {
                        curPart.ignoreDelay -= Time.deltaTime;
                        return;
                    }
                    
                    if (_characterVoice != null)
                        _characterVoice.PlayAngryVoice();

                    if (curPart.Ignore(_cubismController.UserCharacter.HeartLevel))
                    {
                        curPart.ignoreDelay = 2f;
                    }
                    else
                    {
                        EndInteract(true);
                        return;
                    }
                }
                
                if (curPart.currentParamValue >= 0.8f)
                {
                    OnInteractEnoughToGetReward?.Invoke();
                }

                dragDelta = Vector2.zero;
            }
            // else if (needReset)
            // {
            //     foreach (var part in InteractParts)
            //     {
            //         part.BlendPrameter(part.normalValue);
            //     }
            //
            //     needReset = false;
            // }
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

        public void EndInteract(bool forceEnd = false)
        {
            if (isInteracting)
            {
                curPart.EndDrag(forceEnd);
                curPart = null;
            }

            isInteracting = false;
        }

        public void ResetInteractValue()
        {
            //UnuLogger.Log("Reset ------------------ ");
            needReset = true;
        }

        public void LoadInteractPartValues(Dictionary<string, float> savedDict, string subFix)
        {
            if (savedDict == null || savedDict.Count == 0)
            {
                return;
            }
            foreach (var part in InteractParts)
            {
                //foreach (var savedPair in savedDict)
                //{
                    if (part.returnWeight > 0 || !part.allowedClotheIDs.Contains(_cubismController.curClothesID))
                    {
                        //UnuLogger.Log( $"RESET PARAM: --------- {part.Parameter.gameObject.name} --  {part.normalValue}"  );
                        _cubismController.BlendParamToValue(part.Parameter.name, part.normalValue);
                        continue;
                    }

                    string converted = $"{subFix}_{part.Parameter.name}";
                    //UnuLogger.Log($"CONVERTED: {converted}");
                
                    if (savedDict.TryGetValue(converted, out var value))// string.Equals(converted, savedPair.Key))
                    {
                        //UnuLogger.Log($"LOAD: +++++++++ {part} == {converted} --- Load: {value}");
                        //part.BlendPrameter(value);
                        _cubismController.BlendParamToValue(part.Parameter.name, value);
                    }
                //}
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
public class InteractPart : ISearchFilterable
{
    [Title("$partName", " ============================== ",TitleAlignments.Centered)]
    [InlineButton("Rename")]
    [GUIColor(1f, 1f, 0f)]
    public string partName = string.Empty;
    
    public CubismParameter Parameter;
    public List<string> artMeshNames;
    public List<int> allowedClotheIDs;
    
    [Title("Stats")]
    public float normalValue = 0;
    public float dragValue = 1;
    public Vector2 dragDirection;
    public float dragMultiplier = 0.01f;

    [Title("React")] 
    public bool canReact = false;
    [ShowIf("canReact")] 
    public float reactValue;
    [ShowIf("canReact")] 
    public float ignoreAtLevel = -1;
    [ShowIf("canReact")] 
    public float reactReturn;
    [ShowIf("canReact")] 
    public float reactReturnDelay;
    
    [InfoBox("Set returnWeight to 0 to reject return and save value.")]
    [Space(10)]
    [Title("AUTO RETURN SETTING")] 
    public float returnWeight = 5;
    public float returnDelay = 0;
    public UnityEvent OnReturnCompleted;
    
    [ReadOnly][GUIColor(0f, 1f, 0f)]
    public float currentParamValue;
    
    [ShowInInspector, ReadOnly]
    private bool isInNormal = true;
    [ShowInInspector, ReadOnly]
    private float curReturnDelay;
    [ShowInInspector, ReadOnly]
    private float returnSpeed;

    [HideInInspector]
    public float ignoreDelay = 0f;
    
    public void DoReturn()
    {
        if (returnSpeed != 0 && !isInNormal)  // (returnWeight > 0 || reactReturn > 0 || returnSpeed > 0)
        {
            curReturnDelay -= Time.deltaTime;
            if (curReturnDelay > 0)
            {
                return;
            }
            
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

    public bool Ignore(int level)
    {
        return level >= ignoreAtLevel;
    }
    
    public bool NeedReact()
    {
        if (canReact)
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
        }

        return false;
    }
    
    public void EndDrag(bool forceEnd = false)
    {
        ignoreDelay = 0;
        if (forceEnd)
        {
            returnSpeed = dragMultiplier * reactReturn * (normalValue - dragValue);
            curReturnDelay = reactReturnDelay;
        }
        else
        {
            returnSpeed = dragMultiplier * returnWeight * (normalValue - dragValue);
            curReturnDelay = returnDelay;
        }
        //UnuLogger.Log( Parameter.gameObject.name + "  Return Speed: ---------------------------------- " + returnSpeed);
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

    public bool IsMatch(string searchString)
    {
        return string.IsNullOrEmpty(searchString) || partName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0;
    }
}
