//Author: sora

using UnityEditor;
using UnityEngine;

namespace Sora.Tools.CSVLoader.Editor
{
    [CustomPropertyDrawer(typeof(IntProperty))]
    public class IntPropertyDrawer : BasePropertyDrawer
    {
        protected override void DrawProperty(Rect position, SerializedProperty property, SerializedProperty propertyValue)
        {
            propertyValue.intValue = EditorGUI.IntField(position, property.name, propertyValue.intValue);
        }
    }
}