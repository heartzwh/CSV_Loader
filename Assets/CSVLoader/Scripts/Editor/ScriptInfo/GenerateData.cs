//Author: sora

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Sora.Tools.CSVLoader.Editor
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
                csvFileInfo = new FileInfo(value);
                GenerateScript();
            }
        }
        /// <summary>
        /// 资源文件位置(选择保存位置)
        /// </summary>
        public string resourceFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(m_resourceFilePath))
                {
                    /* 未设置资源位置,则使用默认位置 */
                    if (string.IsNullOrEmpty(CSVLoaderWindow.window.csvSavePathFileRootPath)) return string.Empty;
                    var folderName = scriptSetting == null ? string.Empty : scriptSetting.scriptName;
                    var folder = $"{CSVLoaderWindow.window.csvSavePathFileRootPath}/{folderName}";
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                        AssetDatabase.Refresh();
                    }
                    return Helper.GetUnityAssetPath($"{folder}/{scriptSetting.scriptAssetName}.asset");
                }
                else return m_resourceFilePath;
            }
            set => m_resourceFilePath = value;
        }
        /// <summary>
        /// 脚本保存位置
        /// </summary>
        public string scriptFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(CSVLoaderWindow.window.csvSavePathFileRootPath)) return string.Empty;
                var folderName = scriptSetting == null ? string.Empty : scriptSetting.scriptName;
                var path = $"{CSVLoaderWindow.window.csvSavePathFileRootPath}/{folderName}";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    AssetDatabase.Refresh();
                }
                return path;
            }
        }
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
                return !string.IsNullOrEmpty(m_loadFilePath);
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
        public FileInfo csvFileInfo { get; private set; }
        /// <summary>
        /// 块颜色
        /// </summary>
        public Color blockColor = Color.clear;
        /// <summary>
        /// 块高度
        /// </summary>
        public float blockHeight;
        public ScriptSetting scriptSetting;
        /// <summary>
        /// 是否需要创建脚本
        /// </summary>
        public bool generateScriptFlag
        {
            get
            {
                var create = true;
                create &= !CheckHaveSameScript(scriptSetting.scriptFullname, false);
                create &= !CheckHaveSameScript(scriptSetting.scriptAssetFullName, false);
                return create;
            }
        }
        public bool sameFileFlag = false;
        private string m_loadFilePath;
        private string m_resourceFilePath;
        private Dictionary<string, bool> sameScriptMap = new Dictionary<string, bool>();
        public GenerateData(int createIndex)
        {
            this.createIndex = createIndex;
        }
        /// <summary>
        /// 设置块状态
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public async void SetState(BlockState state, bool withDelay = true)
        {
            await Task.Yield();
            switch (state)
            {
                case BlockState.NORMAL:
                    blockColor = Color.clear;
                    break;
                case BlockState.ERROR:
                    blockColor = new Color(255, 0, 0, 0.5f);
                    if (withDelay)
                    {
                        await Task.Delay(System.TimeSpan.FromSeconds(1.5f));
                        SetState(BlockState.NORMAL);
                    }
                    break;
            }
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

        /// <summary>
        /// 检查是否有相同脚本
        /// </summary>
        /// <param name="scriptFullName">脚本全名</param>
        /// <returns>true: 有相同</returns>
        public bool CheckHaveSameScript(string scriptFullName, bool withNotification)
        {
            if (sameScriptMap.ContainsKey(scriptFullName))
            {
                return sameScriptMap[scriptFullName];
            }
            var haveSame = false;
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GetType(scriptFullName) != null)
                {
                    if (withNotification) ShowNotificationAndLog($"类\"{scriptFullName}\"已存在");
                    haveSame = true;
                    break;
                }
            }
            sameScriptMap.Add(scriptFullName, haveSame);
            return haveSame;
        }
        public void ShowNotificationAndLog(string content)
        {
            CSVLoaderWindow.window.ShowNotification(new GUIContent(content));
            Debug.LogError(content);
        }
        /// <summary>
        /// 检测设置完成
        /// </summary>
        /// <returns></returns>
        public bool CheckSettingComplete()
        {
            return scriptAssetData.CheckSettingComplete();
        }

        public const string START_MARK = "@start";
        public const string END_MARK = "@end";
    }

    public enum BlockState
    {
        NORMAL,
        ERROR,
    }
}