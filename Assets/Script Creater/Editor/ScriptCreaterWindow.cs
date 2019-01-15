//Author:	sora

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

namespace Sora.Tools.ScriptCreater
{
    public class ScriptCreaterWindow : EditorWindow
    {
        private ScriptCreaterEnums.ScriptType selectScriptType;
        private ScriptCreaterEnums.ClassScriptType classScriptType;
        private ScriptCreaterEnums.MonoScriptType monoScriptType;
        private ScriptCreaterEnums.ECSSystemType systemType;
        private ScriptCreaterEnums.ECSComponentType componentType;
        private int selectNamespaceIndex = 0;
        private static string seletFolder = "";
        private static string scriptTempName = "";

        private static List<string> namesapceList;
        private static string namespaceFilePath;
        private static string selectFolderFilePath;
        private static string authorNameFilePath;
        private static string headerFilePath;
        private string scriptNameSuffix = "";
        private string scriptFileName = "";//scriptname.cs
        private string scriptFullFileName = "";//path/scirpt.cs
        private string scriptName = "";//scriptname
        private string scriptFullName = "";//namespace.scriptname


        [MenuItem("Sora/Tools/ScriptCreater")]
        static void Open()
        {
            var window = GetWindow(typeof(ScriptCreaterWindow), false, "Script Creater", true);
            window.Show();
        }

        [MenuItem("Assets/设置路径")]
        static void SetScriptPath()
        {
            var selectObjGUID = Selection.assetGUIDs;
            if (selectObjGUID.Length != 1) return;
            var path = AssetDatabase.GUIDToAssetPath(selectObjGUID[0]);
            path = path.Remove(0, 7);
            seletFolder = string.Format("{1}{0}{2}", Seperator(), Application.dataPath, path);
            /* 非文件夹 */
            if (seletFolder.Contains(".")) return;
            SaveSelectFolder();
        }

        private void OnEnable()
        {
            namesapceList = new List<string>();
            namespaceFilePath = string.Format("{1}{2}{0}{3}", Seperator(), RootFolderEditor(), "Datas", "namespace.txt");
            RefreshNamespace();
            if (selectNamespaceIndex >= namesapceList.Count) selectNamespaceIndex = 0;

            selectFolderFilePath = string.Format("{1}{2}{0}{3}", Seperator(), RootFolderEditor(), "Datas", "selectedfolder.txt");
            authorNameFilePath = string.Format("{1}{2}{0}{3}", Seperator(), RootFolderEditor(), "Datas", "authorname.txt");
            headerFilePath = string.Format("{1}{2}{0}{3}", Seperator(), RootFolderEditor(), "Datas", "header.txt");
            LoadSelectFolder();
        }

