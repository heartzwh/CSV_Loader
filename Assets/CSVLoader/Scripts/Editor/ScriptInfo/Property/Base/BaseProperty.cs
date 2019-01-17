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

        #endregion


        #region public method
        public virtual void InitProperty(RawData sourceData)
        {
            /* sourceData 包含该属性所有值 */
            rawData = sourceData;
            /* [0,0]填写了该属性的定义 */
            type = GetPropertyType(rawData[0, 0]);
        }
        public abstract void DrawProperty(float x, float y, ref float width, ref float height);
        #endregion


        #region protected method
        /// <summary>
        /// 获取属性类型
        /// </summary>
        /// <param name="rowData">原始数据</param>
        /// <returns></returns>
        protected abstract Type GetPropertyType(string rowData);
        #endregion


        #region private method

        #endregion


        #region static

        #endregion
    }
}