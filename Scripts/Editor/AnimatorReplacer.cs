#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using UnityEditor.Animations;

public class AnimatorReplacer : OdinEditorWindow
{
    // [FolderPath]
    // public string newFolderPath = "Assets/H210/Live2DModel";
    [FolderPath]
    public string characterPath;
    public AnimatorController AnimatorController;
    public List<string> remainStates;
    [MenuItem("Tools/210/Animator Replacer")]
    public static void ShowExample()
    {
        AnimatorReplacer wnd = GetWindow<AnimatorReplacer>();
        wnd.titleContent = new GUIContent("Animator Replacer");
    }
    
    [Button(ButtonSizes.Large)]
    void Replace()
    {
        remainStates = new List<string>();
        var count = AnimatorController.layers.Length;
        for (int i = 0; i < count; i++)
        {
            Debug.Log(AnimatorController.layers[i].name + "===========================================");
            var states = AnimatorController.layers[i].stateMachine.states;
            ReplaceAnimStates(states);

            var subMachines = AnimatorController.layers[i].stateMachine.stateMachines;
            for (int j = 0; j < subMachines.Length; j++)
            {
                Debug.Log(subMachines[j].stateMachine.name);
                var subStates = subMachines[j].stateMachine.states;
                ReplaceAnimStates(subStates);
            }
        }
        EditorUtility.SetDirty(AnimatorController);
        //AssetDatabase.SaveAssetIfDirty(AnimatorController);
        Debug.Log("I'm done, babe !");
    }

    void ReplaceAnimStates(ChildAnimatorState[] states)
    {
        for (int j = 0; j < states.Length; j++)
        {
            var stateName = states[j].state.name;
            if (stateName.Contains("[Ignore]"))
            {
                continue;
            }
                
            //string clipFile = $"{newFolderPath}/{characterPath}/anim/{stateName}.anim";
            string clipFile = $"{characterPath}/anim/{stateName}.anim";

            var loadedClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(clipFile);

            if (loadedClip == null)
            {
                Debug.Log($"State: <color=red>{stateName}</color> doesn't has motion ref!!!");
                remainStates.Add(stateName);
                continue;
            }

            states[j].state.motion = loadedClip;
        }
    }
    
    [Button(ButtonSizes.Large)]
    void CheckNullStateInAnimator()
    {
        Debug.Log(" START CHECK");
        var count = AnimatorController.layers.Length;
        for (int i = 0; i < count; i++)
        {
            var states = AnimatorController.layers[i].stateMachine.states;
            CheckNullState(states);

            var subMachines = AnimatorController.layers[i].stateMachine.stateMachines;
            for (int j = 0; j < subMachines.Length; j++)
            {
                var subStates = subMachines[j].stateMachine.states;
                CheckNullState(subStates);
            }
        }
        Debug.Log(" DONE CHECK");
    }

    void CheckNullState(ChildAnimatorState[] states)
    {
        for (int j = 0; j < states.Length; j++)
        {
            var stateName = states[j].state.name;
            if (stateName.Contains("[Ignore]"))
            {
                continue;
            }

            if (states[j].state.motion == null)
            {
                Debug.Log("NULL STATE: " + stateName);
            }
        }
    }
}
#endif    