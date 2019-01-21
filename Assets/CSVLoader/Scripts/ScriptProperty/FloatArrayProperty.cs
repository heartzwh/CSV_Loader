//Author: sora

using System;

namespace Sora.Tools.CSVLoader
{
    [Serializable]
    public class FloatArrayProperty : BaseArrayProperty<float[]>
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
            if (propertyValue == null) propertyValue = new float[value.width];
            for (var index = 0; index < value.width; index++)
            {
                var valueStr = value[index, 0];
                propertyValue[index] = string.IsNullOrEmpty(valueStr) ? 0f : Convert.ToSingle(valueStr);
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