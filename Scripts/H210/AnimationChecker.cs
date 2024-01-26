#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class AnimationChecker : OdinEditorWindow
{
    public string newFolderPath = "Assets/H210/Live2DModel";
    public string characterPath;

    public string parameterName;
    public List<TextAsset> allMotions;
    
    [MenuItem("Tools/210/Animation Checker")]
    public static void ShowExample()
    {
        AnimationChecker wnd = GetWindow<AnimationChecker>();
        wnd.titleContent = new GUIContent("Animation Checker");
    }

    [Button]
    void CheckParam()
    {
        if (string.IsNullOrEmpty(parameterName))
        {
            Debug.LogError("NULL PARAMETER NAME !!!!!!!!!!!!!!!!!!!!!!");
            return;
        }
        
        Debug.Log($"Start Check -------------- <color=green>{characterPath}</color> -- <color=red>{parameterName}</color>");

        allMotions = new List<TextAsset>();
        
        string folderpath = $"{newFolderPath}/{characterPath}/anim";
        if (Directory.Exists(folderpath))
        {
            DirectoryInfo info = new DirectoryInfo(folderpath);
            var files = info.GetFiles();
            foreach (var file in files)
            {
                if (string.Equals(file.Extension, ".json"))
                {
                    string motionFile = $"{newFolderPath}/{characterPath}/anim/{file.Name}";
                    
                    var loadedMotion = AssetDatabase.LoadAssetAtPath<TextAsset>(motionFile);
                    if (loadedMotion == null)
                    {
                        Debug.Log($"????? <color=green>{motionFile}</color>");
                        continue;
                    }

                    string loadedText = loadedMotion.text;
                    if (loadedText.Contains(parameterName))
                    {
                        Debug.Log("<color=red>DETECT: -------------------- </color>" + file.Name);
                        allMotions.Add(loadedMotion);
                    }
                }
            }
        }
        else
        {
            Debug.Log("No FOLDER:  " + folderpath);
        }
        
        Debug.Log("End CHECK !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
    }
}
#endif