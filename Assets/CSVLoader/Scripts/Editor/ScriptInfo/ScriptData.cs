//Author: sora

using System;
using System.Collections.Generic;
using System.Linq;

namespace Sora.Tools.CSVLoader
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
        /// <param name="rawScriptSetting">脚本设置</param>
        /// <param name="rawDataSource">csv中@start到@end中数据</param>
        public ScriptData(GenerateData generateData, string[] rawScriptSetting, string rawDataSource)
        {
            this.generateData = generateData;
            scriptSetting = new ScriptSetting(this, rawScriptSetting);
            propertyList = new List<IProperty>();
            scriptRawData = new RawData(rawDataSource);
            if (scriptRawData.width <= 0 || scriptRawData.height <= 0) throw new System.Exception($"{generateData.fileInfo.Name}信息缺失");
            ResolveProperty();
        }

        #endregion


        #region event/delegate

        #endregion


        #region property
        public ScriptSetting scriptSetting { get; private set; }
        /// <summary>
        /// 所有属性
        /// IProperty: 属性
        /// </summary>
        public List<IProperty> propertyList { get; private set; }
        /// <summary>
        /// 脚本所有内容
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
                var propertyData = propertyRawData.Split(GenerateData.SETTING_SPLIT);
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
                    default: throw new System.Exception($"为定义类型\"{propertyData[0]}\"");
                }
                property.InitProperty(propertyData, scriptRawData.GetRangeRawData(range));
                propertyList.Add(property);
            }
            GenerateScriptContent();
        }

        private void GenerateScriptContent()
        {
            var scriptContent = new System.Text.StringBuilder();
            var tabCount = 0;
            /* 脚本设置 */
            if (!string.IsNullOrEmpty(scriptSetting.namespaceName))
            {
                scriptContent.AppendLine($"{GetTab(tabCount)}namespace {scriptSetting.namespaceName}");
                scriptContent.AppendLine(GetTab(tabCount) + "{");
                tabCount++;
            }
            /* 脚本属性 */
            scriptContent.AppendLine($"{GetTab(tabCount)}public class {scriptSetting.scriptName}");
            scriptContent.AppendLine(GetTab(tabCount) + "{");
            tabCount++;
            var propertyIndex = 0;
            foreach (var property in propertyList)
            {
                scriptContent.AppendLine($"{GetTab(tabCount)}{property.propertyContent}");
                if (propertyIndex < propertyList.Count - 1) scriptContent.AppendLine();
                propertyIndex++;
            }
            tabCount--;
            scriptContent.AppendLine(GetTab(tabCount) + "}");
            if (!string.IsNullOrEmpty(scriptSetting.namespaceName))
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