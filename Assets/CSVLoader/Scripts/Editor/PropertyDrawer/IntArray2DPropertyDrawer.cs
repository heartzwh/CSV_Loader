//Author: sora

using UnityEditor;
using UnityEngine;

namespace Sora.Tools.CSVLoader.Editor
{
    [CustomPropertyDrawer(typeof(IntArray2DProperty), true)]
    public class IntArray2DPropertyDrawer : BaseArray2DPropertyDrawer
    {
        protected override void DrawPropertyArray2D(Rect position, SerializedProperty property, SerializedProperty propertyValue, Rect indexRect, SerializedProperty indexProperty)
        {
            indexProperty.intValue = EditorGUI.IntField(indexRect, indexProperty.intValue);
        }
    }
}