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
        public Type propertyType { get; protected set; }
        public Type propertyValueType { get; protected set; }

        public RawData propertyRawData { get; private set; }

        public string propertyContent { get; private set; }

        public string propertyName { get; private set; }
        
        public string[] propertySetting { get; private set; }
        /// <summary>
        /// 属性值
        /// </summary>
        /// <value></value>
        public TValue Value { get { return propertyValue; } }
        /// <summary>
        /// 属性值.
        /// Editor中调用
        /// </summary>
        public TValue propertyValue;
        #endregion


        #region public method
        public virtual void InitProperty(string[] propertySetting, RawData sourceData)
        {
            propertyType = this.GetType();
            propertyValueType = typeof(TValue);
            propertyRawData = sourceData;
            this.propertySetting = new string[propertySetting.Length];
            Array.Copy(propertySetting, this.propertySetting, propertySetting.Length);
            propertyName = propertySetting[1];
            propertyContent = $"public {this.GetType().FullName} {propertyName};";
        }
        public abstract void SetPropertyValue(RawData value);
        #endregion


        #region protected method

        #endregion


        #region private method

        #endregion


        #region static

        #endregion
    }
}