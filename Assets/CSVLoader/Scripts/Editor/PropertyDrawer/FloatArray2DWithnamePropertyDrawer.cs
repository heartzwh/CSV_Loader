//Author: sora

using UnityEditor;
using UnityEngine;

namespace Sora.Tools.CSVLoader.Editor
{
    [CustomPropertyDrawer(typeof(FloatArray2DWithnameProperty), true)]
	public class FloatArray2DWithnamePropertyDrawer : BaseArray2DWithnamePropertyDrawer
	{
        protected override void DrawPropertyArray2D(Rect position, SerializedProperty property, SerializedProperty propertyValue, Rect indexRect, SerializedProperty indexProperty)
        {
            indexProperty.floatValue = EditorGUI.FloatField(indexRect, indexProperty.floatValue);
        }
	}
}