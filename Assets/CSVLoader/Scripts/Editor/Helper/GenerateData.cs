//Author: sora

using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Sora.Tools.CSVLoader
{
    /// <summary>
    /// 创建信息
    /// </summary>
    public class GenerateData
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
                fileInfo = new FileInfo(value);
                GenerateScript();
            }
        }
        /// <summary>
        /// 脚本文件位置(选择保存位置)
        /// </summary>
        public string scriptFilePath;
        /// <summary>
        /// 资源文件位置(选择保存位置)
        /// </summary>
        public string resourceFilePath;
        /// <summary>
        /// 创建顺序
        /// </summary>
        public readonly int createIndex;
        /// <summary>
        /// 是否折叠
        /// </summary>
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
        /// 脚本信息
        /// </summary>
        /// <value></value>
        public ScriptData scriptData { get; private set; }
        public FileInfo fileInfo { get; private set; }
        private string m_loadFilePath;

        public GenerateData(int createIndex)
        {
            this.createIndex = createIndex;
        }

        /// <summary>
        /// 构建脚本信息
        /// </summary>
        private void GenerateScript()
        {
            var fileStream = new FileStream(loadFilePath, FileMode.Open, FileAccess.Read);
            var streamReader = new StreamReader(fileStream);
            var line = "";
            var start = false;
            var rawPropertyData = new System.Text.StringBuilder();
            var rawScriptSetting = new List<string>();
            while ((line = streamReader.ReadLine()) != null)
            {
                var lineData = line.Split(SPLIT);
                if (!start)
                {
                    if (lineData[0].StartsWith(START_MARK))
                    {
                        start = true;
                        for (var i = 1; i < lineData.Length; i++) rawScriptSetting.Add(lineData[i]);
                    }
                    continue;
                }
                if (lineData[0].StartsWith(END_MARK)) break;
                rawPropertyData.AppendLine(line);
            }
            streamReader.Close();
            fileStream.Close();
            scriptData = new ScriptData(this, rawScriptSetting.ToArray(), rawPropertyData.ToString());
        }

        public const char SPLIT = ',';
        public const string START_MARK = "@start";
        public const string END_MARK = "@end";
        public const char SETTING_SPLIT = '#';
    }
}