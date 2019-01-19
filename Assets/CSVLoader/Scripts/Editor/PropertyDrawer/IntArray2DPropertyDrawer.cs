//Author: sora

using UnityEditor;
using UnityEngine;

namespace Sora.Tools.CSVLoader.Editor
{
    [CustomPropertyDrawer(typeof(IntArray2DProperty), true)]
    public class IntArray2DPropertyDrawer : PropertyDrawer
    {
        private const float scrollbarHeight = 50f;
        private Vector2 scrollPosition;
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return property.FindPropertyRelative("height").intValue * EditorGUIUtility.singleLineHeight + scrollbarHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var itemMinWidth = 30f;
            EditorGUI.BeginProperty(position, label, property);
            property.serializedObject.Update();
            var width = property.FindPropertyRelative("width").intValue;
            var height = property.FindPropertyRelative("height").intValue;
            var propertyValue = property.FindPropertyRelative("propertyValue");
            var itemWidth = (EditorGUIUtility.currentViewWidth - 20f) / width;
            var scrollViewFlag = itemWidth < itemMinWidth;
            if (scrollViewFlag)
            {
                itemWidth = itemMinWidth;
                var viewRect = new Rect(position.x, position.y, itemMinWidth * width, height * EditorGUIUtility.singleLineHeight + scrollbarHeight);
                scrollPosition = GUI.BeginScrollView(position, scrollPosition, viewRect, true, false);
            }
            EditorGUI.BeginDisabledGroup(true);
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var itemRect = new Rect(position.x + x * itemWidth, position.y + y * EditorGUIUtility.singleLineHeight, itemWidth, EditorGUIUtility.singleLineHeight);
                    var item = propertyValue.GetArrayElementAtIndex(x + width * y);
                    item.intValue = EditorGUI.IntField(itemRect, item.intValue);
                }
            }
            EditorGUI.EndDisabledGroup();
            if (scrollViewFlag)
            {
                GUI.EndScrollView();
            }
            property.serializedObject.ApplyModifiedProperties();
            EditorGUI.EndProperty();
        }
    }
}