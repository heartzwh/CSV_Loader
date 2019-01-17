//Author: sora

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace Sora.Tools.CSVLoader
{
    public class CSVLoaderWindow : EditorWindow
    {
        /// <summary>
        /// 文件保存位置
        /// </summary>
        private string lastSaveFilePath;
        /// <summary>
        /// 需要创建的文件信息
        /// int: 创建id
        /// CreateData: 创建信息
        /// </summary>
        private Dictionary<int, GenerateData> createDataMap;
        private Vector2 miniSize;
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
        private const float LINE_HEIGHT = 20f;
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
        [MenuItem("Sora/Tools/CSVLoader")]
        static void Open()
        {
            var window = GetWindow(typeof(CSVLoaderWindow), false, "CSV Loader", true);
            window.Show();
            window.minSize = new Vector2(inputMinWidth + previewMinWidth + Margin, (inputMinWidth + Margin + previewMinWidth) * .75f);
            window.maxSize = window.minSize;
        }
        private void OnEnable()
        {
            createDataMap = new Dictionary<int, GenerateData>();
            previewScriptContent = string.Empty;
        }
        private void OnGUI()
        {
            #region 输入区域
            var splitLineWidth = 2f;
            var inputArea = new Rect(Margin / 2, Margin / 2, position.width - previewMinWidth - Margin - Margin - splitLineWidth, position.height - Margin);
            var splitLineArea = new Rect(inputArea.xMax + Margin / 2, 0, splitLineWidth, position.height);
            EditorGUI.DrawRect(splitLineArea, new Color(0, 0, 0, 0.6f));
            var scrollViewOffsetHeight = 10f;
            var scrollRect = new Rect(inputArea.x, inputArea.y, inputArea.width, inputArea.height - Margin - splitLineWidth - BUTTON_HEIGHT);
            var scrollViewRect = new Rect(inputArea.x, inputArea.y - scrollViewOffsetHeight, inputArea.width, inputHeight);
            inputScrollPosition = GUI.BeginScrollView(scrollRect, inputScrollPosition, scrollViewRect, false, false, GUIStyle.none, GUIStyle.none);
            inputHeight = 0f;
            var dataIndex = 0;
            var dataCopy = new List<GenerateData>(createDataMap.Values);
            dataCopy.Sort((x, y) =>
            {
                if (x.createIndex > y.createIndex) return -1;
                else if (x.createIndex < y.createIndex) return 1;
                else return 0;
            });
            foreach (var data in dataCopy)
            {
                if (!data.blockHeight.Equals(0))
                {
                    var blockRect = new Rect(inputArea.x, inputHeight, inputArea.width, data.blockHeight);
                    EditorGUI.DrawRect(blockRect, data.blockColor);
                }
                var recordHeight = inputHeight;
                var buttonWidth = 80f;
                var fieldWidth = inputArea.width * .8f;
                var foldoutRect = new Rect(inputArea.x, inputHeight, fieldWidth, LINE_HEIGHT);
                var removeButtonRect = new Rect(foldoutRect.xMax + Margin, inputHeight, buttonWidth, BUTTON_HEIGHT);
                var foldoutTitle = "未选择文件";
                if (!string.IsNullOrEmpty(data.loadFilePath))
                {
                    foldoutTitle = $"{data.fileInfo.Name}[{data.scriptData.scriptSetting.scriptFullname}]";
                }
                data.foldout = EditorGUI.Foldout(foldoutRect, data.foldout, foldoutTitle, true);
                if (GUI.Button(removeButtonRect, "x"))
                {
                    var removeFlag = false;
                    if (string.IsNullOrEmpty(data.loadFilePath))
                    {
                        removeFlag = true;
                    }
                    else
                    {
                        if (EditorUtility.DisplayDialog("删除资源", $"删除{data.fileInfo.Name}", "确定", "取消")) removeFlag = true;
                    }
                    if (removeFlag)
                    {
                        createDataMap.Remove(data.createIndex);
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
                            previewScriptContent = data.scriptData.scriptContent;
                        }
                    }
                    AddHeight(1);
                    if (!string.IsNullOrEmpty(data.loadFilePath))
                    {
                        AddSpace();
                        var scriptRect = new Rect(inputArea.x, inputHeight, fieldWidth, LINE_HEIGHT);
                        var scriptButtonRect = new Rect(scriptRect.xMax + Margin, inputHeight, buttonWidth, BUTTON_HEIGHT);
                        var fileInfo = new FileInfo(data.loadFilePath);
                        EditorGUI.BeginDisabledGroup(true);
                        EditorGUI.TextField(scriptRect, "脚本路径", data.scriptFilePath);
                        EditorGUI.EndDisabledGroup();
                        if (GUI.Button(scriptButtonRect, "脚本路径"))
                        {
                            var saveScriptFilePath = EditorUtility.SaveFolderPanel("保存脚本位置", "", "");
                            data.scriptFilePath = saveScriptFilePath;
                        }
                        AddHeight(1);
                        var resourceRect = new Rect(inputArea.x, inputHeight, fieldWidth, LINE_HEIGHT);
                        var resourceButtonRect = new Rect(resourceRect.xMax + Margin, inputHeight, buttonWidth, BUTTON_HEIGHT);
                        EditorGUI.BeginDisabledGroup(true);
                        EditorGUI.TextField(resourceRect, "资源路径", data.resourceFilePath);
                        EditorGUI.EndDisabledGroup();
                        if (GUI.Button(resourceButtonRect, "资源路径"))
                        {
                            var saveResourceFilePath = EditorUtility.SaveFolderPanel("保存资源位置", "", "");
                            data.resourceFilePath = saveResourceFilePath;
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
            var addButtonRect = new Rect(inputArea.x, splitLineArea.yMax + Margin / 2, 50, BUTTON_HEIGHT);
            var generateButtonRect = new Rect(addButtonRect.xMax + Margin, splitLineArea.yMax + Margin / 2, 50, BUTTON_HEIGHT);
            if (GUI.Button(addButtonRect, "+"))
            {
                createDataMap.Add(CreateIndex, new GenerateData(CreateIndex) { foldout = true });
                CreateIndex++;
            }
            var createEnable = createDataMap.Count > 0;
            foreach (var data in createDataMap.Values) createEnable &= data.prepareComplete;
            EditorGUI.BeginDisabledGroup(!createEnable);
            if (GUI.Button(generateButtonRect, "创建")) GenerateResource();
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
                EditorGUILayout.TextArea(previewScriptContent);
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
        /// 创建资源
        /// 脚本/ScriptObject
        /// </summary>
        private void GenerateResource()
        {
            foreach (var data in createDataMap.Values)
            {
                data.scriptData.GenerateScript();
            }
        }
        private static int CreateIndex = int.MinValue;
    }
}