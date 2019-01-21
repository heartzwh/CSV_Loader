//Author: sora

using System;

namespace Sora.Tools.CSVLoader
{
    [Serializable]
    public class BooleanArrayProperty : BaseArrayProperty<bool[]>
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
            propertyValue = new bool[value.width];
            for (var index = 0; index < value.width; index++)
            {
                var valueStr = value[index, 0];
                propertyValue[index] = string.IsNullOrEmpty(valueStr) ? false : Convert.ToBoolean(valueStr);
            }
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