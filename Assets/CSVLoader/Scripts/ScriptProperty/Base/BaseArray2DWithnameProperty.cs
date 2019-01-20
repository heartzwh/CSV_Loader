//Author: sora

namespace Sora.Tools.CSVLoader
{
    public abstract class BaseArray2DWithnameProperty<TValue> : BaseArray2DProperty<TValue>
    {
        #region constructor

        #endregion


        #region event/delegate

        #endregion


        #region property
        /// <summary>
        /// 横排名称
        /// Editor调用
        /// </summary>
        public string[] rowNames;
        /// <summary>
        /// 竖排名称
        /// Editor调用
        /// </summary>
        public string[] columnNames;
        #endregion


        #region public method
        public override void SetPropertyValue(RawData value)
        {
            base.SetPropertyValue(value);
            rowNames = new string[width];
            columnNames = new string[height];
            for (var x = 1; x < width; x++)
            {
                rowNames[x] = value[x, 0];
            }
            for (var y = 1; y < height; y++)
            {
                columnNames[y] = value[0, y];
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