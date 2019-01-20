//Author: sora

using UnityEditor;
using UnityEngine;

namespace Sora.Tools.CSVLoader.Editor
{
    [CustomPropertyDrawer(typeof(StringArrayProperty), true)]
    public class StringArrayPropertyDrawer : BaseArrayPropertyDrawer
    {
        protected override void DrawPropertyArray(Rect position, SerializedProperty property, SerializedProperty propertyValue, SerializedProperty indexProperty, Rect indexRect, string indexTitle)
        {
            indexProperty.stringValue = EditorGUI.TextField(indexRect, indexTitle, indexProperty.stringValue);
        }
    }
}