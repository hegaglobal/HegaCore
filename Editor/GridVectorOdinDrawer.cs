using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;

namespace HegaCore.Editor
{
    [DrawerPriority(DrawerPriorityLevel.SuperPriority)]
    public sealed class GridVectorOdinDrawer : OdinValueDrawer<GridVector>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            Rect rect = EditorGUILayout.GetControlRect();

            if (label != null)
            {
                rect = EditorGUI.PrefixLabel(rect, label);
            }

            var value = this.ValueEntry.SmartValue;
            var rowLbl = new GUIContent("R", nameof(value.Row));
            var colLbl = new GUIContent("C", nameof(value.Column));

            GUIHelper.PushLabelWidth(14f);
            value.Row = EditorGUI.IntField(rect.AlignLeft(rect.width * 0.5f), rowLbl, value.Row);
            value.Column = EditorGUI.IntField(rect.AlignRight(rect.width * 0.5f - 2f), colLbl, value.Column);
            GUIHelper.PopLabelWidth();

            this.ValueEntry.SmartValue = value;
        }
    }
}