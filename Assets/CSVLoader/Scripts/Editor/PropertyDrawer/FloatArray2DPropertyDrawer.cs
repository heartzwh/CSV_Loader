//Author: sora

using UnityEditor;
using UnityEngine;

namespace Sora.Tools.CSVLoader.Editor
{
    [CustomPropertyDrawer(typeof(FloatArray2DProperty), true)]
    public class FloatArray2DPropertyDrawer : BaseArray2DPropertyDrawer
    {
        protected override void DrawPropertyArray2D(Rect position, SerializedProperty property, SerializedProperty propertyValue, Rect indexRect, SerializedProperty indexProperty)
        {
            indexProperty.floatValue = EditorGUI.FloatField(indexRect, indexProperty.floatValue);
        }
    }
}