//Author: sora

using UnityEditor;
using UnityEngine;

namespace Sora.Tools.CSVLoader.Editor
{
    [CustomPropertyDrawer(typeof(IntArray2DProperty), true)]
    public class IntArray2DPropertyDrawer : BaseArray2DPropertyDrawer
    {
        private const float minWidth = 26f;
        protected override void DrawPropertyArray2D(Rect position, SerializedProperty property, SerializedProperty propertyValue, Rect indexRect, SerializedProperty indexProperty)
        {
            indexProperty.intValue = EditorGUI.IntField(indexRect, indexProperty.intValue);
        }
        protected override bool DisplayScrollView() => (EditorGUIUtility.currentViewWidth - 20f) / width < minWidth;
        protected override float ItemWidth()
        {
            var itemWidth = (EditorGUIUtility.currentViewWidth - 20f) / width;
            return itemWidth < minWidth ? minWidth : itemWidth;
        }
    }
}