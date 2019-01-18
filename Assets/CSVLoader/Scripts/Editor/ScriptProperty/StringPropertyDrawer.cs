//Author: sora

using UnityEditor;
using UnityEngine;

namespace Sora.Tools.CSVLoader
{
    [CustomPropertyDrawer(typeof(StringProperty), true)]
    public class StringPropertyDrawer : PropertyDrawer
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
            property.FindPropertyRelative("propertyValue").stringValue = EditorGUI.TextField(position, property.name, property.FindPropertyRelative("propertyValue").stringValue);
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