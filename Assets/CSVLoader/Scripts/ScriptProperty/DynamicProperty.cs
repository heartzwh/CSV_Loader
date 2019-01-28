//Author: sora

using System;

namespace Sora.Tools.CSVLoader
{
    public class DynamicProperty : IProperty
    {
        #region constructor

        #endregion


        #region event/delegate

        #endregion


        #region property
        public Type propertyType { get { return Type.GetType(dynamicTypeFullname); } }
        public Type propertyValueType { get { return Type.GetType(dynamicTypeFullname); } }

        public RawData propertyRawData { get; private set; }

        public string propertyContent { get; private set; }

        public string propertyName { get; private set; }

        public string[] propertySetting { get; private set; }

        /// <summary>
        /// 类型全名
        /// </summary>
        /// <value></value>
        public string dynamicTypeName { get; private set; }
        /// <summary>
        /// 类型全名
        /// </summary>
        /// <value></value>
        public string dynamicTypeFullname { get; private set; }
        /// <summary>
        /// 绑定Key
        /// 绑定Key为了在对应的类中查找与该值相同的属性名,用该属性名中作为使用数据的依据
        /// </summary>
        /// <value></value>
        public string bindinKey { get; private set; }
        #endregion


        #region public method
        public void InitProperty(string[] propertySetting, RawData sourceData)
        {
            dynamicTypeFullname = propertySetting[0].Replace("@", "");
            dynamicTypeName = dynamicTypeFullname.Substring(dynamicTypeFullname.LastIndexOf('.') + 1);
            propertyRawData = sourceData;
            this.propertySetting = new string[propertySetting.Length];
            Array.Copy(propertySetting, this.propertySetting, propertySetting.Length);
            propertyName = propertySetting[1];
            propertyContent = $"public {dynamicTypeFullname} {propertyName};";
            bindinKey = propertySetting[2];
        }
        public void SetPropertyValue(RawData value)
        {

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