//Author: sora

using UnityEditor;
using UnityEngine;

namespace Sora.Tools.CSVLoader.Editor
{
    [CustomPropertyDrawer(typeof(IntKeyProperty))]
    public class IntKeyPropertyDrawer : BaseKeyPropertyDrawer
    {
        protected override void DrawProperty(Rect position, SerializedProperty property, SerializedProperty propertyValue)
        {
            propertyValue.intValue = EditorGUI.IntField(position, $"{property.name}[key]", propertyValue.intValue);
        }
    }
}