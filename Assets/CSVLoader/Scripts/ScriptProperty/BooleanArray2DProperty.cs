//Author: sora

using System;
using System.Collections.Generic;

namespace Sora.Tools.CSVLoader
{
    public class BooleanArray2DProperty : BaseArray2DProperty<bool[]>
    {
        #region constructor

        #endregion


        #region event/delegate

        #endregion


        #region property
        /// <summary>
        /// 只能在editor中使用
        /// </summary>
        public int width;
        /// <summary>
        /// 只能在editor中使用
        /// </summary>
        public int height;
        #endregion


        #region public method
        public override void SetPropertyValue(RawData value)
        {
            width = value.width;
            height = value.height;
            propertyValue = new bool[width * height];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    propertyValue[x + width * y] = Convert.ToBoolean(value[x, y]);
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