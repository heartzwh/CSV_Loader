//Author: sora

using System;

namespace Sora.Tools.CSVLoader
{
    [System.Serializable]
    public class IntArray2DWithnameProperty : BaseArray2DWithnameProperty<int[]>
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
            propertyValue = new int[(width - 1) * (height - 1)];
            for (var y = 1; y < height; y++)
            {
                for (var x = 1; x < width; x++)
                {
                    var valueStr = value[x, y];
                    propertyValue[(x - 1) + (width - 1) * (y - 1)] = string.IsNullOrEmpty(valueStr) ? 0 : Convert.ToInt32(valueStr);
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