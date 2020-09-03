using System;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace HegaCore.Editor
{
    public sealed class ColliderEditor : MonoBehaviour
    {
        [MenuItem("Tools/Edit Collider2D %_e")] // CTRL/CMD + E
        public static void EditCollider2D()
        {
            if (!Selection.activeGameObject)
                return;

            if (!Selection.activeGameObject.TryGetComponent<Collider2D>(out var collider))
                return;

            if (EditMode.editMode == EditMode.SceneViewEditMode.Collider)
            {
                EditMode.ChangeEditMode(EditMode.SceneViewEditMode.None, new Bounds(), null);
                return;
            }

            var colliderEditorBase = Type.GetType("UnityEditor.ColliderEditorBase,UnityEditor.dll");

            if (!(Resources.FindObjectsOfTypeAll(colliderEditorBase) is UnityEditor.Editor[] colliderEditors))
                return;

            foreach (var editor in colliderEditors)
            {
                if (editor.target == collider)
                {
                    EditMode.ChangeEditMode(EditMode.SceneViewEditMode.Collider, collider.bounds, editor);
                    return;
                }
            }
        }
    }
}