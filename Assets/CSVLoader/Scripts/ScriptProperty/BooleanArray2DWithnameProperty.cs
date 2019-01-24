//Author: sora

using System;

namespace Sora.Tools.CSVLoader
{
    [System.Serializable]
    public class BooleanArray2DWithnameProperty : BaseArray2DWithnameProperty<bool[]>
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
            propertyValue = new bool[(width - 1) * (height - 1)];
            for (var y = 1; y < height; y++)
            {
                for (var x = 1; x < width; x++)
                {
                    var valueStr = value[x, y];
                    propertyValue[(x - 1) + (width - 1) * (y - 1)] = string.IsNullOrEmpty(valueStr) ? false : Convert.ToBoolean(valueStr);
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