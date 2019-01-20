//Author: sora

using UnityEditor;
using UnityEngine;

namespace Sora.Tools.CSVLoader.Editor
{
    [CustomPropertyDrawer(typeof(IProperty), true)]
    public abstract class BaseArray2DPropertyDrawer : BasePropertyDrawer
    {
        protected const float scrollbarHeight = 50f;
        protected Vector2 scrollPosition { get; private set; }
        protected int width { get; private set; }
        protected int height { get; private set; }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return property.FindPropertyRelative("height").intValue * EditorGUIUtility.singleLineHeight + scrollbarHeight;
        }

        protected override void DrawProperty(Rect position, SerializedProperty property, SerializedProperty propertyValue)
        {
            var itemWidth = ItemWidth();
            width = property.FindPropertyRelative("width").intValue;
            height = property.FindPropertyRelative("height").intValue;
            var scrollViewFlag = DisplayScrollView();
            if (scrollViewFlag)
            {
                var viewRect = new Rect(position.x, position.y, itemWidth * width, height * EditorGUIUtility.singleLineHeight + scrollbarHeight);
                scrollPosition = GUI.BeginScrollView(position, scrollPosition, viewRect, true, false);
            }
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var indexRect = new Rect(position.x + x * itemWidth, position.y + y * EditorGUIUtility.singleLineHeight, itemWidth, EditorGUIUtility.singleLineHeight);
                    var indexProperty = propertyValue.GetArrayElementAtIndex(x + width * y);
                    // indexProperty.boolValue = EditorGUI.Toggle(indexRect, indexProperty.boolValue);
                    DrawPropertyArray2D(position, property, propertyValue, indexRect, indexProperty);
                }
            }
            if (scrollViewFlag)
            {
                GUI.EndScrollView();
            }
        }

        protected abstract bool DisplayScrollView();
        protected abstract float ItemWidth();
        protected abstract void DrawPropertyArray2D(Rect position, SerializedProperty property, SerializedProperty propertyValue, Rect indexRect, SerializedProperty indexProperty);
    }
}