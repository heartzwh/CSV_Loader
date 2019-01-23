//Author: sora

using UnityEditor;
using UnityEngine;

namespace Sora.Tools.CSVLoader.Editor
{
    [CustomPropertyDrawer(typeof(BooleanArray2DProperty), true)]
    public class BooleanArray2DPropertyDrawer : BaseArray2DPropertyDrawer
    {
        protected override void DrawPropertyArray2D(Rect position, SerializedProperty property, SerializedProperty propertyValue, Rect indexRect, SerializedProperty indexProperty)
        {
            indexProperty.boolValue = EditorGUI.Toggle(indexRect, indexProperty.boolValue);
        }
        protected override float ItemWidth() => 26f;
    }
}