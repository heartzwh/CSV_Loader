//Author: sora

using System;

namespace Sora.Tools.CSVLoader
{
    [Serializable]
    public class StringProperty : BaseProperty<string>
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
            propertyValue = Convert.ToString(value[0, 0]);
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