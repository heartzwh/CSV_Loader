//Author: sora

using UnityEditor;
using UnityEngine;

namespace Sora.Tools.CSVLoader.Editor
{
    [CustomPropertyDrawer(typeof(FloatArrayProperty), true)]
    public class FloatArrayPropertyDrawer : PropertyDrawer
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

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.BeginProperty(position, label, property);
            property.serializedObject.Update();
            var foldout = property.FindPropertyRelative("foldout");
            var propertyValue = property.FindPropertyRelative("propertyValue");
            var foldoudRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            foldout.boolValue = EditorGUI.Foldout(foldoudRect, foldout.boolValue, $"{property.name} [{propertyValue.arraySize}]", true);
            if (foldout.boolValue)
            {
                EditorGUI.indentLevel++;
                for (var index = 0; index < propertyValue.arraySize; index++)
                {
                    var indexRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * (index + 1), position.width, EditorGUIUtility.singleLineHeight);
                    var indexProperty = propertyValue.GetArrayElementAtIndex(index);
                    indexProperty.floatValue = EditorGUI.FloatField(indexRect, $"[{index}]", indexProperty.floatValue);
                }
                EditorGUI.indentLevel--;
            }
            property.serializedObject.ApplyModifiedProperties();
            EditorGUI.EndProperty();
            EditorGUI.EndDisabledGroup();
        }
    }
}