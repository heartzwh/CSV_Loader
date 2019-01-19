//Author: sora

using System;

namespace Sora.Tools.CSVLoader
{
    [Serializable]
    public class BooleanProperty : BaseProperty<bool>
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
            propertyValue = Convert.ToBoolean(value);
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