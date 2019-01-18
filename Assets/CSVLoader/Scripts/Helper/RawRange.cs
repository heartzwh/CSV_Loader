//Author: sora

namespace Sora.Tools.CSVLoader
{
    /// <summary>
    /// csv数据范围
    /// </summary>
    public struct RawRange
    {
        /// <summary>
        /// 起始行
        /// </summary>
        public int beginRow;
        /// <summary>
        /// 起始列
        /// </summary>
        public int beginColumn;
        /// <summary>
        /// 高度(行数量)
        /// </summary>
        public int height;
        /// <summary>
        /// 宽度(列数量)
        /// </summary>
        public int width;
        public RawRange(int beginRow, int beginColumn, int width, int height)
        {
            this.beginRow = beginRow;
            this.beginColumn = beginColumn;
            this.width = width;
            this.height = height;
        }
        public override string ToString()
        {
            return $"beginRow: {beginRow}\n" +
                $"beginColumn: {beginColumn}\n" +
                $"width: {width}\n" +
                $"height: {height}";
        }
    }
}