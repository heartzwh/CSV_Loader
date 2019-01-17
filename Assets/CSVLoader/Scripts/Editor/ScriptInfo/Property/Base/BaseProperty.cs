//Author: sora

using System;
using UnityEngine;

namespace Sora.Tools.CSVLoader
{
    /// <summary>
    /// csv属性基类
    /// </summary>
    public abstract class BaseProperty : IProperty
    {
        #region constructor

        #endregion


        #region event/delegate

        #endregion


        #region property
        public Type type { get; protected set; }

        public RawData rawData { get; private set; }

        public string propertyContent { get; private set; }

        public string propertyName { get; private set; }
        #endregion


        #region public method
        public virtual void InitProperty(string[] propertySetting, RawData sourceData)
        {
            rawData = sourceData;
            propertyName = propertySetting[1];
            type = GetPropertyType();
            propertyContent = $"public {type.FullName} {propertyName};";
        }
        public abstract void DrawProperty(float x, float y, ref float width, ref float height);
        #endregion


        #region protected method
        protected abstract Type GetPropertyType();
        #endregion


        #region private method

        #endregion


        #region static

        #endregion
    }
}