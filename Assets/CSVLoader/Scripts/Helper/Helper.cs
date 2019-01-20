//Author: sora

using UnityEngine;

namespace Sora.Tools.CSVLoader
{
    public static class Helper
    {
        public const char SPLIT = ',';
        public const char SETTING_SPLIT = '#';
        private static string dataPath = "";
        public static string GetUnityAssetPath(string path) => path.Substring(path.LastIndexOf("Assets"));
        public static string DataPathWithoutAssets()
        {
            if (!string.IsNullOrEmpty(dataPath)) return dataPath;
            var lastIndex = Application.dataPath.LastIndexOf("Assets");
            dataPath = Application.dataPath.Substring(0, lastIndex - 1);
            return dataPath;
        }
    }
}