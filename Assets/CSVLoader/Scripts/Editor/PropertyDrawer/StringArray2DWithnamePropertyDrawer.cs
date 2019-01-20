//Author: sora

using UnityEditor;
using UnityEngine;

namespace Sora.Tools.CSVLoader.Editor
{
    [CustomPropertyDrawer(typeof(StringArray2DWithnameProperty), true)]
    public class StringArray2DWithnamePropertyDrawer : BaseArray2DWithnamePropertyDrawer
    {
        private const float minWidth = 26f;
        protected override void DrawPropertyArray2D(Rect position, SerializedProperty property, SerializedProperty propertyValue, Rect indexRect, SerializedProperty indexProperty)
        {
            indexProperty.stringValue = EditorGUI.TextField(indexRect, indexProperty.stringValue);
        }
        protected override bool DisplayScrollView() => (EditorGUIUtility.currentViewWidth - 20f - namelabelWidth) / width < minWidth;
        protected override float ItemWidth()
        {
            var itemWidth = (EditorGUIUtility.currentViewWidth - 20f) / width;
            return itemWidth < minWidth ? minWidth : itemWidth;
        }
    }
}