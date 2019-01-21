//Author: sora

using System;
using System.Collections.Generic;

namespace Sora.Tools.CSVLoader
{
    [System.Serializable]
    public class BooleanArray2DProperty : BaseArray2DProperty<bool[]>
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
            base.SetPropertyValue(value);
            propertyValue = new bool[width * height];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var valueStr = value[x, y];
                    propertyValue[x + width * y] = string.IsNullOrEmpty(valueStr) ? false : Convert.ToBoolean(valueStr);
                }
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