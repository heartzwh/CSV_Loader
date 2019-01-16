//Author: sora

using System;

namespace Sora.Tools.CSVLoader
{
    /// <summary>
    /// csv属性
    /// </summary>
    public interface IProperty
    {
        /// <summary>
        /// 属性类型
        /// </summary>
        /// <value></value>
        Type type { get; }
        /// <summary>
        /// 属性原始数据
        /// </summary>
        /// <value></value>
        RawData rawData { get; }
		/// <summary>
		/// 属性内容
		/// 比如:
		/// public string name;
		/// </summary>
		/// <value></value>
        string propertyContent { get; }
        /// <summary>
        /// 初始化属性
        /// </summary>
        /// <param name="sourceData">csv表数据</param>
        void InitProperty(string sourceData);
    }

}