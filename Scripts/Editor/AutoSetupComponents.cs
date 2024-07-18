#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;
using Live2D.Cubism.Framework.Physics;
using Live2D.Cubism.Framework.Raycasting;
using Live2D.Cubism.Rendering;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace HegaCore
{
    public class AutoSetupComponents : OdinEditorWindow
    {
        public GameObject fromObject;
        public GameObject toTarget;

        public List<MonoBehaviour> leftOverObjects;

        [MenuItem("Tools/210/Auto Setup Components")]
        public static void ShowExample()
        {
            AutoSetupComponents wnd = GetWindow<AutoSetupComponents>();
            wnd.titleContent = new GUIContent("Auto Setup Components");
        }

        // [Button(ButtonSizes.Large)]
        // void Setup()
        // {
        //     if (fromObject == null || toTarget == null)
        //     {
        //         Debug.LogError("NULLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL");
        //         return;
        //     }
        //
        //     leftOverObjects = new List<MonoBehaviour>();
        //
        //     Copy<CubismParameterCustomControl>(fromObject, toTarget.transform.Find("Parameters"));
        //
        //     var physicComp = toTarget.GetComponent<CubismPhysicsController>();
        //     if (physicComp)
        //     {
        //         DestroyImmediate(physicComp);
        //     }
        //
        //     var store = toTarget.GetComponent<CubismParameterStore>();
        //     if (store)
        //         store.enabled = false;
        //
        //     var oldAnimator = fromObject.GetComponent<Animator>();
        //     var newAnimator = toTarget.GetComponent<Animator>();
        //     newAnimator.runtimeAnimatorController = oldAnimator.runtimeAnimatorController;
        //
        //     EditorUtility.SetDirty(toTarget);
        // }
        
        
        [Button(ButtonSizes.Large)]
        void Setup()
        {
            if (fromObject == null || toTarget == null)
            {
                Debug.LogError("NULLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL");
                return;
            }

            leftOverObjects = new List<MonoBehaviour>();
            Dictionary<string, CubismParameter> customDict =
                new Dictionary<string, CubismParameter>();
            
            Copy<CubismParameterCustomControl>(fromObject, toTarget.transform.Find("Parameters"),
                (f, t) =>
                {
                    t.customParamName = f.customParamName;
                    customDict.Add(t.name, t.GetComponent<CubismParameter>());
                });
            
            var physicComp = toTarget.GetComponent<CubismPhysicsController>();
            if (physicComp)
            {
                DestroyImmediate(physicComp);
            }

            var store = toTarget.GetComponent<CubismParameterStore>();
            if (store)
                store.enabled = false;

            var oldAnimator = fromObject.GetComponent<Animator>();
            var newAnimator = toTarget.GetComponent<Animator>();
            newAnimator.runtimeAnimatorController = oldAnimator.runtimeAnimatorController;

            var oldCubismRenderController = fromObject.GetComponent<CubismRenderController>();
            var newCubismRenderController = toTarget.GetComponent<CubismRenderController>();
            newCubismRenderController.MutualTexturePath = oldCubismRenderController.MutualTexturePath;
            newCubismRenderController.UseMutualTexturePath = oldCubismRenderController.UseMutualTexturePath;
            newCubismRenderController.ForceUseTexturePath = oldCubismRenderController.ForceUseTexturePath;
            
            var fromInteract = fromObject.GetComponent<Live2DCharInteract>();
            if (fromInteract)
            {
                var drawables = toTarget.transform.Find("Drawables");
                var toInteract = toTarget.AddComponent<Live2DCharInteract>();
                toInteract.InteractParts = new List<InteractPart>();
                foreach (var part in fromInteract.InteractParts)
                {
                    var newPart = new InteractPart
                    {
                        partName = part.partName,
                        Parameter = customDict[part.Parameter.name],
                        artMeshNames = new List<string>()
                    };
                    
                    foreach (var meshName in part.artMeshNames)
                    {
                        newPart.artMeshNames.Add(meshName);
                        var obj = drawables.Find(meshName);
                        if (obj != null)
                        {
                            if (!obj.TryGetComponent<CubismRaycastable>(out CubismRaycastable r))
                            {
                                var newR = obj.gameObject.AddComponent<CubismRaycastable>();
                                newR.Precision = CubismRaycastablePrecision.Triangles;
                            }
                        }
                    }
                    
                    if (part.allowedClotheIDs != null && part.allowedClotheIDs.Count > 0)
                    {
                        newPart.allowedClotheIDs = new List<int>();
                        foreach (var id in part.allowedClotheIDs)
                        {
                            newPart.allowedClotheIDs.Add(id);
                        }
                    }

                    newPart.normalValue = part.normalValue;
                    newPart.dragValue = part.dragValue;
                    newPart.dragDirection = part.dragDirection;
                    newPart.dragMultiplier = part.dragMultiplier;

                    newPart.canReact = part.canReact;
                    newPart.reactValue = part.reactValue;
                    newPart.reactReturn = part.reactReturn;
                    newPart.reactReturnDelay = part.reactReturnDelay;
                    
                    newPart.returnWeight = part.returnWeight;
                    newPart.returnDelay = part.returnDelay;
                    
                    toInteract.InteractParts.Add(newPart);
                }
            }
            
            EditorUtility.SetDirty(toTarget);
        }
        
        
        private void Copy<T>(GameObject from, Transform toParent, Action<T,T> onAdd = null) where T : MonoBehaviour
        {
            var arr = from.GetComponentsInChildren<T>();
            if (arr != null && arr.Length > 0)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    var obj = toParent.Find(arr[i].name);
                    if (obj != null)
                    {
                        var newComp = obj.gameObject.AddComponent<T>();
                        Debug.Log("ADDED: " + arr[i].name);
                        onAdd?.Invoke(arr[i],newComp);
                    }
                    else
                    {
                        leftOverObjects.Add(arr[i]);
                        Debug.Log("MISSED: " + arr[i].name);
                    }
                }
            }
        }

        // private void Copy<T>(GameObject from, Transform toParent) where T : MonoBehaviour
        // {
        //     var arr = from.GetComponentsInChildren<T>();
        //     if (arr != null && arr.Length > 0)
        //     {
        //         for (int i = 0; i < arr.Length; i++)
        //         {
        //             var obj = toParent.Find(arr[i].name);
        //             if (obj != null)
        //             {
        //                 obj.gameObject.AddComponent<T>();
        //                 Debug.Log("ADDED: " + arr[i].name);
        //
        //             }
        //             else
        //             {
        //                 leftOverObjects.Add(arr[i]);
        //                 Debug.Log("MISSED: " + arr[i].name);
        //             }
        //         }
        //     }
        // }
    }
}
#endif
