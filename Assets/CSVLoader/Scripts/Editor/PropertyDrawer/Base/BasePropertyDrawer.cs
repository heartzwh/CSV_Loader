//Author: sora

using UnityEditor;
using UnityEngine;

namespace Sora.Tools.CSVLoader.Editor
{
    public abstract class BasePropertyDrawer : PropertyDrawer
    {
        #region constructor

        #endregion


        #region event/delegate

        #endregion


        #region property
        protected const float FieldMaxiWidth = 80;
        protected const float FieldMiniWidth = 26f;
        protected SerializedProperty propertyValue { get; private set; }
        protected SerializedProperty property { get; private set; }
        #endregion


        #region public method
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            var toggleRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            // EditorGUI.BeginDisabledGroup(true);
            property.serializedObject.Update();
            this.property = property;
            propertyValue = property.FindPropertyRelative("propertyValue");
            DrawProperty(position, property, propertyValue);
            property.serializedObject.ApplyModifiedProperties();
            // EditorGUI.EndDisabledGroup();
            EditorGUI.EndProperty();
        }
        #endregion


        #region protected method
        protected abstract void DrawProperty(Rect position, SerializedProperty property, SerializedProperty propertyValue);

        #endregion


        #region private method

        #endregion


        #region static

        #endregion
    }
}