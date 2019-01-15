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
        /// string: 文件路径
        /// CreateData: 创建信息
        /// </summary>
        private Dictionary<string, CreateData> createDataMap;
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
            createDataMap = new Dictionary<string, CreateData>();
        }
        private void OnGUI()
        {
			#region 输入区域
            var splitLineWidth = 2f;
            var inputArea = new Rect(Margin / 2, Margin / 2, position.width - previewMinWidth - Margin - Margin - splitLineWidth, position.height - Margin);
            var splitLineArea = new Rect(inputArea.xMax + Margin / 2, 0, splitLineWidth, position.height);
            EditorGUI.DrawRect(splitLineArea, new Color(0, 0, 0, 0.6f));
            EditorGUI.DrawRect(inputArea, Color.cyan);
            // GUILayout.BeginArea()
			添加创建数据
			#endregion 输入区域

			#region 预览区域
            var previewArea = new Rect(inputMinWidth + Margin / 2, Margin / 2, position.width - inputMinWidth - Margin, position.height - Margin);
            EditorGUI.DrawRect(previewArea, new Color(0, 0, 0, 0.4f));
			#endregion 预览区域
        }
    }
}