        private void OnGUI()
        {
            scriptNameSuffix = "";
            /* 选择创建脚本类型 */
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("创建脚本类型: ", GUILayout.Width(80));
            selectScriptType = (ScriptCreaterEnums.ScriptType)EditorGUILayout.EnumPopup(selectScriptType, GUILayout.Width(200));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("选择类型: ", GUILayout.Width(80));
            switch (selectScriptType)
            {
                case ScriptCreaterEnums.ScriptType.Class:
                    classScriptType = (ScriptCreaterEnums.ClassScriptType)EditorGUILayout.EnumPopup(classScriptType, GUILayout.Width(200));
                    break;
                case ScriptCreaterEnums.ScriptType.Mono:
                    monoScriptType = (ScriptCreaterEnums.MonoScriptType)EditorGUILayout.EnumPopup(monoScriptType, GUILayout.Width(200));
                    switch (monoScriptType)
                    {
                        case ScriptCreaterEnums.MonoScriptType.Editor:
                            scriptNameSuffix = "Editor";
                            break;
                    }
                    break;
                case ScriptCreaterEnums.ScriptType.ECSSystem:
                    scriptNameSuffix = "System";
                    systemType = (ScriptCreaterEnums.ECSSystemType)EditorGUILayout.EnumPopup(systemType, GUILayout.Width(200));
                    break;
                case ScriptCreaterEnums.ScriptType.ECSComponent:
                    scriptNameSuffix = "Component";
                    componentType = (ScriptCreaterEnums.ECSComponentType)EditorGUILayout.EnumPopup(componentType, GUILayout.Width(200));
                    break;
            }
            EditorGUILayout.EndHorizontal();
            /* namespace */
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("namespace: ", GUILayout.Width(80));
            if (namesapceList.Count > 0)
            {
                selectNamespaceIndex = EditorGUILayout.Popup(selectNamespaceIndex, namesapceList.ToArray(), GUILayout.Width(200));
            }
            else
            {
                EditorGUILayout.LabelField("NULL", GUILayout.Width(35));
            }
            if (GUILayout.Button("刷新", GUILayout.Width(55))) RefreshNamespace();
            EditorGUILayout.EndHorizontal();
            /* 类名称 */
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("类名称: ", GUILayout.Width(80));
            scriptTempName = EditorGUILayout.TextField(scriptTempName, GUILayout.Width(200));
            EditorGUILayout.EndHorizontal();

            scriptName = "";
            scriptFullName = "";
            /* 显示类全名 */
            var fullNameRect = EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("类全名: ", GUILayout.Width(80));
            if (!string.IsNullOrEmpty(scriptTempName))
            {
                scriptName = scriptTempName + scriptNameSuffix;
                scriptFullName = string.Format("{0}.{1}", namesapceList[selectNamespaceIndex], scriptName);
            }
            else scriptFullName = "NULL";
            EditorGUILayout.LabelField(ShortenPath(scriptFullName, fullNameRect.width - 80));
            EditorGUILayout.EndHorizontal();
            /* 保存位置 */
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("保存路径: ", GUILayout.Width(80));
            if (GUILayout.Button("选择路径", GUILayout.Width(55)))
            {
                seletFolder = EditorUtility.OpenFolderPanel("选择路径", seletFolder, "");
                SaveSelectFolder();
            }
            if (GUILayout.Button("加载路径", GUILayout.Width(55))) seletFolder = LoadSelectFolder();
            EditorGUILayout.EndHorizontal();
            var labelRect = EditorGUILayout.BeginHorizontal();
            scriptFileName = "";
            scriptFullFileName = "";
            EditorGUILayout.LabelField("文件路径: ", GUILayout.Width(80));
            if (!string.IsNullOrEmpty(seletFolder))
            {
                scriptFileName = scriptTempName + scriptNameSuffix;
                scriptFullFileName = string.Format("{0}/{1}.cs", seletFolder, scriptFileName);
            }
            else scriptFullFileName = "NULL";
            EditorGUILayout.LabelField(ShortenPath(scriptFullFileName, labelRect.width - 80));
            EditorGUILayout.EndHorizontal();
            /* 创建脚本 */
            EditorGUILayout.Space();
            if (GUILayout.Button("创建"))
            {
                Create(false);
            }
            if (GUILayout.Button("创建&打开"))
            {
                Create(true);
            }

        }

        private void Create(bool isOpen)
        {
            if (string.IsNullOrEmpty(scriptName) || string.IsNullOrEmpty(seletFolder) || namesapceList.Count == 0)
            {
                return;
            }

            var templateScriptPath = string.Format("{1}{2}{0}", Seperator(), RootFolderEditor(), "Script Template");
            var templateScriptName = "";
            switch (selectScriptType)
            {
                case ScriptCreaterEnums.ScriptType.Class:
                    switch (classScriptType)
                    {
                        case ScriptCreaterEnums.ClassScriptType.Class:
                            templateScriptName = "Class";
                            break;
                        case ScriptCreaterEnums.ClassScriptType.Interface:
                            templateScriptName = "Interface";
                            break;
                        case ScriptCreaterEnums.ClassScriptType.Enum:
                            templateScriptName = "Enum";
                            break;
                        case ScriptCreaterEnums.ClassScriptType.Struct:
                            templateScriptName = "Struct";
                            break;
                    }
                    break;
                case ScriptCreaterEnums.ScriptType.Mono:
                    switch (monoScriptType)
                    {
                        case ScriptCreaterEnums.MonoScriptType.MonoBehaviour:
                            templateScriptName = "MonoBehaviour";
                            break;
                        case ScriptCreaterEnums.MonoScriptType.Editor:
                            templateScriptName = "Editor";
                            break;
                        case ScriptCreaterEnums.MonoScriptType.EditorWindow:
                            templateScriptName = "EditorWindow";
                            break;
                        case ScriptCreaterEnums.MonoScriptType.ScriptableObject:
                            templateScriptName = "ScriptableObject";
                            break;
                    }
                    break;
                case ScriptCreaterEnums.ScriptType.ECSSystem:
                    switch (systemType)
                    {
                        case ScriptCreaterEnums.ECSSystemType.ComponentSystem:
                            templateScriptName = "ComponentSystem";
                            break;
                        case ScriptCreaterEnums.ECSSystemType.JobomponentSystem:
                            templateScriptName = "JobComponentSystem";
                            break;
                    }
                    break;
                case ScriptCreaterEnums.ScriptType.ECSComponent:
                    switch (componentType)
                    {
                        case ScriptCreaterEnums.ECSComponentType.ComponentData:
                            templateScriptName = "ComponentData";
                            break;
                        case ScriptCreaterEnums.ECSComponentType.SharedComponentData:
                            templateScriptName = "SharedComponentData";
                            break;
                    }
                    break;
            }
            templateScriptPath += templateScriptName + ".txt";
            if (!File.Exists(templateScriptPath))
            {
                Debug.Log("模板不存在");
                return;
            }
            var scriptContent = File.ReadAllText(templateScriptPath);
            var changeScriptContent = scriptContent
            .Replace("#AUTHORNAME#", GetAuthorName())
            .Replace("#NAMESPACE#", namesapceList[selectNamespaceIndex])
            .Replace("#SCRIPTNAME#", scriptTempName)
            .Replace("#HEADER#", GetHeader())

            ;
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GetType(scriptFullName) != null)
                {
                    Debug.LogError($"类\"{scriptFullName}\"已存在");
                    return;
                }
            }
            var savePath = string.Format("{1}{0}{2}.cs", Seperator(), seletFolder, scriptName);
            if (File.Exists(savePath))
            {
                Debug.LogError($"文件\"{scriptFileName}\"已存在");
                return;
            }
            StreamWriter sw = File.CreateText(savePath);
            sw.Write(changeScriptContent);
            sw.Flush();
            sw.Close();
            AssetDatabase.Refresh();

