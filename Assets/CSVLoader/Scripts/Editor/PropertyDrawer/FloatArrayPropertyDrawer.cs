//Author: sora

using UnityEditor;
using UnityEngine;

namespace Sora.Tools.CSVLoader.Editor
{
    [CustomPropertyDrawer(typeof(FloatArrayProperty), true)]
    public class FloatArrayPropertyDrawer : BaseArrayPropertyDrawer
    {
        protected override void DrawPropertyArray(Rect position, SerializedProperty property, SerializedProperty propertyValue, SerializedProperty indexProperty, Rect indexRect, string indexTitle)
        {
            indexProperty.floatValue = EditorGUI.FloatField(indexRect, indexTitle, indexProperty.floatValue);
        }
    }
}