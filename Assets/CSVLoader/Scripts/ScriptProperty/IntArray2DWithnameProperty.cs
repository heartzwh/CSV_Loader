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
            /* 去除name行列 */
            width -= 1;
            height -= 1;
            propertyValue = new int[width * height];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    /* [0]被name行列占据 */
                    var valueStr = value[x + 1, y + 1];
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