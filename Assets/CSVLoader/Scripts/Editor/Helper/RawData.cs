//Author: sora

using System.Collections.Generic;

namespace Sora.Tools.CSVLoader
{
    /// <summary>
    /// 原始数据
    /// </summary>
    public class RawData
    {
        #region constructor
		/// <summary>
		/// 初始化数据
		/// </summary>
		/// <param name="sourceData">csv表数据</param>
        public RawData(string sourceData)
        {
        }
        #endregion


        #region event/delegate

        #endregion


        #region property
        private string[,] rowData;
        public string this[int x, int y]
        {
            get
            {
                try
                {
                    return rowData[x, y];
                }
                catch
                {
                    throw new System.Exception($"超过范围.x=>{rowData.GetLength(0)},y=>{rowData.GetLength(1)}");
                }
            }
        }
        #endregion

        #region public method

        #endregion


        #region protected method

        #endregion


        #region private method

        #endregion


        #region static

        #endregion
    }
}