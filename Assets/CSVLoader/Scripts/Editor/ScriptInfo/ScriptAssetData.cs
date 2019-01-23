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
        /// 资源文件
        /// </summary>
        /// <value></value>
        public ScriptableObject assetObject { get; private set; }
        #endregion


        #region public method
        public void GenerateScript()
        {
            var savePath = string.Format("{1}{0}{2}.cs", CSVLoaderWindow.Seperator(), generateData.scriptFilePath, generateData.scriptSetting.scriptAssetName);
            /* 当前为加载模式,如果文件存在就不需要创建 */
            if (!generateData.generateScriptFlag && File.Exists(savePath)) return;
            /* 检测相同脚本 */
            if (generateData.CheckHaveSameScript(generateData.scriptSetting.scriptAssetFullName, true))
            {
                generateData.SetState(BlockState.ERROR);
                return;
            }
            if (File.Exists(savePath))
            {
                CSVLoaderWindow.window.ShowNotification(new GUIContent($"文件\"{generateData.scriptSetting.scriptAssetName}\"已存在"));
                generateData.SetState(BlockState.ERROR);
                return;
            }
            StreamWriter sw = File.CreateText(savePath);
            sw.Write(scriptContent);
            sw.Flush();
            sw.Close();
        }
        public void GenerateAsset()
        {
            /* 当文件存在时 */
            var assetPath = $"{Helper.DataPathWithoutAssets()}{CSVLoaderWindow.Seperator()}{generateData.resourceFilePath}";
            if (File.Exists(assetPath))
            {
                assetObject = AssetDatabase.LoadAssetAtPath(generateData.resourceFilePath, typeof(ScriptableObject)) as ScriptableObject;
            }
            else
            {
                assetObject = ScriptableObject.CreateInstance(generateData.scriptSetting.scriptAssetFullName);
                AssetDatabase.CreateAsset(assetObject, generateData.resourceFilePath);
            }
        }
        /// <summary>
        /// 设置 ScriptableObject
        /// </summary>
        /// <param name="assetObject"></param>
        /// <returns>true: 设置成功</returns>
        public bool SetAssetObject(ScriptableObject assetObject)
        {
            var ok = false;
            /* 判断加载的 ScriptableObject 与数据类型是否匹配  */
            if (assetObject != null)
            {
                if (!assetObject.GetType().FullName.Equals(generateData.scriptSetting.scriptAssetFullName))
                {
                    generateData.SetState(BlockState.ERROR);
                    generateData.ShowNotificationAndLog($"类型不匹配");
                }
                else
                {
                    ok = true;
                    this.assetObject = assetObject;
                }
            }
            else this.assetObject = assetObject;
            return ok;
        }
        /// <summary>
        /// 检测设置是否完成
        /// </summary>
        /// <returns></returns>
        public bool CheckSettingComplete()
        {
            var ok = true;
            if (!generateData.generateScriptFlag && assetObject == null)
            {
                generateData.SetState(BlockState.ERROR);
                Debug.LogError($"\"{generateData.csvFileInfo.Name}\"未设置Asset");
                ok = false;
            }
            return ok;
        }
        #endregion


        #region protected method

        #endregion


        #region private method
        private void GenerateScriptContent()
        {
            switch (generateData.scriptSetting.scriptObjectDataType)
            {
                case ScriptObjectDataType.BASE:
                    scriptAsset = new DefaultScriptAsset();
                    break;
                case ScriptObjectDataType.ARRAY2D:
                case ScriptObjectDataType.ARRAY2DWITHNAME:
                    scriptAsset = new Array2DScriptAsset();
                    break;
                case ScriptObjectDataType.DICTIONARY:
                    scriptAsset = new DictionayScriptAsset();
                    break;
                default: throw new System.Exception($"未定义的类型{generateData.scriptSetting.scriptObjectDataType}");
            }
            scriptAsset.InitScriptAsset(generateData);
            scriptContent = scriptAsset.scriptContent;
        }
        #endregion


        #region static

        #endregion
    }
}