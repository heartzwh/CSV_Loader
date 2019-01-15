//Author: sora

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

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
        private const float LINE_HEIGHT = 26f;
        private const float BUTTON_HEIGHT = 20f;
        /// <summary>
        /// 已经加载的文件路径
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <returns></returns>
        private HashSet<string> loadedFilePath = new HashSet<string>();
        private Vector2 inputScrollPosition;
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
            EditorGUI.DrawRect(inputArea, Color.cyan);
            var scrollViewOffsetHeight = 10f;
            var scrollRect = new Rect(inputArea.x, inputArea.y, inputArea.width, inputArea.height - Margin - splitLineWidth - BUTTON_HEIGHT);
            var scrollViewRect = new Rect(inputArea.x, inputArea.y - scrollViewOffsetHeight, inputArea.width, createDataMap.Count * LINE_HEIGHT + scrollViewOffsetHeight);
            inputScrollPosition = GUI.BeginScrollView(scrollRect, inputScrollPosition, scrollViewRect, false, false, GUIStyle.none, GUIStyle.none);
            var dataIndex = 0;
            foreach (var data in createDataMap.Values)
            {
                var filePathRect = new Rect(inputArea.x, inputArea.y + dataIndex * LINE_HEIGHT, 200, LINE_HEIGHT);
                var filePathButtonRect = new Rect(filePathRect.xMax, 0, 80f, BUTTON_HEIGHT);
                EditorGUI.LabelField(filePathRect, "CSV文件路径", data.loadFilePath);
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
                    data.loadFilePath = saveFilePath;
                }
                dataIndex++;
            }
            GUI.EndScrollView();
            var lastRectHeight = 0f;
            if (scrollViewRect.yMax > scrollRect.yMax)
            {
                lastRectHeight = scrollRect.yMax;
            }
            else
            {
                lastRectHeight = scrollViewRect.yMax;
            }
            splitLineArea = new Rect(inputArea.x, lastRectHeight + Margin / 2, inputArea.width, splitLineWidth);
            EditorGUI.DrawRect(splitLineArea, new Color(0, 0, 0, 0.6f));
            var addButtonRect = new Rect(inputArea.x, splitLineArea.yMax + Margin / 2, 50, BUTTON_HEIGHT);
            if (GUI.Button(addButtonRect, "+"))
            {
                createDataMap.Add(CreateIndex, new CreateData(CreateIndex));
                CreateIndex++;
            }
            #endregion 输入区域

            #region 预览区域
            var previewArea = new Rect(inputMinWidth + Margin / 2, Margin / 2, position.width - inputMinWidth - Margin, position.height - Margin);
            EditorGUI.DrawRect(previewArea, new Color(0, 0, 0, 0.4f));
            #endregion 预览区域
        }
        private static int CreateIndex = int.MinValue;
    }
}