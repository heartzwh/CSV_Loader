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
        public string key { get; private set; }
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
            key = propertySetting[2];
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