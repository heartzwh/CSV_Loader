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
        public override void SetPropertyValue(string value)
        {
            propertyValue = Convert.ToString(value);
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