//Author: sora

using System;

namespace Sora.Tools.CSVLoader.Editor
{
    [System.Serializable]
    public class IntArray2DProperty : BaseArray2DProperty<int[]>
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
            propertyValue = new int[width * height];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var valueStr = value[x, y];
                    propertyValue[x + width * y] = string.IsNullOrEmpty(valueStr) ? 0 : Convert.ToInt32(valueStr);
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