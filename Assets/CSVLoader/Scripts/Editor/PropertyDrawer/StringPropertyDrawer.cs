//Author: sora

using UnityEditor;
using UnityEngine;

namespace Sora.Tools.CSVLoader.Editor
{
    [CustomPropertyDrawer(typeof(StringProperty))]
    public class StringPropertyDrawer : BasePropertyDrawer
    {
        protected override void DrawProperty(Rect position, SerializedProperty property, SerializedProperty propertyValue)
        {
            propertyValue.stringValue = EditorGUI.TextField(position, property.name, propertyValue.stringValue);
        }
    }
}