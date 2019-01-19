//Author: sora

using UnityEditor;
using UnityEngine;

namespace Sora.Tools.CSVLoader.Editor
{
    [CustomPropertyDrawer(typeof(IntProperty), true)]
    public class IntPropertyDrawer : PropertyDrawer
    {
        #region constructor

        #endregion


        #region event/delegate

        #endregion


        #region property
        #endregion


        #region public method
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            property.FindPropertyRelative("propertyValue").intValue = EditorGUI.IntField(position, property.name, property.FindPropertyRelative("propertyValue").intValue);
            EditorGUI.EndProperty();
        }

        #endregion


        #region protected method

        #endregion


        #region private method

        #endregion


        #region static

        #endregion
    }
}