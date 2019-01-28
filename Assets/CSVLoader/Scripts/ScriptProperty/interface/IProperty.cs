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
        Type propertyType { get; }
        /// <summary>
        /// 属性值类型
        /// </summary>
        /// <value></value>
        Type propertyValueType { get; }
        /// <summary>
        /// 属性名称
        /// </summary>
        /// <value></value>
        string propertyName { get; }
        /// <summary>
        /// 属性原始数据
        /// </summary>
        /// <value></value>
        RawData propertyRawData { get; }
        /// <summary>
        /// 属性内容
        /// 比如:
        /// public string name;
        /// </summary>
        /// <value></value>
        string propertyContent { get; }
        /// <summary>
        /// 属性设置
        /// </summary>
        /// <value></value>
        string[] propertySetting { get; }
        /// <summary>
        /// 初始化属性
        /// </summary>
        /// <param name="propertySetting">属性设置</param>
        /// <param name="sourceData">csv表数据</param>
        void InitProperty(string[] propertySetting, RawData sourceData);
        /// <summary>
        /// IScriptAsset => 设置当前属性值
        /// </summary>
        /// <param name="value"></param>
        void SetPropertyValue(RawData value);
    }
}