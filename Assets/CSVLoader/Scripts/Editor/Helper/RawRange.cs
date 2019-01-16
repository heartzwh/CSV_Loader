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
	}
}