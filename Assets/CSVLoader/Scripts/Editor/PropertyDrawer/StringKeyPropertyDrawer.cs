//Author: sora

using UnityEditor;
using UnityEngine;

namespace Sora.Tools.CSVLoader.Editor
{
    [CustomPropertyDrawer(typeof(StringKeyProperty))]
    public class StringKeyPropertyDrawer : BaseKeyPropertyDrawer
    {
        protected override void DrawProperty(Rect position, SerializedProperty property, SerializedProperty propertyValue)
        {
            propertyValue.stringValue = EditorGUI.TextField(position, $"{property.name}[key]", propertyValue.stringValue);
        }
    }
}