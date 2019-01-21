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
            recordPropertyMap = new Dictionary<string, IProperty>();
            scriptRawData = new RawData(rawDataSource);
            if (scriptRawData.width <= 0 || scriptRawData.height <= 0) throw new System.Exception($"{generateData.csvFileInfo.Name}信息缺失");
            ResolveDefaultProperty();
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
            "int", "float", "string", "bool",
            "int_array", "float_array", "string_array", "bool_array"
        };
        #endregion


        #region public method
        public void GenerateScript()
        {
            var savePath = string.Format("{1}{0}{2}.cs", CSVLoaderWindow.Seperator(), generateData.scriptFilePath, generateData.scriptSetting.scriptName);
            /* 当前为加载模式,如果文件存在就不需要创建 */
            if (!generateData.generateScriptFlag && File.Exists(savePath)) return;
            /* 检测相同脚本 */
            if (generateData.CheckHaveSameScript(generateData.scriptSetting.scriptFullname, true))
            {
                generateData.SetState(BlockState.ERROR);
                return;
            }
            if (File.Exists(savePath))
            {
                CSVLoaderWindow.window.ShowNotification(new GUIContent($"文件\"{generateData.scriptSetting.scriptName}\"已存在"));
                generateData.SetState(BlockState.ERROR);
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
        /// 解析普通属性
        /// </summary>
        private void ResolveDefaultProperty()
        {
            var errorFalg = false;
            /* [0] 属性定义行 */
            /* 属性定义样式 */
            /*
                int,float,string,bool,int_array,float_array,string_array,bool_array: TYPE#PROPERTY_NAME(类型#属性名称)
            */
            var propertyRaw = scriptRawData.GetRow(0);
            /* 读取一次属性后,到下一个属性步进值,需要根据当前属性数据占用范围来计算 */
            /* 像Array或Array2D这样的数据占用的宽度会更大 */
            var dataSourceColumnIndex = 0;
            for (var columnIndex = 0; columnIndex < propertyRaw.Length; columnIndex++)
            {
                var propertyRawData = propertyRaw[columnIndex];
                var propertyData = propertyRawData.Split(Helper.SETTING_SPLIT);
                if (propertyData[0].Equals(RawData.FILLING_DATA)) continue;
                /* 整竖行都是空,则忽略该竖行 */
                var columnIsEmpty = true;
                for (var y = 0; y < scriptRawData.height; y++)
                {
                    columnIsEmpty &= string.IsNullOrEmpty(scriptRawData[columnIndex, y]);
                    if (!columnIsEmpty) break;
                }
                if (columnIsEmpty) continue;
                if (!valiblePropertySet.Contains(propertyData[0])) throw new System.Exception($"未包含属性{propertyData[0]}");
                var property = default(IProperty);
                var range = new RawRange(1, dataSourceColumnIndex, 1, scriptRawData.height - 1);
                /* 基础模式 */
                if (generateData.scriptSetting.scriptObjectDataType == ScriptObjectDataType.BASE)
                {
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
                        case "int_array":
                            property = new IntArrayProperty();
                            range.width = Convert.ToInt32(propertyData[2]);
                            /* 为了适应数据宽度超过属性栏宽度 */
                            columnIndex += range.width - 1;
                            break;
                        case "float_array":
                            property = new FloatArrayProperty();
                            range.width = Convert.ToInt32(propertyData[2]);
                            /* 为了适应数据宽度超过属性栏宽度 */
                            columnIndex += range.width - 1;
                            break;
                        case "string_array":
                            property = new StringArrayProperty();
                            range.width = Convert.ToInt32(propertyData[2]);
                            /* 为了适应数据宽度超过属性栏宽度 */
                            columnIndex += range.width - 1;
                            break;
                        case "bool_array":
                            property = new BooleanArrayProperty();
                            range.width = Convert.ToInt32(propertyData[2]);
                            /* 为了适应数据宽度超过属性栏宽度 */
                            columnIndex += range.width - 1;
                            break;
                        default: throw new System.Exception($"\"{generateData.loadFilePath}\"未定义类型\"{propertyData[0]}\"");
                    }
                }
                /* 二维数组模式 */
                else if (generateData.scriptSetting.scriptObjectDataType == ScriptObjectDataType.ARRAY2D)
                {
                    switch (propertyData[0])
                    {
                        case "int":
                            property = new IntArray2DProperty();
                            range.width = Convert.ToInt32(propertyData[2]);
                            break;
                        case "float":
                            property = new FloatArray2DProperty();
                            range.width = Convert.ToInt32(propertyData[2]);
                            break;
                        case "string":
                            property = new StringArray2DProperty();
                            range.width = Convert.ToInt32(propertyData[2]);
                            break;
                        case "bool":
                            property = new BooleanArray2DProperty();
                            range.width = Convert.ToInt32(propertyData[2]);
                            break;
                        default: throw new System.Exception($"\"{generateData.loadFilePath}\"未定义类型\"{propertyData[0]}\"");
                    }
                }
                else if (generateData.scriptSetting.scriptObjectDataType == ScriptObjectDataType.ARRAY2DWITHNAME)
                {
                    switch (propertyData[0])
                    {
                        case "int":
                            property = new IntArray2DWithnameProperty();
                            range.width = Convert.ToInt32(propertyData[2]) + 1;
                            break;
                        case "float":
                            property = new FloatArray2DWithnameProperty();
                            range.width = Convert.ToInt32(propertyData[2]) + 1;
                            break;
                        case "string":
                            property = new StringArray2DWithnameProperty();
                            range.width = Convert.ToInt32(propertyData[2]) + 1;
                            break;
                        case "bool":
                            property = new BooleanArray2DWithnameProperty();
                            range.width = Convert.ToInt32(propertyData[2]) + 1;
                            break;
                        default: throw new System.Exception($"\"{generateData.loadFilePath}\"未定义类型\"{propertyData[0]}\"");
                    }
                }
                else throw new System.Exception($"未定义的类型\"{generateData.scriptSetting.scriptObjectDataType}\"");
                property.InitProperty(propertyData, scriptRawData.GetRangeRawData(range));
                if (recordPropertyMap.ContainsKey(property.propertyName))
                {
                    CSVLoaderWindow.window.ShowNotification(new GUIContent($"文件\"{generateData.scriptSetting.scriptName}\"存在相同属性名称{property.propertyName}"));
                    generateData.SetState(BlockState.ERROR);
                    errorFalg = true;
                    break;
                }
                recordPropertyMap.Add(property.propertyName, property);
                dataSourceColumnIndex += range.width;
            }
            if (!errorFalg) GenerateScriptContent();
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