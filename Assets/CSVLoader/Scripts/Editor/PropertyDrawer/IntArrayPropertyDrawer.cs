//Author: sora

using UnityEditor;
using UnityEngine;

namespace Sora.Tools.CSVLoader.Editor
{
    [CustomPropertyDrawer(typeof(IntArrayProperty), true)]
    public class IntArrayPropertyDrawer : BaseArrayPropertyDrawer
    {
        protected override void DrawPropertyArray(Rect position, SerializedProperty property, SerializedProperty propertyValue, SerializedProperty indexProperty, Rect indexRect, string indexTitle)
        {
            indexProperty.intValue = EditorGUI.IntField(indexRect, indexTitle, indexProperty.intValue);
        }
    }
}