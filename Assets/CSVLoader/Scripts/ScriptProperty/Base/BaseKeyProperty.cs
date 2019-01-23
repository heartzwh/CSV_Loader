//Author: sora

namespace Sora.Tools.CSVLoader
{
    public abstract class BaseKeyProperty<TValue> : BaseProperty<TValue>, IDictionaryKey
    {
        #region constructor

        #endregion


        #region event/delegate

        #endregion


        #region property
        public bool keyFlag { get; private set; }

        #endregion


        #region public method
        public override void InitProperty(string[] propertySetting, RawData sourceData)
        {
            base.InitProperty(propertySetting, sourceData);
            /* 最后一个属性的标志为 [key],则该属性为字典的 key */
            keyFlag = propertySetting[propertySetting.Length - 1].ToLower().Equals("key");
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