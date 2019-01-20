//Author: sora

using UnityEditor;
using UnityEngine;

namespace Sora.Tools.CSVLoader.Editor
{
    [CustomPropertyDrawer(typeof(BooleanArray2DWithnameProperty), true)]
    public class BooleanArray2DWithnamePropertyDrawer : BaseArray2DWithnamePropertyDrawer
    {
        private const float minWidth = 26f;
        protected override void DrawPropertyArray2D(Rect position, SerializedProperty property, SerializedProperty propertyValue, Rect indexRect, SerializedProperty indexProperty)
        {
            indexProperty.boolValue = EditorGUI.Toggle(indexRect, indexProperty.boolValue);
        }

        protected override bool DisplayScrollView() => (ItemWidth() * width - namelabelWidth) > EditorGUIUtility.currentViewWidth;
        protected override float ItemWidth()
        {
            var itemWidth = (EditorGUIUtility.currentViewWidth - 20f) / width;
            return itemWidth < minWidth ? minWidth : itemWidth;
        }
    }
}