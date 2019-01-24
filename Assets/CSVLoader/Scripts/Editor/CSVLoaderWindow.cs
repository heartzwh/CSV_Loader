//Author: sora

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace Sora.Tools.CSVLoader.Editor
{
    public class CSVLoaderWindow : EditorWindow
    {
        /// <summary>
        /// 获取editor之外的assembly,在editor中调用程序集只能获取editor内方法
        /// </summary>
        public Assembly assembly { get; private set; }
        /// <summary>
        /// 需要创建的文件信息
        /// int: 创建id
        /// CreateData: 创建信息
        /// </summary>
        private Dictionary<int, GenerateData> generateDataMap;
        /// <summary>
        /// 输入窗口最小宽度
        /// </summary>
        private const float inputMinWidth = 500f;
        /// <summary>
        /// 预览窗口最小宽度
        /// </summary>
        private const float previewMinWidth = 300f;
        /// <summary>
        /// 边距
        /// </summary>
        private const float Margin = 10f;
        /// <summary>
        /// 每行高度
        /// </summary>
        private const float LINE_HEIGHT = 20f;
        /// <summary>
        /// 按钮高度
        /// </summary>
        private const float BUTTON_HEIGHT = 18f;
        /// <summary>
        /// 已经加载的文件路径
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <returns></returns>
        private HashSet<string> loadedFilePath = new HashSet<string>();
        private Vector2 inputScrollPosition;
        private Vector2 previewScrollPosition;
        private float inputHeight;
        private string previewScriptContent;
        private float previewHeight;
        private float previewSaveHeight;
        private GUIStyle previewTextAreaStyle;
        private bool tipNoSaveFilePathErrorFlag = false;
        /// <summary>
        /// 保存文件存储位置的文件位置
        /// </summary>
        /// <value></value>
        public string csvSavePathFileRootPath
        {
            get => m_csvSavePathFileRootPath;
            set
            {
                if (m_csvSavePathFileRootPath != value)
                {
                    var filePath = $"{UnityEngine.Application.dataPath}{CSVLoaderWindow.Seperator()}CSVLoader/Scripts/Editor/SavePathFile.txt";
                    if (!File.Exists(filePath)) File.CreateText(filePath);
                    var writeFileSteam = new StreamWriter(filePath);
                    writeFileSteam.Write(value);
                    writeFileSteam.Flush();
                    writeFileSteam.Close();
                    AssetDatabase.Refresh();
                    m_csvSavePathFileRootPath = $"{Helper.DataPathWithoutAssets()}{Seperator()}{value}";
                }
            }
        }
        private string m_csvSavePathFileRootPath;
        [MenuItem("Sora/Tools/CSVLoader")]
        static void Open()
        {
            window = GetWindow(typeof(CSVLoaderWindow), false, "CSV Loader", true) as CSVLoaderWindow;
            window.autoRepaintOnSceneChange = false;
            window.wantsMouseMove = false;
            window.Show();
            window.minSize = new Vector2(inputMinWidth + previewMinWidth + Margin, (inputMinWidth + Margin + previewMinWidth) * .75f);
            window.maxSize = window.minSize;
        }
        private void OnEnable()
        {
            LoadSaveFilePathFile();
            window = this;
            generateDataMap = new Dictionary<int, GenerateData>();
            previewScriptContent = string.Empty;

            previewTextAreaStyle = new GUIStyle();
            previewTextAreaStyle.wordWrap = true;

            assembly = typeof(Sora.Tools.CSVLoader.IProperty).Assembly;
        }
        private void OnGUI()
        {
            #region 输入区域
            var splitLineWidth = 2f;
            var buttonWidth = 80f;
            var buttonHalfWidth = (buttonWidth - Margin / 2) / 2;
            var inputArea = new Rect(Margin / 2, Margin / 2, position.width - previewMinWidth - Margin - Margin - splitLineWidth, position.height - Margin);
            var splitLineArea = new Rect(inputArea.xMax + Margin / 2, 0, splitLineWidth, position.height);
            EditorGUI.DrawRect(splitLineArea, new Color(0, 0, 0, 0.6f));
            var scrollViewOffsetHeight = 10f;
            var scrollRect = new Rect(inputArea.x, inputArea.y, inputArea.width, inputArea.height - Margin - splitLineWidth - BUTTON_HEIGHT - LINE_HEIGHT - Margin / 2);
            var scrollViewRect = new Rect(inputArea.x, inputArea.y - scrollViewOffsetHeight, inputArea.width, inputHeight);
            inputScrollPosition = GUI.BeginScrollView(scrollRect, inputScrollPosition, scrollViewRect, false, false, GUIStyle.none, GUIStyle.none);
            inputHeight = 0f;
            var fieldWidth = inputArea.width * .8f;
            var dataIndex = 0;
            var dataCopy = new List<GenerateData>(generateDataMap.Values);
            dataCopy.Sort((x, y) =>
            {
                if (x.createIndex > y.createIndex) return -1;
                else if (x.createIndex < y.createIndex) return 1;
                else return 0;
            });
            foreach (var data in dataCopy)
            {
                if (data.sameFileFlag)
                {
                    data.sameFileFlag = false;
                    data.SetState(BlockState.NORMAL);
                }
                foreach (var dataCheckSame in dataCopy)
                {
                    if (data.createIndex.Equals(dataCheckSame.createIndex)) continue;
                    if (string.IsNullOrEmpty(data.loadFilePath)) continue;
                    if (string.IsNullOrEmpty(dataCheckSame.loadFilePath)) continue;
                    if (dataCheckSame.loadFilePath.Equals(data.loadFilePath))
                    {
                        data.sameFileFlag = true;
                        dataCheckSame.sameFileFlag = true;
                        data.SetState(BlockState.ERROR, false);
                        dataCheckSame.SetState(BlockState.ERROR, false);
                    }
                }
                if (!data.blockHeight.Equals(0))
                {
                    var blockRect = new Rect(inputArea.x, inputHeight, inputArea.width, data.blockHeight);
                    EditorGUI.DrawRect(blockRect, data.blockColor);
                }
                var recordHeight = inputHeight;
                var foldoutRect = new Rect(inputArea.x, inputHeight, fieldWidth, LINE_HEIGHT);
                var previewButtonRect = new Rect(foldoutRect.xMax + Margin, inputHeight, buttonHalfWidth, BUTTON_HEIGHT);
                var removeButtonRect = new Rect(previewButtonRect.xMax + Margin / 2, inputHeight, buttonHalfWidth, BUTTON_HEIGHT);
                var foldoutTitle = "未选择文件";
                if (!string.IsNullOrEmpty(data.loadFilePath))
                {
                    foldoutTitle = $"{data.csvFileInfo.Name}[{data.scriptSetting.scriptFullname}]";
                }
                data.foldout = EditorGUI.Foldout(foldoutRect, data.foldout, foldoutTitle, true);
                EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(data.loadFilePath));
                if (GUI.Button(previewButtonRect, "预览"))
                {
                    PreviewScriptContent(data);
                }
                EditorGUI.EndDisabledGroup();
                if (GUI.Button(removeButtonRect, "x"))
                {
                    var removeFlag = false;
                    if (string.IsNullOrEmpty(data.loadFilePath))
                    {
                        removeFlag = true;
                    }
                    else
                    {
                        if (EditorUtility.DisplayDialog("删除资源", $"删除{data.csvFileInfo.Name}", "确定", "取消")) removeFlag = true;
                    }
                    if (removeFlag)
                    {
                        generateDataMap.Remove(data.createIndex);
                        break;
                    }
                }
                inputHeight += LINE_HEIGHT;
                if (data.foldout)
                {
                    EditorGUI.indentLevel++;
                    var filePathRect = new Rect(inputArea.x, inputHeight, fieldWidth, LINE_HEIGHT);
                    var filePathButtonRect = new Rect(filePathRect.xMax + Margin, inputHeight, buttonWidth, BUTTON_HEIGHT);
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUI.TextField(filePathRect, "选择路径", data.loadFilePath);
                    EditorGUI.EndDisabledGroup();
                    if (GUI.Button(filePathButtonRect, "选择CSV"))
                    {
                        var selectedPath = EditorUtility.OpenFilePanelWithFilters("选择文件", "", new string[] { "CSV文件", "csv" });
                        if (!string.IsNullOrEmpty(selectedPath))
                        {
                            if (loadedFilePath.Contains(selectedPath))
                            {
                                Debug.LogError("文件已经被加载");
                                continue;
                            }
                            var saveFilePath = data.loadFilePath;
                            if (!string.IsNullOrEmpty(saveFilePath) && loadedFilePath.Contains(saveFilePath)) loadedFilePath.Remove(saveFilePath);
                            data.loadFilePath = selectedPath;
                            PreviewScriptContent(data);
                        }
                    }
                    AddHeight(1);
                    if (!string.IsNullOrEmpty(data.loadFilePath) && !data.generateScriptFlag)
                    {
                        AddSpace();
                        var resourceRect = new Rect(inputArea.x, inputHeight, fieldWidth, LINE_HEIGHT);
                        var selectObject = EditorGUI.ObjectField(resourceRect, "资源文件", data.scriptAssetData.assetObject, typeof(ICSVLoaderAsset), false) as ScriptableObject;
                        if (
                            (selectObject == null) ||
                            (data.scriptAssetData.assetObject == null) ||
                            (data.scriptAssetData.assetObject != null && selectObject != null && !data.scriptAssetData.assetObject.GetInstanceID().Equals(selectObject.GetInstanceID()))
                        )
                        {
                            var ok = data.scriptAssetData.SetAssetObject(selectObject);
                            if (ok)
                            {
                                data.resourceFilePath = AssetDatabase.GetAssetPath(selectObject);
                            }
                            else data.resourceFilePath = string.Empty;
                        }
                        AddHeight(0);
                    }
                    EditorGUI.indentLevel--;
                }
                if (dataIndex < dataCopy.Count - 1)
                {
                    AddSpace();
                    splitLineArea = new Rect(inputArea.x, inputHeight, inputArea.width, 1);
                    EditorGUI.DrawRect(splitLineArea, new Color(0, 0, 0, 0.6f));
                    AddSpace(4);
                }
                data.blockHeight = inputHeight - recordHeight;
                dataIndex++;
            }
            GUI.EndScrollView();
            AddSpace(10);
            var lastRectHeight = inputHeight;
            if (lastRectHeight > scrollRect.yMax)
            {
                lastRectHeight = scrollRect.yMax;
            }
            splitLineArea = new Rect(inputArea.x, lastRectHeight + Margin / 2, inputArea.width, splitLineWidth);
            EditorGUI.DrawRect(splitLineArea, new Color(0, 0, 0, 0.6f));

            var csvFileDirectoryRect = new Rect(inputArea.x, splitLineArea.yMax + Margin / 2, fieldWidth, LINE_HEIGHT);
            var csvFileDirectoryBurronRect = new Rect(csvFileDirectoryRect.xMax + Margin, csvFileDirectoryRect.y, buttonWidth, BUTTON_HEIGHT);
            if (tipNoSaveFilePathErrorFlag)
            {
                EditorGUI.DrawRect(new Rect(csvFileDirectoryRect.x, csvFileDirectoryRect.y, inputArea.width, LINE_HEIGHT), new Color(255, 0, 0, 0.5f));
                DelayCall(1, () =>
                {
                    tipNoSaveFilePathErrorFlag = false;
                });
            }
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.TextField(csvFileDirectoryRect, "文件保存位置", csvSavePathFileRootPath);
            EditorGUI.EndDisabledGroup();
            if (GUI.Button(csvFileDirectoryBurronRect, "选择路径"))
            {
                var selectPath = EditorUtility.SaveFolderPanel("保存资源位置", csvSavePathFileRootPath, "");
                if (!string.IsNullOrEmpty(selectPath))
                {
                    csvSavePathFileRootPath = Helper.GetUnityAssetPath(selectPath);
                }
            }

            var addButtonRect = new Rect(inputArea.x, csvFileDirectoryRect.yMax + Margin / 2, buttonHalfWidth, BUTTON_HEIGHT);
            var generateButtonRect = new Rect(addButtonRect.xMax + Margin, addButtonRect.y, buttonHalfWidth, BUTTON_HEIGHT);
            if (GUI.Button(addButtonRect, "+"))
            {
                generateDataMap.Add(GenerateIndex, new GenerateData(GenerateIndex) { foldout = true });
                GenerateIndex++;
            }
            var createEnable = generateDataMap.Count > 0;
            foreach (var data in generateDataMap.Values) createEnable &= data.prepareComplete;
            EditorGUI.BeginDisabledGroup(!createEnable);
            if (GUI.Button(generateButtonRect, "GO!")) GenerateResource();
            EditorGUI.EndDisabledGroup();
            #endregion 输入区域

            #region 预览区域
            if (!string.IsNullOrEmpty(previewScriptContent))
            {
                if (previewHeight < previewSaveHeight) previewHeight = previewSaveHeight;
                else previewSaveHeight = previewHeight;
                var previewRect = new Rect(inputMinWidth + Margin / 2, Margin / 2, position.width - inputMinWidth - Margin, position.height - Margin);
                GUILayout.BeginArea(previewRect);
                var previewScrollRect = new Rect(0, 0, previewRect.width, previewRect.height);
                var previewScrollViewRect = new Rect(0, 0, previewScrollRect.width, previewHeight);
                previewScrollPosition = GUI.BeginScrollView(previewScrollRect, previewScrollPosition, previewScrollViewRect, false, false, GUIStyle.none, GUIStyle.none);
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.TextArea(previewScriptContent, previewTextAreaStyle);
                EditorGUI.EndDisabledGroup();
                previewHeight = GUILayoutUtility.GetLastRect().yMax;
                GUI.EndScrollView();
                GUILayout.EndArea();
            }
            #endregion 预览区域
        }
        private void AddHeight(int space)
        {
            inputHeight += LINE_HEIGHT;
            inputHeight += space;
        }
        private void AddSpace(int height = 1) => inputHeight += height;
        /// <summary>
        /// 创建脚本
        /// c#脚本与ScriptObject脚本
        /// </summary>
        private void GenerateResource()
        {
            var ok = true;
            if (string.IsNullOrEmpty(csvSavePathFileRootPath))
            {
                window.ShowNotification(new GUIContent("设置保存路径"));
                tipNoSaveFilePathErrorFlag = true;
                ok = false;
            }
            foreach (var data in generateDataMap.Values)
            {
                ok &= data.CheckSettingComplete();
            }
            if (!ok) return;
            /* 全部资源都是加载,不需要生成脚本 */
            /* 如果不需要生成脚本就不能触发 [DidReloadScripts] ,需要手动调用 */
            var isAllLoad = true;
            foreach (var data in generateDataMap.Values)
            {
                isAllLoad &= !data.generateScriptFlag;
                if (!isAllLoad) break;
            }
            if (!isAllLoad)
            {
                var dataRecorderMap = new List<GenreateDataRecorder>();
                foreach (var generateData in generateDataMap.Values)
                {
                    var dataRecorder = new GenreateDataRecorder();
                    dataRecorder.loadFilePath = generateData.loadFilePath;
                    dataRecorder.resourceFilePath = generateData.resourceFilePath;
                    dataRecorderMap.Add(dataRecorder);
                }
                EditorPrefs.SetString("Data", SerializeToXml(dataRecorderMap));
                foreach (var data in generateDataMap.Values)
                {
                    data.scriptData.GenerateScript();
                    data.scriptAssetData.GenerateScript();
                }
                AssetDatabase.Refresh();
            }
            else
            {
                var generateDataList = new List<GenerateData>();
                foreach (var dataRecorder in generateDataMap.Values)
                {
                    var generateData = new GenerateData(0);
                    generateData.loadFilePath = dataRecorder.loadFilePath;
                    generateData.resourceFilePath = dataRecorder.resourceFilePath;
                    generateDataList.Add(generateData);
                }
                SetAssetData(generateDataList);
            }
        }
        /// <summary>
        /// 生成预览脚本
        /// </summary>
        /// <param name="data"></param>
        private void PreviewScriptContent(GenerateData data)
        {
            var scriptContent = new System.Text.StringBuilder();
            scriptContent.Append(data.scriptData.scriptContent);
            scriptContent.AppendLine();
            scriptContent.AppendLine();
            scriptContent.Append(data.scriptAssetData.scriptContent);
            previewScriptContent = scriptContent.ToString();
        }
        /// <summary>
        /// 加载保存文件路径的文件
        /// </summary>
        private void LoadSaveFilePathFile()
        {
            var saveFilePath = string.Empty;
            var filePath = $"{UnityEngine.Application.dataPath}{CSVLoaderWindow.Seperator()}CSVLoader/Scripts/Editor/SavePathFile.txt";
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
                AssetDatabase.Refresh();
            }
            else
            {
                var readFileStream = new StreamReader(filePath);
                saveFilePath = readFileStream.ReadLine();
                readFileStream.Close();
            }
            if (!string.IsNullOrEmpty(saveFilePath))
            {
                m_csvSavePathFileRootPath = $"{Helper.DataPathWithoutAssets()}{Seperator()}{saveFilePath}";
            }
            else m_csvSavePathFileRootPath = string.Empty;
        }
        private async void DelayCall(float time, System.Action completeAction)
        {
            await Task.Yield();
            await Task.Delay(System.TimeSpan.FromSeconds(time));
            completeAction?.Invoke();
        }
        /// <summary>
        /// 将对象序列化为XML数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeToXml(object obj)
        {
            MemoryStream stream = new MemoryStream();
            XmlSerializer xs = new XmlSerializer(obj.GetType());
            xs.Serialize(stream, obj);
            byte[] data = stream.ToArray();
            stream.Close();
            return System.Text.Encoding.Default.GetString(data);
        }
        /// <summary>
        /// 将XML数据反序列化为指定类型对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T DeserializeWithXml<T>(string dataStr)
        {
            byte[] data = System.Text.Encoding.Default.GetBytes(dataStr);
            MemoryStream stream = new MemoryStream();
            stream.Write(data, 0, data.Length);
            stream.Position = 0;
            XmlSerializer xs = new XmlSerializer(typeof(T));
            object obj = xs.Deserialize(stream);
            stream.Close();
            return (T)obj;
        }
        /// <summary>
        /// 当脚本加载完成后,生成保存数据的资源
        /// </summary>
        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnReloadScripts()
        {
            var data = EditorPrefs.GetString("Data");
            EditorPrefs.DeleteKey("Data");
            if (!string.IsNullOrEmpty(data))
            {
                var dataRecorderMap = DeserializeWithXml<List<GenreateDataRecorder>>(data);
                if (dataRecorderMap != null && dataRecorderMap.Count > 0)
                {
                    var generateDataMap = new List<GenerateData>();
                    foreach (var dataRecorder in dataRecorderMap)
                    {
                        var generateData = new GenerateData(0);
                        generateData.loadFilePath = dataRecorder.loadFilePath;
                        generateData.resourceFilePath = dataRecorder.resourceFilePath;
                        generateDataMap.Add(generateData);
                    }
                    SetAssetData(generateDataMap);
                }
            }
        }
        private static void SetAssetData(List<GenerateData> generateDataMap)
        {
            /* 生成/加载asset object */
            foreach (var generateData in generateDataMap)
            {
                generateData.scriptAssetData.GenerateAsset();
            }
            /* 初始化asset数据 */
            foreach (var generateData in generateDataMap)
            {
                generateData.scriptAssetData.scriptAsset.InitData();
                EditorUtility.SetDirty(generateData.scriptAssetData.assetObject);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        public static CSVLoaderWindow window;
        public static char Seperator()
        {
            char separator = '/';
#if UNITY_STANDALONE_OSX
            separator = '/';
#elif UNITY_STANDALONE_WIN
            separator = '\\';
#endif
            return separator;
        }

        private static int GenerateIndex = int.MinValue;
    }
    /// <summary>
    /// 创建的资源csv需要的数据
    /// </summary>
    [System.Serializable]
    public class GenreateDataRecorder
    {
        public string loadFilePath;
        public string resourceFilePath;
    }
}