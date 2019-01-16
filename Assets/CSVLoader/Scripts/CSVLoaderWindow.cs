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
        private Dictionary<int, CreateData> createDataMap;
        private Vector2 miniSize;
        /// <summary>
        /// 输入窗口最小宽度
        /// </summary>
        private const float inputMinWidth = 500f;
        /// <summary>
        /// 预览窗口最小宽度
        /// </summary>
        private const float previewMinWidth = 400f;
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
        private float inputHeight;
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
            createDataMap = new Dictionary<int, CreateData>();
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
            var scrollViewRect = new Rect(inputArea.x, inputArea.y - scrollViewOffsetHeight, inputArea.width, createDataMap.Count * LINE_HEIGHT + scrollViewOffsetHeight);
            inputScrollPosition = GUI.BeginScrollView(scrollRect, inputScrollPosition, scrollViewRect, false, false, GUIStyle.none, GUIStyle.none);
            inputHeight = 0f;
            foreach (var data in createDataMap.Values)
            {
                var buttonWidth = 80f;
                var fieldWidth = inputArea.width * .8f;
                var foldoutRect = new Rect(inputArea.x, inputHeight, fieldWidth, LINE_HEIGHT);
                var removeButtonRect = new Rect(foldoutRect.xMax + Margin, inputHeight, buttonWidth, BUTTON_HEIGHT);
                var foldoutTitle = "未选择文件";
                if (!string.IsNullOrEmpty(data.loadFilePath))
                {
                    foldoutTitle = new FileInfo(data.loadFilePath).Name.Split('.')[0];
                }
                data.foldout = EditorGUI.Foldout(foldoutRect, data.foldout, foldoutTitle, true);
                if (GUI.Button(removeButtonRect, "x"))
                {
                    createDataMap.Remove(data.createIndex);
                    break;
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
                        if (loadedFilePath.Contains(selectedPath))
                        {
                            Debug.LogError("文件已经被加载");
                            continue;
                        }
                        var saveFilePath = data.loadFilePath;
                        if (!string.IsNullOrEmpty(saveFilePath) && loadedFilePath.Contains(saveFilePath)) loadedFilePath.Remove(saveFilePath);
                        data.loadFilePath = selectedPath;
                    }
                    AddHeight(1);
                    EditorGUI.BeginDisabledGroup(true);
                    var createTypeRect = new Rect(inputArea.x, inputHeight, fieldWidth, LINE_HEIGHT);
                    EditorGUI.EnumPopup(createTypeRect, "数据类型", data.createType);
                    EditorGUI.EndDisabledGroup();
                    var previewButtonRect = new Rect(createTypeRect.xMax + Margin, inputHeight, buttonWidth, BUTTON_HEIGHT);
                    EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(data.loadFilePath));
                    if (GUI.Button(previewButtonRect, "预览脚本"))
                    {
                        PreviewScript(data);
                    }
                    EditorGUI.EndDisabledGroup();
                    AddHeight(0);
                    if (!string.IsNullOrEmpty(data.loadFilePath))
                    {
                        AddSpace();
                        var scriptRect = new Rect(inputArea.x, inputHeight, fieldWidth, LINE_HEIGHT);
                        var scriptButtonRect = new Rect(scriptRect.xMax + Margin, inputHeight, buttonWidth, BUTTON_HEIGHT);
                        var fileInfo = new FileInfo(data.loadFilePath);
                        data.scriptNameSpace = EditorGUI.TextField(scriptRect, "namespace", data.scriptNameSpace);
                        if (GUI.Button(scriptButtonRect, "脚本路径"))
                        {
                            var saveScriptFilePath = EditorUtility.SaveFilePanel("保存脚本", "", $"{fileInfo.Name.Split('.')[0]}.cs", "cs");
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
                            var saveResourceFilePath = EditorUtility.SaveFilePanel("保存资源", data.scriptFilePath, $"{fileInfo.Name.Split('.')[0]}.cs", "cs");
                            data.resourceFilePath = saveResourceFilePath;
                        }
                        AddHeight(0);
                    }
                    EditorGUI.indentLevel--;
                }
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
                createDataMap.Add(CreateIndex, new CreateData(CreateIndex) { foldout = true });
                CreateIndex++;
            }
            var createEnable = createDataMap.Count > 0;
            foreach (var data in createDataMap.Values) createEnable &= data.prepareComplete;
            EditorGUI.BeginDisabledGroup(!createEnable);
            if (GUI.Button(generateButtonRect, "创建"))
            {
            }
            EditorGUI.EndDisabledGroup();
            #endregion 输入区域

            #region 预览区域
            var previewArea = new Rect(inputMinWidth + Margin / 2, Margin / 2, position.width - inputMinWidth - Margin, position.height - Margin);
            EditorGUI.DrawRect(previewArea, new Color(0, 0, 0, 0.4f));
            #endregion 预览区域
        }

        /// <summary>
        /// 预览脚本
        /// </summary>
        /// <param name="data"></param>
        private void PreviewScript(CreateData data)
        {

        }

        private void AddHeight(int space)
        {
            inputHeight += LINE_HEIGHT;
            inputHeight += space;
        }
        private void AddSpace(int height = 1) => inputHeight += height;

        private static int CreateIndex = int.MinValue;
    }
}