//Author: sora

using System;
using UnityEngine;

namespace Sora.Tools.CSVLoader
{
    /// <summary>
    /// csv属性基类
    /// </summary>
    public abstract class BaseProperty<TValue> : IProperty
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
        /// <summary>
        /// 属性值
        /// </summary>
        public TValue propertyValue;
        #endregion


        #region public method
        public virtual void InitProperty(string[] propertySetting, RawData sourceData)
        {
            type = typeof(TValue);
            rawData = sourceData;
            propertyName = propertySetting[1];
            propertyContent = $"public {this.GetType().FullName} {propertyName};";
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