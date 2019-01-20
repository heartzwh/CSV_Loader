//Author: sora

using UnityEditor;
using UnityEngine;

namespace Sora.Tools.CSVLoader.Editor
{
    [CustomPropertyDrawer(typeof(IProperty), true)]
    public abstract class BaseArray2DWithnamePropertyDrawer : BaseArray2DPropertyDrawer
    {
        protected float namelabelWidth;
        protected SerializedProperty rowNames;
        protected SerializedProperty columnNames;
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return property.FindPropertyRelative("height").intValue * EditorGUIUtility.singleLineHeight + scrollbarHeight + EditorGUIUtility.singleLineHeight;
        }

        protected override void DrawProperty(Rect position, SerializedProperty property, SerializedProperty propertyValue)
        {
            namelabelWidth = EditorGUIUtility.currentViewWidth * .2f;
            var itemWidth = ItemWidth();
            if (namelabelWidth > itemWidth)
            {
                itemWidth = namelabelWidth;
            }
            else namelabelWidth = itemWidth;
            width = property.FindPropertyRelative("width").intValue;
            height = property.FindPropertyRelative("height").intValue;
            rowNames = property.FindPropertyRelative("rowNames");
            columnNames = property.FindPropertyRelative("columnNames");
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
                    if (y.Equals(0))
                    {
                        var indexRect = new Rect(position.x + (x) * namelabelWidth, position.y, itemWidth, EditorGUIUtility.singleLineHeight);
                        var indexProperty = rowNames.GetArrayElementAtIndex(x);
                        EditorGUI.LabelField(indexRect, indexProperty.stringValue);
                    }
                    else if (x.Equals(0))
                    {
                        var indexRect = new Rect(position.x, position.y + (y) * EditorGUIUtility.singleLineHeight, itemWidth, EditorGUIUtility.singleLineHeight);
                        var indexProperty = columnNames.GetArrayElementAtIndex(y);
                        EditorGUI.LabelField(indexRect, indexProperty.stringValue);
                    }
                    else
                    {
                        var indexRect = new Rect(position.x + x * itemWidth, position.y + y * EditorGUIUtility.singleLineHeight, itemWidth, EditorGUIUtility.singleLineHeight);
                        var indexProperty = propertyValue.GetArrayElementAtIndex(x + width * y);
                        DrawPropertyArray2D(position, property, propertyValue, indexRect, indexProperty);
                    }
                }
            }
            if (scrollViewFlag)
            {
                GUI.EndScrollView();
            }
        }
    }
}