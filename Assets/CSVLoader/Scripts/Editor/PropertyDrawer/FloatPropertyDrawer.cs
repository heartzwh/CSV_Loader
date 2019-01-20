//Author: sora

using UnityEditor;
using UnityEngine;

namespace Sora.Tools.CSVLoader.Editor
{
    [CustomPropertyDrawer(typeof(FloatProperty))]
    public class FloatPropertyDrawer : BasePropertyDrawer
    {
        protected override void DrawProperty(Rect position, SerializedProperty property, SerializedProperty propertyValue)
        {
            propertyValue.floatValue = EditorGUI.FloatField(position, property.name, propertyValue.floatValue);
        }
    }
}