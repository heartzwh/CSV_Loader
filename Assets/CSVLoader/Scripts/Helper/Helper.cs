//Author: sora

using UnityEngine;

namespace Sora.Tools.CSVLoader
{
    public static class Helper
    {
        public const char SPLIT = ',';
        public const char SETTING_SPLIT = '#';
        public static string GetUnityAssetPath(string path) => path.Substring(path.LastIndexOf("Assets"));
    }
}