//Author: sora

using System;
using UnityEngine;

namespace Sora.Tools.CSVLoader
{
    public interface IProperty
    {
        /// <summary>
        /// 属性类型
        /// </summary>
        /// <value></value>
        Type type { get; }
        /// <summary>
        /// 属性名称
        /// </summary>
        /// <value></value>
        string propertyName { get; }
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
        /// <param name="propertySetting">属性设置</param>
        /// <param name="sourceData">csv表数据</param>
        void InitProperty(string[] propertySetting, RawData sourceData);
    }
}