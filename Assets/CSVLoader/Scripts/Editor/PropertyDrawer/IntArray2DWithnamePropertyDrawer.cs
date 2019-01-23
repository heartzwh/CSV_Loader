//Author: sora

using UnityEditor;
using UnityEngine;

namespace Sora.Tools.CSVLoader.Editor
{
    [CustomPropertyDrawer(typeof(IntArray2DWithnameProperty), true)]
    public class IntArray2DWithnamePropertyDrawer : BaseArray2DWithnamePropertyDrawer
    {
        protected override void DrawPropertyArray2D(Rect position, SerializedProperty property, SerializedProperty propertyValue, Rect indexRect, SerializedProperty indexProperty)
        {
            indexProperty.intValue = EditorGUI.IntField(indexRect, indexProperty.intValue);
        }
    }
}