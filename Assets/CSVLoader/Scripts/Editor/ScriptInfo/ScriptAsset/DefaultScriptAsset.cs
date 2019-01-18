//Author: sora

namespace Sora.Tools.CSVLoader
{
    public class DefaultScriptAsset : BaseScriptAsset
    {
        #region constructor

        #endregion


        #region event/delegate

        #endregion


        #region property

        #endregion


        #region public method

        #endregion


        #region protected method
        protected override string GetPropertyContent(int tabCount)
        {
            return $"{GetTab(tabCount)}public System.Collections.Generic.List<{generateData.scriptSetting.scriptName}> dataSet;";
        }
        #endregion


        #region private method

        #endregion


        #region static

        #endregion
    }
}