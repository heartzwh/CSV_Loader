//Author: sora

using System;

namespace Sora.Tools.CSVLoader
{
    [Serializable]
    public class FloatKeyProperty : BaseProperty<float>
    {
        #region constructor

        #endregion


        #region event/delegate

        #endregion


        #region property

        #endregion


        #region public method
        public override void SetPropertyValue(RawData value)
        {
            var valueStr = value[0, 0];
            propertyValue = string.IsNullOrEmpty(valueStr) ? 0f : Convert.ToSingle(valueStr);
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