            if (isOpen)
            {
                OpenFile(scriptFileName);
            }
        }

        private static void RefreshNamespace()
        {
            namesapceList.Clear();
            var lines = File.ReadAllLines(namespaceFilePath);
            namesapceList.AddRange(lines);
        }

        private static string LoadSelectFolder()
        {
            var lines = File.ReadAllLines(selectFolderFilePath);
            if (lines.Length > 0) return lines[0];
            else return "";
        }

        private static void SaveSelectFolder()
        {
            StreamWriter sw = File.CreateText(selectFolderFilePath);
            sw.Write(seletFolder);
            sw.Flush();
            sw.Close();
        }

        private string GetHeader()
        {
            var lines = File.ReadAllLines(headerFilePath);
            if (lines.Length > 0)
            {
                string value = "";
                foreach (var line in lines) value += line + "\n";
                value += "\n";
                return value;
            }
            else return "";
        }

        private static char Seperator()
        {
            char separator = '/';
#if UNITY_STANDALONE_OSX
            separator = '/';
#elif UNITY_STANDALONE_WIN
            separator = '\\';
#endif
            return separator;
        }
        /// <summary>
        /// 插件根目录
        /// </summary>
        /// <returns></returns>
        private static string RootFolder()
        {
            return string.Format("{1}{0}{2}{0}", Seperator(), Application.dataPath, "Script Creater");
        }

        private static string RootFolderEditor()
        {
            return string.Format("{1}{2}{0}", Seperator(), RootFolder(), "Editor");
        }

        private static string GetAuthorName()
        {
            return File.ReadAllLines(authorNameFilePath)[0];
        }

        private void OpenFile(string fileNameWithoutExtension)
        {
            var foundGenerated = AssetDatabase.FindAssets($"t:script {fileNameWithoutExtension}");
            if (foundGenerated.Length > 0)
            {
                int openCount = 0;
                foreach (var select in foundGenerated)
                {
                    var obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(AssetDatabase.GUIDToAssetPath(select));
                    if (obj != null && obj.name.Equals(fileNameWithoutExtension))
                    {
                        AssetDatabase.OpenAsset(obj, -1);
                        openCount++;
                    }
                }
                if (openCount > 1)
                {
                    Debug.Log($"打开多个{fileNameWithoutExtension}脚本");
                }
            }
        }

        private static string ShortenPath(string path, float width)
        {
            int len = (int)Mathf.Min(Mathf.RoundToInt(0.137f * width) + 3, width);
            var str = path;
            if (path.Length > len && len > 0)
            {
                var start = path.Length - len;
                var length = len;
                str = "..." + path.Substring(start, length);
            }

            return str;
        }
    }
}