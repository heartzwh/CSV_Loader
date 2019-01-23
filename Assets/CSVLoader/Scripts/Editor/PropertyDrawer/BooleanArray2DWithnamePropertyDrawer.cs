//Author: sora

using UnityEditor;
using UnityEngine;

namespace Sora.Tools.CSVLoader.Editor
{
    [CustomPropertyDrawer(typeof(BooleanArray2DWithnameProperty), true)]
    public class BooleanArray2DWithnamePropertyDrawer : BaseArray2DWithnamePropertyDrawer
    {
        protected override void DrawPropertyArray2D(Rect position, SerializedProperty property, SerializedProperty propertyValue, Rect indexRect, SerializedProperty indexProperty)
        {
            indexProperty.boolValue = EditorGUI.Toggle(indexRect, indexProperty.boolValue);
        }
        protected override float ItemWidth() => 26f;
    }
}