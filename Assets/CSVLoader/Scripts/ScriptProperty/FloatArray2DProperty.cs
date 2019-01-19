//Author: sora

using System;
using System.Collections.Generic;

namespace Sora.Tools.CSVLoader
{
    [System.Serializable]
    public class FloatArray2DProperty : BaseArray2DProperty<float[]>
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
            propertyValue = new float[width * height];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    propertyValue[x + width * y] = Convert.ToSingle(value[x, y]);
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