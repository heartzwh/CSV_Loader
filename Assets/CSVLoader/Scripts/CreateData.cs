//Author: sora

using System.IO;
using UnityEngine;

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
        public string loadFilePath
        {
            get => m_loadFilePath;
            set
            {
                m_loadFilePath = value;
                GenerateScriptContent();
            }
        }
        /// <summary>
        /// 创建类型
        /// </summary>
        public Createype createType;
        /// <summary>
        /// 脚本文件位置(选择保存位置)
        /// </summary>
        public string scriptFilePath;
        /// <summary>
        /// 脚本命名空间
        /// </summary>
        public string scriptNameSpace;
        /// <summary>
        /// 资源文件位置(选择保存位置)
        /// </summary>
        public string resourceFilePath;
        public int createIndex;
        public bool foldout;
        /// <summary>
        /// 资源准备就绪,可生成文件了
        /// </summary>
        public bool prepareComplete
        {
            get
            {
                return !string.IsNullOrEmpty(scriptFilePath) && !string.IsNullOrEmpty(resourceFilePath);
            }
        }
        /// <summary>
        /// 脚本代码内容
        /// </summary>
        public string scriptContent;
        private string m_loadFilePath;
        public CreateData(int createIndex)
        {
            this.createIndex = createIndex;
        }
        private void GenerateScriptContent()
        {
            初始化scriptContent
            
            var fileStream = new FileStream(loadFilePath, FileMode.Open, FileAccess.Read);
            var streamReader = new StreamReader(fileStream);
            var line = "";
            while ((line = streamReader.ReadLine()) != null)
            {
                Debug.Log(line);
            }
        }
    }
}