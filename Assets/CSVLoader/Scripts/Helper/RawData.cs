//Author: sora

using System.Collections.Generic;
using System.Linq;

namespace Sora.Tools.CSVLoader
{
    /// <summary>
    /// 原始数据
    /// </summary>
    public class RawData
    {
        #region constructor
        public RawData() { }
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="sourceData">csv表数据</param>
        public RawData(string sourceData)
        {
            /* windows/mac分隔符不一样,如果数据解析出错,查看是否没有添加对应编辑器的分隔符 */
            var lineData = sourceData.Split(new string[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.None);
            height = lineData.Length;
            if (string.IsNullOrWhiteSpace(lineData[lineData.Length - 1])) height--;
            foreach (var line in lineData)
            {
                /* 找到最宽的数据作为宽度 */
                var currentWidth = line.Count(c => c.Equals(Helper.SPLIT)) + 1;
                if (currentWidth > width)
                {
                    width = currentWidth;
                }
            }
            rowData = new string[width, height];
            for (var y = 0; y < height; y++)
            {
                var line = lineData[y].Split(Helper.SPLIT);
                for (var x = 0; x < width; x++)
                {
                    rowData[x, y] = line[x];
                }
            }
        }
        public RawData(string[,] sourceData)
        {
            width = sourceData.GetLength(0);
            height = sourceData.GetLength(1);
            rowData = new string[width, height];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    rowData[x, y] = sourceData[x, y];
                }
            }
        }
        #endregion


        #region event/delegate

        #endregion


        #region property
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
                    throw new System.Exception($"\"[{x},{y}]\"超过范围.width: {rowData.GetLength(0)},height: {rowData.GetLength(1)}");
                }
            }
        }
        /// <summary>
        /// 数据宽度
        /// </summary>
        /// <returns></returns>
        public int width { get; private set; }
        /// <summary>
        /// 数据高度
        /// </summary>
        /// <returns></returns>
        public int height { get; private set; }
        private string[,] rowData;
        #endregion

        #region public method
        /// <summary>
        /// 获取某行的数据
        /// </summary>
        /// <param name="y">行数</param>
        /// <returns></returns>
        public string[] GetRow(int y)
        {
            if (y > height) throw new System.Exception($"\"{y}\"超过了高度");
            var data = new string[width];
            for (var index = 0; index < width; index++)
            {
                data[index] = rowData[index, y];
            }
            return data;
        }
        /// <summary>
        /// 获取某列数据
        /// </summary>
        /// <param name="x">列数</param>
        /// <returns></returns>
        public string[] GetColumn(int x)
        {
            if (x > width) throw new System.Exception($"\"{x}\"超过了宽度");
            var data = new string[height];
            for (var index = 0; index < height; index++)
            {
                data[index] = rowData[x, index];
            }
            return data;
        }
        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            for (var y = 0; y < rowData.GetLength(1); y++)
            {
                for (var x = 0; x < rowData.GetLength(0); x++)
                {
                    sb.Append(rowData[x, y]);
                    if (x < rowData.GetLength(0) - 1) sb.Append(" ");
                }
                if (y < rowData.GetLength(1) - 1) sb.Append("\n");
            }
            return sb.ToString();
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