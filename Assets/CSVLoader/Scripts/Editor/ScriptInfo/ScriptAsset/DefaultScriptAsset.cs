//Author: sora

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

namespace Sora.Tools.CSVLoader.Editor
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
        public override void InitData()
        {
            var dataSource = new List<object>();
            /* 从 recordProperty 中获取数据索引 */
            var getPropertyValueIndex = 0;
            /* dataIndex从1开始,是因为[0]数据为属性的设置等等信息,[1]开始才是属性的数据信息 */
            /* 获取 generateData.scriptData.scriptRawData.height 可以知道整个属性中有多个条属性值 */
            for (var dataIndex = 1; dataIndex < generateData.scriptData.scriptRawData.height; dataIndex++)
            {
                /* 添加数据的脚本类 */
                var script = Assembly.GetAssembly(Type.GetType(generateData.scriptSetting.scriptFullname)).CreateInstance(generateData.scriptSetting.scriptFullname);
                var scriptType = script.GetType();
                foreach (var field in script.GetType().GetFields())
                {
                    /* 获取加载时保存的数据 */
                    var recordProperty = generateData.scriptData.recordPropertyMap[field.Name];
                    // UnityEngine.Debug.Log("FullName " + recordProperty.GetType().FullName + " " + field.Name + " " + recordProperty.propertyRawData[0, getPropertyValueIndex]);
                    /* Field: IProperty类,所以需要创建 */
                    var fieldScript = Assembly.GetAssembly(recordProperty.GetType()).CreateInstance(recordProperty.GetType().FullName);
                    var fieldType = fieldScript.GetType();
                    /* fieldInitPropertyMethod: IProperty.InitProperty */
                    var fieldInitPropertyMethod = fieldType.GetMethod("InitProperty");
                    /* fieldSetPropertyValueMethod: IProperty.SetPropertyValue */
                    var fieldSetPropertyValueMethod = fieldType.GetMethod("SetPropertyValue");
                    /* 调用 InitProperty 方法 */
                    fieldInitPropertyMethod.Invoke(fieldScript, new object[] { recordProperty.propertySetting, recordProperty.propertyRawData });
                    /* 根据属性占用数据范围,获取需要设置的值 */
                    var setDataSource = recordProperty.propertyRawData.GetRangeRawData(new RawRange(getPropertyValueIndex, 0, recordProperty.propertyRawData.width, 1));
                    /* 调用 SetPropertyValue 方法 */
                    fieldSetPropertyValueMethod.Invoke(fieldScript, new object[] { setDataSource });
                    scriptType.GetField(field.Name).SetValue(script, fieldScript);
                }
                getPropertyValueIndex++;
                dataSource.Add(script);
            }
            /* 创建的 ScriptableObjectAsset ,添加所有创建的数据 */
            var scriptAssetSetDataMethod = generateData.scriptAssetData.assetObject.GetType().GetMethod(METHOD_NAME_SETDATA);
            scriptAssetSetDataMethod.Invoke(generateData.scriptAssetData.assetObject, new object[] { dataSource });
        }
        #endregion


        #region protected method
        protected override string GetScriptContent(int tabCount)
        {
            var scriptContent = new System.Text.StringBuilder();
            /* 此处定义为属性为Filed */
            scriptContent.AppendLine($"{GetTab(tabCount)}public System.Collections.Generic.List<{generateData.scriptSetting.scriptName}> {PROPERTY_NAME_DATASET};");
            scriptContent.AppendLine();
            /* 定义添加数据函数 */
            scriptContent.AppendLine($"{GetTab(tabCount)}public void {METHOD_NAME_SETDATA}(System.Collections.Generic.List<object> dataSetSource)");
            scriptContent.AppendLine(GetTab(tabCount) + "{");
            tabCount++;
            scriptContent.AppendLine($"{GetTab(tabCount)}{PROPERTY_NAME_DATASET} = new System.Collections.Generic.List<{generateData.scriptSetting.scriptName}>();");
            scriptContent.AppendLine($"{GetTab(tabCount)}foreach (var data in dataSetSource)");
            scriptContent.AppendLine(GetTab(tabCount) + "{");
            tabCount++;
            scriptContent.AppendLine($"{GetTab(tabCount)}{PROPERTY_NAME_DATASET}.Add(data as {generateData.scriptSetting.scriptName});");
            tabCount--;
            scriptContent.AppendLine(GetTab(tabCount) + "}");
            tabCount--;
            scriptContent.AppendLine(GetTab(tabCount) + "}");
            return scriptContent.ToString();
            /* 内容为
            public System.Collections.Generic.List<CLASS_NAME> dataSet;

            public void SetData(System.Collections.Generic.List<object> dataSetSource)
            {
                dataSet = new System.Collections.Generic.List<CLASS_NAME>();
                foreach (var data in dataSetSource)
                {
                    dataSet.Add(data as CLASS_NAME);
                }
            }
            */
        }
        #endregion


        #region private method
        private const string PROPERTY_NAME_DATASET = "dataSet";
        private const string METHOD_NAME_SETDATA = "SetData";
        #endregion


        #region static

        #endregion
    }
}