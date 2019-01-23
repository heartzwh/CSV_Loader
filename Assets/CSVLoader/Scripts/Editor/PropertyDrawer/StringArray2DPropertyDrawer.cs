//Author: sora

using UnityEditor;
using UnityEngine;

namespace Sora.Tools.CSVLoader.Editor
{
    [CustomPropertyDrawer(typeof(StringArray2DProperty), true)]
    public class StringArray2DPropertyDrawer : BaseArray2DPropertyDrawer
    {
        protected override void DrawPropertyArray2D(Rect position, SerializedProperty property, SerializedProperty propertyValue, Rect indexRect, SerializedProperty indexProperty)
        {
            indexProperty.stringValue = EditorGUI.TextField(indexRect, indexProperty.stringValue);
        }
    }
}