//Author: sora

using System;
using System.Collections.Generic;

namespace Sora.Tools.CSVLoader
{
    /// <summary>
    /// 脚本包含的数据
    /// </summary>
    public class ScriptData
    {
        #region constructor
        /// <summary>
        /// 构建脚本
        /// </summary>
        /// <param name="rawData">csv中@start到@end中数据</param>
        public ScriptData(string rawData)
        {
            propertyList = new List<IProperty>();
        }

        #endregion


        #region event/delegate

        #endregion


        #region property
        /// <summary>
        /// 所有属性
        /// IProperty: 属性
        /// </summary>
        public List<IProperty> propertyList { get; private set; }
        /// <summary>
        /// 脚本所有内容
        /// </summary>
        /// <value></value>
        public string scriptContent { get; private set; }
        #endregion


        #region public method
        #endregion


        #region protected method

        #endregion


        #region private method

        #endregion


        #region static

        #endregion
    }
}