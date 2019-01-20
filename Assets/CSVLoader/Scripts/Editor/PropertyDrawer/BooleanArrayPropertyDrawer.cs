//Author: sora

using UnityEditor;
using UnityEngine;

namespace Sora.Tools.CSVLoader.Editor
{
    [CustomPropertyDrawer(typeof(BooleanArrayProperty), true)]
    public class BooleanArrayPropertyDrawer : BaseArrayPropertyDrawer
    {
        protected override void DrawPropertyArray(Rect position, SerializedProperty property, SerializedProperty propertyValue, SerializedProperty indexProperty, Rect indexRect, string indexTitle)
        {
            indexProperty.boolValue = EditorGUI.Toggle(indexRect, indexTitle, indexProperty.boolValue);
        }
    }
}