//Author: sora

namespace Sora.Tools.CSVLoader
{
    public abstract class BaseArray2DProperty<TValue> : BaseProperty<TValue>
    {
        #region constructor

        #endregion


        #region event/delegate

        #endregion


        #region property
        /// <summary>
        /// 数据宽度
        /// </summary>
        public int width;
        /// <summary>
        /// 数据高度
        /// </summary>
        public int height;
        #endregion


        #region public method
        public override void SetPropertyValue(RawData value)
        {
            width = value.width;
            height = value.height;
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