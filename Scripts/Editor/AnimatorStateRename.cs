#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class AnimatorStateRename : OdinEditorWindow
{
    public AnimatorController AnimatorController;

    public List<StateInfo> needRenameState;
    [MenuItem("Tools/210/Animator State Rename")]
    public static void ShowExample()
    {
        AnimatorStateRename wnd = GetWindow<AnimatorStateRename>();
        wnd.titleContent = new GUIContent("Animator State Rename");
    }
    
    [Button(ButtonSizes.Large)]
    void CompareName()
    {
        needRenameState = new List<StateInfo>();
        var count = AnimatorController.layers.Length;
        for (int i = 0; i < count; i++)
        {
            Debug.Log(AnimatorController.layers[i].name + "===========================================");
            var states = AnimatorController.layers[i].stateMachine.states;
            for (int j = 0; j < states.Length; j++)
            {
                var stateName = states[j].state.name;
                if (stateName.Contains("[Ignore]"))
                {
                    continue;
                }

                if (states[j].state.motion == null)
                {
                    Debug.Log($"{stateName} -- null clip");
                    continue;
                }

                if (!string.Equals(states[j].state.motion.name , stateName))
                {
                    needRenameState.Add(new StateInfo(states[j].state.motion.name,states[j].state, states[j].state.motion));
                    //Debug.Log($"{stateName} -- has different clip name !!!!!!!!!!!!!!!");
                }
            }
        }
    }
}

[System.Serializable]
public class StateInfo
{
    [InlineButton("RenameStateByMotion", "Rename")]
    public string name;
    private AnimatorState state;
    private Motion motion;

    public StateInfo(string n, AnimatorState s, Motion m)
    {
        name = n;
        state = s;
        motion = m;
    }

    public void RenameStateByMotion()
    {
        state.name = motion.name;
    }
}
#endif