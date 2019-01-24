//Author: sora

namespace Sora.Tools.CSVLoader.Editor
{
    public class ScriptSetting
    {
        #region constructor
        public ScriptSetting() { }
        public ScriptSetting(GenerateData generateData, string[] setting)
        {
            this.generateData = generateData;
            if (setting.Length < 1) throw new System.Exception($"{generateData.csvFileInfo.Name}未写脚本设置");
            /* 类信息: 命名空间.类名称 */
            var namespaceString = setting[0];
            var lastDotIndex = namespaceString.LastIndexOf('.');
            if (!lastDotIndex.Equals(-1))
            {
                namespaceName = namespaceString.Substring(0, lastDotIndex);
            }
            scriptName = namespaceString.Substring(lastDotIndex + 1);
            scriptFullname = namespaceString;
            if (setting.Length < 2) throw new System.Exception($"{generateData.csvFileInfo.Name}未写script object设置");
            /* script object信息: 数据显示方式 */
            scriptAssetName = $"{scriptName}Source";
            scriptAssetFullName = $"{scriptFullname}Source";
            scriptObjectDataType = (ScriptObjectDataType)System.Enum.Parse(typeof(ScriptObjectDataType), setting[1].ToUpper());
        }
        #endregion


        #region event/delegate

        #endregion


        #region property
        /// <summary>
        /// 脚本命名空间
        /// </summary>
        /// <value></value>
        public string namespaceName;
        /// <summary>
        /// 脚本名称
        /// </summary>
        /// <value></value>
        public string scriptName;
        /// <summary>
        /// 资源脚本名称
        /// </summary>
        public string scriptAssetName;
        /// <summary>
        /// 脚本全名
        /// </summary>
        public string scriptFullname;
        /// <summary>
        /// 资源脚本全名
        /// </summary>
        public string scriptAssetFullName;
        public GenerateData generateData;
        /// <summary>
        /// ScriptObject 中数据排列的方式
        /// </summary>
        /// <value></value>
        public ScriptObjectDataType scriptObjectDataType { get; private set; }
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

    public enum ScriptObjectDataType
    {
        /// <summary>
        /// 使用默认方式(list)
        /// </summary>
        BASE,
        /// <summary>
        /// 使用2D数组
        /// </summary>
        ARRAY2D,
        /// <summary>
        /// 使用2D数组并且带有名称(player, target为名称)
        ///       , player  ,   target
        /// player, APlayer ,   ATarget
        /// target, BPlayer ,   BTarget
        /// </summary>
        ARRAY2DWITHNAME,
    }
}