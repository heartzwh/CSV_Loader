//Author: sora

namespace Sora.Tools.CSVLoader.Editor
{
    public interface IScriptAsset
    {
        /// <summary>
        /// 脚本内容
        /// </summary>
        /// <value></value>
        string scriptContent { get; }
        /// <summary>
        /// 初始化ScriptObject脚本资源
        /// </summary>
        /// <param name="generateData">生成信息</param>
        void InitScriptAsset(GenerateData generateData);
        /// <summary>
        /// 添加数据到Asset中
        /// </summary>
        void InitData();
    }
}