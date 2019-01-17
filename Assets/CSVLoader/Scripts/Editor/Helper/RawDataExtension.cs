//Author: sora

namespace Sora.Tools.CSVLoader
{
    public static class RawDataExtension
    {
        /// <summary>
        /// 获取范围内的数据
        /// </summary>
        /// <param name="self"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public static RawData GetRangeRawData(this RawData self, RawRange range)
        {
            if (range.width > self.width || (range.beginColumn + range.width) > self.width) throw new System.Exception($"\"{range.width}\"超过\"{self.width}\"");
            if (range.height > self.height || (range.beginRow + range.height) > self.height) throw new System.Exception($"\"{range.height}\"超过\"{self.height}\"");
            var data = new string[range.width, range.height];
            var columnIndex = 0;
            var rowIndex = 0;
            for (var y = range.beginRow; y < range.beginRow + range.height; y++)
            {
                columnIndex = 0;
                for (var x = range.beginColumn; x < range.beginColumn + range.width; x++)
                {
                    data[columnIndex, rowIndex] = self[x, y];
                    columnIndex++;
                }
                rowIndex++;
            }
            return new RawData(data);
        }
    }
}