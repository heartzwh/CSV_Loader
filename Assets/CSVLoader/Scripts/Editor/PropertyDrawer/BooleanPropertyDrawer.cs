//Author: sora

using UnityEditor;
using UnityEngine;

namespace Sora.Tools.CSVLoader.Editor
{
    [CustomPropertyDrawer(typeof(BooleanProperty))]
    public class BooleanPropertyDrawer : BasePropertyDrawer
    {
        protected override void DrawProperty(Rect position, SerializedProperty property, SerializedProperty propertyValue)
        {
            propertyValue.boolValue = EditorGUI.Toggle(position, property.name, propertyValue.boolValue);
        }
    }
}