//Author:	sora

namespace Sora.Tools.ScriptCreater
{
    public class ScriptCreaterEnums
    {
        public enum ScriptType
        {
            Class,
            Mono,
            ECSComponent,
            ECSSystem,
        }
        public enum ClassScriptType
        {
            Class,
            Interface,
            Enum,
            Struct,
        }

        public enum MonoScriptType
        {
            MonoBehaviour,
            Editor,
            EditorWindow,
            ScriptableObject,
        }

        public enum ECSSystemType
        {
            ComponentSystem,
            JobomponentSystem,
        }

        public enum ECSComponentType
        {
            ComponentData,
            SharedComponentData,
        }

    }
}