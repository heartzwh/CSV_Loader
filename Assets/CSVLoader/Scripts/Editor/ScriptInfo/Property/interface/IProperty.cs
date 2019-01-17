//Author: sora

using System;
using UnityEngine;

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
        /// <summary>
        /// editor 显示
        /// </summary>
        /// <param name="x">开始X位置</param>
        /// <param name="y">开始Y位置</param>
        /// <param name="width">占用的宽度</param>
        /// <param name="height">占用的高度</param>
        void DrawProperty(float x, float y, ref float width, ref float height);
    }

}