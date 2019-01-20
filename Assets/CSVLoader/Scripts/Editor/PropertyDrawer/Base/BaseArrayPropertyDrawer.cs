//Author: sora

using UnityEditor;
using UnityEngine;

namespace Sora.Tools.CSVLoader.Editor
{
    [CustomPropertyDrawer(typeof(IProperty), true)]
    public abstract class BaseArrayPropertyDrawer : BasePropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var foldout = property.FindPropertyRelative("foldout");
            var propertyValue = property.FindPropertyRelative("propertyValue");
            if (foldout.boolValue)
            {
                return (property.FindPropertyRelative("propertyValue").arraySize + 1) * EditorGUIUtility.singleLineHeight;
            }
            return base.GetPropertyHeight(property, label);
        }

        protected override void DrawProperty(Rect position, SerializedProperty property, SerializedProperty propertyValue)
        {
            var foldout = property.FindPropertyRelative("foldout");
            var foldoudRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            foldout.boolValue = EditorGUI.Foldout(foldoudRect, foldout.boolValue, $"{property.name} [{propertyValue.arraySize}]", true);
            if (foldout.boolValue)
            {
                EditorGUI.indentLevel++;
                for (var index = 0; index < propertyValue.arraySize; index++)
                {
                    var indexRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * (index + 1), position.width, EditorGUIUtility.singleLineHeight);
                    var indexProperty = propertyValue.GetArrayElementAtIndex(index);
                    DrawPropertyArray(position, property, propertyValue, indexProperty, indexRect, $"[{index}]");
                }
                EditorGUI.indentLevel--;
            }
        }

        protected abstract void DrawPropertyArray(Rect position, SerializedProperty property, SerializedProperty propertyValue, SerializedProperty indexProperty, Rect indexRect, string indexTitle);
    }
}