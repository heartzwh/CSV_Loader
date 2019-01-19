//Author: sora

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Sora.Tools.CSVLoader.Editor
{
    /// <summary>
    /// 脚本包含的数据
    /// </summary>
    public class ScriptData
    {
        #region constructor
        /// <summary>
        /// 构建脚本
        /// </summary>
        /// <param name="generateData">生成数据</param>
        /// <param name="rawDataSource">csv中@start到@end中数据</param>
        public ScriptData(GenerateData generateData, string rawDataSource)
        {
            this.generateData = generateData;
            /* 当前为数据加载,则不检测相同脚本,因为不用生成脚本了 */
            if (!CSVLoaderWindow.loadDataFlag && CheckHaveSameScript())
            {
                generateData.blockColor = GenerateData.ERROR_COLOR;
                generateData.ClearColor();
                return;
            }
            recordPropertyMap = new Dictionary<string, IProperty>();
            scriptRawData = new RawData(rawDataSource);
            if (scriptRawData.width <= 0 || scriptRawData.height <= 0) throw new System.Exception($"{generateData.csvFileInfo.Name}信息缺失");
            ResolveProperty();
        }

        #endregion


        #region event/delegate

        #endregion


        #region property
        /// <summary>
        /// 记录脚本所有属性
        /// key: property name
        /// value: 属性
        /// </summary>
        public Dictionary<string, IProperty> recordPropertyMap { get; private set; }
        /// <summary>
        /// c#脚本内容
        /// </summary>
        /// <value></value>
        public string scriptContent { get; private set; }
        public readonly GenerateData generateData;
        public readonly RawData scriptRawData;
        /// <summary>
        /// 可以使用属性
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <returns></returns>
        private HashSet<string> valiblePropertySet = new HashSet<string>()
        {
            "int", "float", "string", "bool", "double", "long"
        };
        #endregion


        #region public method
        public void GenerateScript()
        {
            var savePath = string.Format("{1}{0}{2}.cs", CSVLoaderWindow.Seperator(), CSVLoaderWindow.scriptFilePath, generateData.scriptSetting.scriptName);
            if (File.Exists(savePath))
            {
                CSVLoaderWindow.window.ShowNotification(new GUIContent($"文件\"{generateData.scriptSetting.scriptName}\"已存在"));
                generateData.blockColor = GenerateData.ERROR_COLOR;
                generateData.ClearColor();
                return;
            }
            StreamWriter sw = File.CreateText(savePath);
            sw.Write(scriptContent);
            sw.Flush();
            sw.Close();
        }
        #endregion


        #region protected method

        #endregion


        #region private method
        /// <summary>
        /// 解析所有属性
        /// </summary>
        private void ResolveProperty()
        {
            var propertyRaw = scriptRawData.GetRow(0);
            for (var columnIndex = 0; columnIndex < propertyRaw.Length; columnIndex++)
            {
                var propertyRawData = propertyRaw[columnIndex];
                var propertyData = propertyRawData.Split(Helper.SETTING_SPLIT);
                if (!valiblePropertySet.Contains(propertyData[0])) throw new System.Exception($"未包含属性{propertyData[0]}");
                var property = default(IProperty);
                var range = new RawRange(1, columnIndex, 1, scriptRawData.height - 1);
                switch (propertyData[0])
                {
                    case "int":
                        property = new IntProperty();
                        break;
                    case "float":
                        property = new FloatProperty();
                        break;
                    case "string":
                        property = new StringProperty();
                        break;
                    case "bool":
                        property = new BooleanProperty();
                        break;
                    default: throw new System.Exception($"\"{generateData.loadFilePath}\"未定义类型\"{propertyData[0]}\"");
                }
                property.InitProperty(propertyData, scriptRawData.GetRangeRawData(range));
                recordPropertyMap.Add(property.propertyName, property);
            }
            GenerateScriptContent();
        }

        private void GenerateScriptContent()
        {
            var scriptContent = new System.Text.StringBuilder();
            var tabCount = 0;
            /* 脚本设置 */
            if (!string.IsNullOrEmpty(generateData.scriptSetting.namespaceName))
            {
                scriptContent.AppendLine($"{GetTab(tabCount)}namespace {generateData.scriptSetting.namespaceName}");
                scriptContent.AppendLine(GetTab(tabCount) + "{");
                tabCount++;
            }
            /* 脚本属性 */
            scriptContent.AppendLine($"{GetTab(tabCount)}[System.Serializable]");
            scriptContent.AppendLine($"{GetTab(tabCount)}public class {generateData.scriptSetting.scriptName}");
            scriptContent.AppendLine(GetTab(tabCount) + "{");
            tabCount++;
            var propertyIndex = 0;
            foreach (var property in recordPropertyMap.Values)
            {
                scriptContent.AppendLine($"{GetTab(tabCount)}{property.propertyContent}");
                if (propertyIndex < recordPropertyMap.Count - 1) scriptContent.AppendLine();
                propertyIndex++;
            }
            tabCount--;
            scriptContent.AppendLine(GetTab(tabCount) + "}");
            if (!string.IsNullOrEmpty(generateData.scriptSetting.namespaceName))
            {
                tabCount--;
                scriptContent.AppendLine(GetTab(tabCount) + "}");
            }
            this.scriptContent = scriptContent.ToString();
        }
        private bool CheckHaveSameScript()
        {
            var haveSame = false;
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GetType(generateData.scriptSetting.scriptFullname) != null)
                {
                    CSVLoaderWindow.window.ShowNotification(new GUIContent($"类\"{generateData.scriptSetting.scriptFullname}\"已存在"));
                    haveSame = true;
                    break;
                }
            }
            return haveSame;
        }
        private string GetTab(int count)
        {
            var tabStr = "";
            for (int i = 0; i < count; i++)
            {
                tabStr += "    ";
            }
            return tabStr;
        }
        #endregion


        #region static

        #endregion
    }
}