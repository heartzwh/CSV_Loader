//Author: sora

namespace Sora.Tools.CSVLoader.Editor
{
    public abstract class BaseScriptAsset : IScriptAsset
    {
        #region constructor
        #endregion


        #region event/delegate

        #endregion


        #region property
        public string scriptContent { get; private set; }
        protected GenerateData generateData { get; private set; }

        #endregion


        #region public method
        public void InitScriptAsset(GenerateData generateData)
        {
            this.generateData = generateData;
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
            scriptContent.AppendLine($"{GetTab(tabCount)}public class {generateData.scriptSetting.scriptAssetName} : UnityEngine.ScriptableObject, {typeof(ICSVLoaderAsset).FullName}");
            scriptContent.AppendLine(GetTab(tabCount) + "{");
            tabCount++;
            scriptContent.AppendLine(GetScriptContent(tabCount));
            tabCount--;
            scriptContent.AppendLine(GetTab(tabCount) + "}");
            if (!string.IsNullOrEmpty(generateData.scriptSetting.namespaceName))
            {
                tabCount--;
                scriptContent.AppendLine(GetTab(tabCount) + "}");
            }
            this.scriptContent = scriptContent.ToString();
        }
        public abstract void InitData();
        #endregion


        #region protected method
        protected abstract string GetScriptContent(int tabCount);
        protected string GetTab(int count)
        {
            var tabStr = "";
            for (int i = 0; i < count; i++)
            {
                tabStr += "    ";
            }
            return tabStr;
        }
        #endregion


        #region private method

        #endregion


        #region static
        protected const string PROPERTY_NAME_DATASET = "dataSet";
        protected const string METHOD_NAME_SETDATA = "SetData";

        #endregion
    }
}