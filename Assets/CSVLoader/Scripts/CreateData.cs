//Author: sora

namespace Sora.Tools.CSVLoader
{
    /// <summary>
    /// 创建的类型
    /// </summary>
    public enum Createype
    {
        /// <summary>
        /// 加载类型 
        /// <summary>
        CLASS,
    }


    /// <summary>
    /// 创建信息
    /// </summary>
    public class CreateData
    {
        /// <summary>
        /// 文件位置(csv文件位置)
        /// </summary>
        public string loadFilePath;
        /// <summary>
        /// 创建类型
        /// </summary>
        public Createype createType;
        /// <summary>
        /// 脚本文件位置(选择保存位置)
        /// </summary>
        public string scriptFilePath;
        /// <summary>
        /// 资源文件位置(选择保存位置)
        /// </summary>
        public string resourceFilePath;
        public int createIndex;
        public CreateData(int createIndex)
        {
            this.createIndex = createIndex;
        }
    }
}