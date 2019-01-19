//Author: sora

using UnityEditor;
using UnityEngine;

namespace Sora.Tools.CSVLoader.Editor
{
    [CustomPropertyDrawer(typeof(IntProperty), true)]
    public class IntPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.BeginProperty(position, label, property);
            property.serializedObject.Update();
            property.FindPropertyRelative("propertyValue").intValue = EditorGUI.IntField(position, property.name, property.FindPropertyRelative("propertyValue").intValue);
            property.serializedObject.ApplyModifiedProperties();
            EditorGUI.EndProperty();
            EditorGUI.EndDisabledGroup();
        }
    }
}