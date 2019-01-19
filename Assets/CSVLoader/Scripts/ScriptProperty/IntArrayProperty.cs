//Author: sora

using System;

namespace Sora.Tools.CSVLoader
{
    [Serializable]
    public class IntArrayProperty : BaseArrayProperty<int[]>
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
            if (propertyValue == null) propertyValue = new int[value.width];
            for (var index = 0; index < value.width; index++)
            {
                propertyValue[index] = Convert.ToInt32(value[index, 0]);
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