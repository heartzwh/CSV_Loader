//Author: sora

using UnityEditor;
using UnityEngine;

namespace Sora.Tools.CSVLoader.Editor
{
    [CustomPropertyDrawer(typeof(FloatArray2DProperty), true)]
    public class FloatArray2DPropertyDrawer : BaseArray2DPropertyDrawer
    {
        private const float minWidth = 26f;
        protected override void DrawPropertyArray2D(Rect position, SerializedProperty property, SerializedProperty propertyValue, Rect indexRect, SerializedProperty indexProperty)
        {
            indexProperty.floatValue = EditorGUI.FloatField(indexRect, indexProperty.floatValue);
        }
        protected override bool DisplayScrollView() => (EditorGUIUtility.currentViewWidth - 20f) / width < minWidth;
        protected override float ItemWidth()
        {
            var itemWidth = (EditorGUIUtility.currentViewWidth - 20f) / width;
            return itemWidth < minWidth ? minWidth : itemWidth;
        }
    }
}