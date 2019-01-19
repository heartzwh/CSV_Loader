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
                propertyValue[index] = Convert.ToBoolean(value[index, 0]);
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