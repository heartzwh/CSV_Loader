//Author: sora

using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Sora.Tools.CSVLoader
{
    /// <summary>
    /// 创建的类型
    /// </summary>
    public enum CreateType
    {
        /// <summary>
        /// 默认类型,创建一个类
        /// <summary>
        DEFAULT,
        /// <summary>
        /// 2D数组
        /// <summary>
        ARRAY_2D,
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
        public CreateType createType;
        /// <summary>
        /// 脚本文件位置(选择保存位置)
        /// </summary>
        public string scriptFilePath;
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
            var fileStream = new FileStream(loadFilePath, FileMode.Open, FileAccess.Read);
            var streamReader = new StreamReader(fileStream);
            var line = "";
            var lineIndex = 0;
            var start = false;
            var dataSet = new Dictionary<int, List<string>>();
            while ((line = streamReader.ReadLine()) != null)
            {
                var lineData = line.Split(SPLIT);
                if (!start && lineData[0].StartsWith(StartMark))
                {
                    start = true;
                    if (lineData.Length < 3) SetCreateType("");
                    else SetCreateType(lineData[2]);
                }
                if (!start) continue;
                if (lineData[0].StartsWith(EndMark)) break;
                var dataList = new List<string>();
                foreach (var data in lineData)
                {
                    dataList.Add(data);
                }
                dataSet.Add(lineIndex, dataList);
                lineIndex++;
            }

            if (start)
            {
                scriptContent = CreateFileManager.Instance.GenerateScriptContent(createType, dataSet);
                Debug.Log(scriptContent);
            }
        }
        /// <summary>
        /// 设置创建的脚本类型
        /// </summary>
        /// <param name="data"></param>
        private void SetCreateType(string data)
        {
            data = data.ToLower();
            switch (data)
            {
                case "":
                    createType = CreateType.DEFAULT;
                    break;
                case "array_2d":
                    createType = CreateType.ARRAY_2D;
                    break;
            }
        }

        public const char SPLIT = ',';
        public const string StartMark = "@start";
        public const string EndMark = "@end";
    }
}