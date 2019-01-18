//Author: sora

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
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
        public ScriptData scriptData;
        /// <summary>
        /// 脚本资源信息(ScriptObject)
        /// </summary>
        /// <value></value>
        public ScriptAssetData scriptAssetData;
        public FileInfo fileInfo { get; private set; }
        /// <summary>
        /// 块颜色
        /// </summary>
        public Color blockColor = NORMALR_COLOR;
        /// <summary>
        /// 块高度
        /// </summary>
        public float blockHeight;
        public ScriptSetting scriptSetting;
        private string m_loadFilePath;
        public GenerateData(int createIndex)
        {
            this.createIndex = createIndex;
        }
        /// <summary>
        /// 清除块颜色
        /// </summary>
        /// <param name="delayTime"></param>
        /// <returns></returns>
        public async void ClearColor(float delayTime = 1.5f)
        {
            await Task.Yield();
            await Task.Delay(System.TimeSpan.FromSeconds(delayTime));
            blockColor = NORMALR_COLOR;
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
                var lineData = line.Split(Helper.SPLIT);
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
            scriptSetting = new ScriptSetting(this, rawScriptSetting.ToArray());
            scriptData = new ScriptData(this, rawPropertyData.ToString());
            scriptAssetData = new ScriptAssetData(this);
        }

        public const string START_MARK = "@start";
        public const string END_MARK = "@end";
        public static Color ERROR_COLOR = new Color(255, 0, 0, 0.5f);
        public static Color NORMALR_COLOR = Color.clear;
    }
}