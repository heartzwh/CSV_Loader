//Author: sora

using UnityEditor;
using UnityEngine;

namespace Sora.Tools.CSVLoader.Editor
{
    [CustomPropertyDrawer(typeof(FloatKeyProperty))]
    public class FloatKeyPropertyDrawer : BaseKeyPropertyDrawer
    {
        protected override void DrawProperty(Rect position, SerializedProperty property, SerializedProperty propertyValue)
        {
            propertyValue.floatValue = EditorGUI.FloatField(position, $"{property.name}[key]", propertyValue.floatValue);
        }
    }
}