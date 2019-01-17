//Author: sora

using System.Collections.Generic;

namespace Sora.Tools.CSVLoader
{
    public class CreateFileManager
    {
        #region constructor

        #endregion


        #region event/delegate

        #endregion


        #region property
        private HashSet<string> propertyValibleSet = new HashSet<string>()
        {
            "int", "float", "string", "bool", "double", "long"
        };
        #endregion


        #region public method

        #endregion


        #region protected method

        #endregion


        #region private method
        private string GenerateDefaultScriptContent(Dictionary<int, List<string>> dataSet)
        {
            var scriptContent = new System.Text.StringBuilder();
            var tabCount = 0;

            /* 脚本设置 */
            /* @start,namespace */
            var scriptSetting = dataSet[0];
            var namespaceStr = scriptSetting[1];
            if (!string.IsNullOrEmpty(namespaceStr))
            {
                scriptContent.AppendLine($"{GetTab(tabCount)}namespace {namespaceStr}");
                scriptContent.AppendLine(GetTab(tabCount) + "{");
                tabCount++;
            }
            /* 脚本属性 */
            var scriptProperty = dataSet[1];
            scriptContent.AppendLine($"{GetTab(tabCount)}public class #CLASS_NAME#");
            scriptContent.AppendLine(GetTab(tabCount) + "{");
            tabCount++;
            var propertyIndex = 0;
            foreach (var property in scriptProperty)
            {
                var data = property.Split(SETTING_SPLIT);
                /* 属性名称 */
                var propertyName = data[0];
                /* 属性类型 */
                var propertyNameType = data[1];
                if (!propertyValibleSet.Contains(propertyNameType))
                {
                    throw new System.Exception($"包含未定义类型{propertyNameType}");
                }
                scriptContent.AppendLine($"{GetTab(tabCount)}public {propertyNameType} {propertyName}");
                if (propertyIndex < scriptProperty.Count - 1) scriptContent.AppendLine();
                propertyIndex++;
            }
            tabCount--;
            scriptContent.AppendLine(GetTab(tabCount) + "}");
            if (!string.IsNullOrEmpty(namespaceStr))
            {
                tabCount--;
                scriptContent.AppendLine(GetTab(tabCount) + "}");
            }
            return scriptContent.ToString();
        }

        private string GenerateArray2DScriptContent(Dictionary<int, List<string>> dataSet)
        {
            return "";
        }
        private void CheckScriptPropertyValible(string propertyName)
        {

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
        public static CreateFileManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new CreateFileManager();
                }
                return _Instance;
            }
        }
        private static CreateFileManager _Instance;
        private const char SETTING_SPLIT = '#';
        #endregion
    }
}