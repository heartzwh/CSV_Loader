//Author: sora

using UnityEditor;
using UnityEngine;

namespace Sora.Tools.CSVLoader.Editor
{
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
            namelabelWidth = EditorGUIUtility.currentViewWidth * .1f;
            var itemWidth = ItemWidth();
            if (namelabelWidth > itemWidth)
            {
                itemWidth = namelabelWidth;
            }
            else namelabelWidth = itemWidth;
            namelabelWidth = Mathf.Clamp(namelabelWidth, 0, FieldMaxiWidth);
            itemWidth = Mathf.Clamp(itemWidth, 0, FieldMaxiWidth);
            width = property.FindPropertyRelative("width").intValue - 1;
            height = property.FindPropertyRelative("height").intValue - 1;
            rowNames = property.FindPropertyRelative("rowNames");
            columnNames = property.FindPropertyRelative("columnNames");
            var scrollViewFlag = DisplayScrollView(itemWidth);
            if (scrollViewFlag)
            {
                var viewRect = new Rect(position.x, position.y, itemWidth * width, height * EditorGUIUtility.singleLineHeight + scrollbarHeight);
                scrollPosition = GUI.BeginScrollView(position, scrollPosition, viewRect, true, false);
            }

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    if (x.Equals(0))
                    {
                        var columnNameLabelRect = new Rect(position.x, position.y + (y + 1) * EditorGUIUtility.singleLineHeight, itemWidth, EditorGUIUtility.singleLineHeight);
                        var columnNameProperty = columnNames.GetArrayElementAtIndex(y);
                        EditorGUI.LabelField(columnNameLabelRect, columnNameProperty.stringValue);
                    }
                    if (y.Equals(0))
                    {
                        var rowNameLabelRect = new Rect(position.x + (x + 1) * itemWidth, position.y, itemWidth, EditorGUIUtility.singleLineHeight);
                        var rowNameLabelProperty = rowNames.GetArrayElementAtIndex(x);
                        EditorGUI.LabelField(rowNameLabelRect, rowNameLabelProperty.stringValue);
                    }
                    var indexRect = new Rect(position.x + (x + 1) * itemWidth, position.y + (y + 1) * EditorGUIUtility.singleLineHeight, itemWidth, EditorGUIUtility.singleLineHeight);
                    var indexProperty = propertyValue.GetArrayElementAtIndex(x + width * y);
                    DrawPropertyArray2D(position, property, propertyValue, indexRect, indexProperty);
                }
            }
            if (scrollViewFlag)
            {
                GUI.EndScrollView();
            }
        }
        protected override bool DisplayScrollView(float itemWidth) => (itemWidth * (width + 1)) >= EditorGUIUtility.currentViewWidth;

        protected override float ItemWidth()
        {
            var itemWidth = (EditorGUIUtility.currentViewWidth - 20f) / (width + 1);
            return itemWidth < FieldMiniWidth ? FieldMiniWidth : itemWidth;
        }
    }
}