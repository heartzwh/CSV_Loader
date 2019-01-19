//Author: sora

using System;
using UnityEngine;

namespace Sora.Tools.CSVLoader
{
    [Serializable]
    public class IntProperty : BaseProperty<int>
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
            propertyValue = Convert.ToInt32(value[0, 0]);
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