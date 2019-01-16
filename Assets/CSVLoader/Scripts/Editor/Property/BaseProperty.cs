//Author: sora

using System;

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
        public virtual void InitProperty(string sourceData)
        {
            /* sourceData 包含该属性所有值 */
            rawData = new RawData(sourceData);
            /* [0,0]填写了该属性的定义 */
            type = GetPropertyType(rawData[0, 0]);
        }
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