//Author: sora

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

namespace Sora.Tools.CSVLoader.Editor
{
    public class Array2DScriptAsset : BaseScriptAsset
    {
        #region constructor

        #endregion


        #region event/delegate

        #endregion


        #region property

        #endregion


        #region public method
        public override void InitData()
        {
            var assembly = CSVLoaderWindow.window.assembly;
            /* 添加数据的脚本类 */
            var script = assembly.CreateInstance(generateData.scriptSetting.scriptFullname);
            var scriptType = script.GetType();
            foreach (var field in script.GetType().GetFields())
            {
                /* 获取加载时保存的数据 */
                var recordProperty = generateData.scriptData.recordPropertyMap[field.Name];
                // UnityEngine.Debug.Log("FullName " + recordProperty.GetType().FullName + " " + field.Name);
                /* Field: IProperty类,所以需要创建 */
                var fieldScript = assembly.CreateInstance(recordProperty.GetType().FullName);
                var fieldType = fieldScript.GetType();
                /* fieldInitPropertyMethod: IProperty.InitProperty */
                var fieldInitPropertyMethod = fieldType.GetMethod("InitProperty");
                /* fieldSetPropertyValueMethod: IProperty.SetPropertyValue */
                var fieldSetPropertyValueMethod = fieldType.GetMethod("SetPropertyValue");
                /* 调用 InitProperty 方法 */
                fieldInitPropertyMethod.Invoke(fieldScript, new object[] { recordProperty.propertySetting, recordProperty.propertyRawData });
                /* 调用 SetPropertyValue 方法 */
                fieldSetPropertyValueMethod.Invoke(fieldScript, new object[] { recordProperty.propertyRawData });
                scriptType.GetField(field.Name).SetValue(script, fieldScript);
            }
            /* 创建的 ScriptableObjectAsset ,添加所有创建的数据 */
            var scriptAssetSetDataMethod = generateData.scriptAssetData.assetObject.GetType().GetField(PROPERTY_NAME_DATASET);
            scriptAssetSetDataMethod.SetValue(generateData.scriptAssetData.assetObject, script);
        }

        #endregion


        #region protected method

        protected override string GetScriptContent(int tabCount)
        {
            /* 此处定义为属性为Filed */
            var scriptContent = new System.Text.StringBuilder();
            scriptContent.AppendLine($"{GetTab(tabCount)}public {generateData.scriptSetting.scriptName} {PROPERTY_NAME_DATASET};");
            return scriptContent.ToString();
            /* 内容为
                public CLASS_NAME dataSet;
            */
        }

        #endregion


        #region private method

        #endregion


        #region static
        #endregion
    }
}