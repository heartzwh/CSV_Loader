//Author: sora

namespace Sora.Tools.CSVLoader
{
    public class ScriptSetting
    {
        #region constructor
        public ScriptSetting(ScriptData scriptData, string[] setting)
        {
            this.scriptData = scriptData;
            if (setting.Length < 1) throw new System.Exception($"{scriptData.generateData.fileInfo.Name}未写脚本设置");
            /* 类设置 */
            var namespaceString = setting[0];
            var lastDotIndex = namespaceString.LastIndexOf('.');
            namespaceName = namespaceString.Substring(0, lastDotIndex);
            scriptName = namespaceString.Substring(lastDotIndex + 1);
            scriptFullname = namespaceString;
        }
        #endregion


        #region event/delegate

        #endregion


        #region property
        /// <summary>
        /// 脚本命名空间
        /// </summary>
        /// <value></value>
        public readonly string namespaceName;
        /// <summary>
        /// 脚本名称
        /// </summary>
        /// <value></value>
        public readonly string scriptName;
        /// <summary>
        /// 脚本全名
        /// </summary>
        public readonly string scriptFullname;
        public readonly ScriptData scriptData;
        #endregion


        #region public method

        #endregion


        #region protected method

        #endregion


        #region private method

        #endregion


        #region static

        #endregion
    }
}