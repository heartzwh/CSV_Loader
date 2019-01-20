namespace Sora.Tools.CSVLoader
{
    [System.Serializable]
    public class TestBaseArrayScriptableObject : UnityEngine.ScriptableObject, Sora.Tools.CSVLoader.ICSVLoaderAsset
    {
        public System.Collections.Generic.List<TestBaseArray> dataSet;

        public void SetData(System.Collections.Generic.List<object> dataSetSource)
        {
            dataSet = new System.Collections.Generic.List<TestBaseArray>();
            foreach (var data in dataSetSource)
            {
                dataSet.Add(data as TestBaseArray);
            }
        }

    }
}
