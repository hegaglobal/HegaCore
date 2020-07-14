using System;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace HegaCore.Editor
{
    public sealed class ColliderEditor : MonoBehaviour
    {
        [MenuItem("Tools/Edit Collider %_e")] // CTRL/CMD + E
        public static void EditCollider()
        {
            var sel = Selection.activeGameObject;

            if (!sel)
                return;

            var col = sel.GetComponent<Collider2D>();

            if (!col)
                return;

            if (EditMode.editMode == EditMode.SceneViewEditMode.Collider)
            {
                EditMode.ChangeEditMode(EditMode.SceneViewEditMode.None, new Bounds(), null);
            }
            else
            {
                var colliderEditorBase = Type.GetType("UnityEditor.ColliderEditorBase,UnityEditor.dll");

                if (!(Resources.FindObjectsOfTypeAll(colliderEditorBase) is UnityEditor.Editor[] colliderEditors) ||
                    colliderEditors.Length <= 0)
                    return;

                EditMode.ChangeEditMode(EditMode.SceneViewEditMode.Collider, col.bounds, colliderEditors[0]);
            }
        }
    }
}