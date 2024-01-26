#if UNITY_EDITOR
using System.Collections.Generic;
using Live2D.Cubism.Framework;
using Live2D.Cubism.Framework.Physics;
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

        [Button(ButtonSizes.Large)]
        void Setup()
        {
            if (fromObject == null || toTarget == null)
            {
                Debug.LogError("NULLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL");
                return;
            }

            leftOverObjects = new List<MonoBehaviour>();

            Copy<CubismParameterCustomControl>(fromObject, toTarget.transform.Find("Parameters"));

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

            EditorUtility.SetDirty(toTarget);
        }

        private void Copy<T>(GameObject from, Transform toParent) where T : MonoBehaviour
        {
            var arr = from.GetComponentsInChildren<T>();
            if (arr != null && arr.Length > 0)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    var obj = toParent.Find(arr[i].name);
                    if (obj != null)
                    {
                        obj.gameObject.AddComponent<T>();
                        Debug.Log("ADDED: " + arr[i].name);

                    }
                    else
                    {
                        leftOverObjects.Add(arr[i]);
                        Debug.Log("MISSED: " + arr[i].name);
                    }
                }
            }
        }
    }
}
#endif
