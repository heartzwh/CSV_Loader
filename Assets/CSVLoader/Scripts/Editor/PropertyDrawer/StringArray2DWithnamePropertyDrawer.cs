//Author: sora

using UnityEditor;
using UnityEngine;

namespace Sora.Tools.CSVLoader.Editor
{
    [CustomPropertyDrawer(typeof(StringArray2DWithnameProperty), true)]
    public class StringArray2DWithnamePropertyDrawer : BaseArray2DWithnamePropertyDrawer
    {
        protected override void DrawPropertyArray2D(Rect position, SerializedProperty property, SerializedProperty propertyValue, Rect indexRect, SerializedProperty indexProperty)
        {
            indexProperty.stringValue = EditorGUI.TextField(indexRect, indexProperty.stringValue);
        }
    }
}