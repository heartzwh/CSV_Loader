//Author: sora

using System.Collections.Generic;

namespace Sora.Tools.CSVLoader.Editor
{
    public class DictionayScriptAsset : BaseScriptAsset
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
            var dataSource = new List<object>();
            var keySource = new List<object>();
            /* 从 recordProperty 中获取数据索引 */
            var getPropertyValueIndex = 0;
            /* dataIndex从1开始,是因为[0]数据为属性的设置等等信息,[1]开始才是属性的数据信息 */
            /* 获取 generateData.scriptData.scriptRawData.height 可以知道整个属性中有多个条属性值 */
            for (var dataIndex = 1; dataIndex < generateData.scriptData.scriptRawData.height; dataIndex++)
            {
                /* 添加数据的脚本类 */
                // UnityEngine.Debug.Log(generateData.scriptSetting.scriptFullname);
                var script = assembly.CreateInstance(generateData.scriptSetting.scriptFullname);
                var scriptType = script.GetType();
                foreach (var field in script.GetType().GetFields())
                {
                    /* 获取加载时保存的数据 */
                    var recordProperty = generateData.scriptData.recordPropertyMap[field.Name];
                    // UnityEngine.Debug.Log("FullName " + recordProperty.GetType().FullName + " " + field.Name + " " + recordProperty.propertyRawData[0, getPropertyValueIndex]);
                    /* Field: IProperty类,所以需要创建 */
                    var fieldScript = assembly.CreateInstance(recordProperty.GetType().FullName);
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
            var keyType = "";
            var valueType = generateData.scriptSetting.scriptName;
            foreach (var property in generateData.scriptData.recordPropertyMap.Values)
            {
                if (property is IDictionaryKey)
                {
                    keyType = property.propertyValueType.FullName;
                }
            }
            if (string.IsNullOrEmpty(keyType)) throw new System.Exception($"\"{generateData.csvFileInfo.Name}\"key为空???");
            var scriptContent = new System.Text.StringBuilder();
            /* 此处定义为属性为Filed */
            scriptContent.AppendLine($"{GetTab(tabCount)}public System.Collections.Generic.Dictionary<{keyType}, {valueType}> {PROPERTY_NAME_DATASET};");
            scriptContent.AppendLine($"{GetTab(tabCount)}public System.Collections.Generic.List<{valueType}> {PROPERTY_NAME_DATASET}Editor;");
            scriptContent.AppendLine();
            /* 定义添加数据函数 */
            scriptContent.AppendLine($"{GetTab(tabCount)}public void {METHOD_NAME_SETDATA}(System.Collections.Generic.List<object> dataSetSource)");
            scriptContent.AppendLine(GetTab(tabCount) + "{");
            tabCount++;
            scriptContent.AppendLine($"{GetTab(tabCount)}{PROPERTY_NAME_DATASET} = new System.Collections.Generic.Dictionary<{keyType}, {valueType}>();");
            scriptContent.AppendLine($"{GetTab(tabCount)}{PROPERTY_NAME_DATASET}Editor = new System.Collections.Generic.List<{valueType}>();");
            scriptContent.AppendLine($"{GetTab(tabCount)}foreach (var data in dataSetSource)");
            scriptContent.AppendLine(GetTab(tabCount) + "{");
            tabCount++;
            scriptContent.AppendLine($"{GetTab(tabCount)}var fields = data.GetType().GetFields();");
            scriptContent.AppendLine($"{GetTab(tabCount)}foreach (var field in fields)");
            scriptContent.AppendLine(GetTab(tabCount) + "{");
            tabCount++;
            scriptContent.AppendLine($"{GetTab(tabCount)}var find = false;");
            scriptContent.AppendLine($"{GetTab(tabCount)}foreach (var _interface in field.FieldType.GetInterfaces())");
            scriptContent.AppendLine(GetTab(tabCount) + "{");
            tabCount++;
            scriptContent.AppendLine($"{GetTab(tabCount)}if (_interface.FullName.Equals(\"{typeof(IDictionaryKey).FullName}\"))");
            scriptContent.AppendLine(GetTab(tabCount) + "{");
            tabCount++;
            scriptContent.AppendLine($"{GetTab(tabCount)}var fieldValue = (field.GetValue(data) as {typeof(IProperty)}<{keyType}>).Value;");
            scriptContent.AppendLine($"{GetTab(tabCount)}{PROPERTY_NAME_DATASET}.Add(fieldValue, data as {valueType});");
            scriptContent.AppendLine($"{GetTab(tabCount)}{PROPERTY_NAME_DATASET}Editor.Add(data as {valueType});");
            scriptContent.AppendLine($"{GetTab(tabCount)}find = true;");
            scriptContent.AppendLine($"{GetTab(tabCount)}break;");
            tabCount--;
            scriptContent.AppendLine(GetTab(tabCount) + "}");
            tabCount--;
            scriptContent.AppendLine(GetTab(tabCount) + "}");
            scriptContent.AppendLine($"{GetTab(tabCount)}if (find) break;");
            tabCount--;
            scriptContent.AppendLine(GetTab(tabCount) + "}");
            tabCount--;
            scriptContent.AppendLine(GetTab(tabCount) + "}");
            tabCount--;
            scriptContent.AppendLine(GetTab(tabCount) + "}");
            return scriptContent.ToString();
            /* 内容为
            public System.Collections.Generic.Dictionary<KEYTYPE, VALUETYPE> dataSet;

            public void SetData(System.Collections.Generic.List<object> dataSetSource)
            {
                dataSet = new System.Collections.Generic.Dictionary<KEYTYPE, VALUETYPE>();
                dataSetEditor = new System.Collections.Generic.List<VALUETYPE>();
                foreach (var data in dataSetSource)
                {
                    var fields = data.GetType().GetFields();
                    foreach (var field in fields)
                    {
                        var find = false;
                        foreach (var _interface in field.FieldType.GetInterfaces())
                        {
                            if (_interface.FullName.Equals(typeof(IDictionaryKey).FullName))
                            {
                                var fieldValue = (field.GetValue(data) as IProperty<KEYTYPE>).Value;
                                dataSet.Add(fieldValue, data as VALUETYPE);
                                dataSetEditor.Add(data as VALUETYPE);
                                find = true;
                                break;
                            }
                        }
                        if (find) break;
                    }
                }
            }
            */
        }
        #endregion


        #region private method

        #endregion


        #region static

        #endregion
    }
}