//Author: sora

using System.IO;
using UnityEditor;
using UnityEngine;

namespace Sora.Tools.CSVLoader.Editor
{
    public class ScriptAssetData
    {
        #region constructor
        public ScriptAssetData(GenerateData generateData)
        {
            this.generateData = generateData;
            GenerateScriptContent();
        }
        #endregion


        #region event/delegate

        #endregion


        #region property
        public readonly GenerateData generateData;
        /// <summary>
        /// 脚本内容
        /// </summary>
        /// <value></value>
        public string scriptContent { get; private set; }
        public IScriptAsset scriptAsset { get; private set; }
        /// <summary>
        /// 资源路径
        /// </summary>
        /// <value></value>
        public string assetPath
        {
            get
            {
                return Helper.GetUnityAssetPath(string.Format("{1}{0}{2}.asset", CSVLoaderWindow.Seperator(), generateData.resourceFilePath, generateData.scriptSetting.scriptAssetName));
            }
        }
        /// <summary>
        /// 资源文件
        /// </summary>
        /// <value></value>
        public ScriptableObject assetObject { get; private set; }
        #endregion


        #region public method
        public void GenerateScript()
        {
            var savePath = string.Format("{1}{0}{2}.cs", CSVLoaderWindow.Seperator(), CSVLoaderWindow.scriptFilePath, generateData.scriptSetting.scriptAssetName);
            if (File.Exists(savePath))
            {
                CSVLoaderWindow.window.ShowNotification(new GUIContent($"文件\"{generateData.scriptSetting.scriptAssetName}\"已存在"));
                generateData.blockColor = GenerateData.ERROR_COLOR;
                generateData.ClearColor();
                return;
            }
            StreamWriter sw = File.CreateText(savePath);
            sw.Write(scriptContent);
            sw.Flush();
            sw.Close();
        }
        public void GenerateAsset()
        {
            assetObject = ScriptableObject.CreateInstance(generateData.scriptSetting.scriptAssetFullName);
            AssetDatabase.CreateAsset(assetObject, assetPath);
        }
        #endregion


        #region protected method

        #endregion


        #region private method
        private void GenerateScriptContent()
        {
            switch (generateData.scriptSetting.scriptObjectDataType)
            {
                case ScriptObjectDataType.DEFAULT:
                    scriptAsset = new DefaultScriptAsset();
                    break;
            }
            scriptAsset.InitScriptAsset(generateData);
            scriptContent = scriptAsset.scriptContent;
        }
        #endregion


        #region static

        #endregion
    }
}