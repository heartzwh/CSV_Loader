namespace Sora.Tools.CSVLoader
{
    [System.Serializable]
    public class TestBaseSource : UnityEngine.ScriptableObject, Sora.Tools.CSVLoader.ICSVLoaderAsset
    {
        public System.Collections.Generic.List<TestBase> dataSet;

        public void SetData(System.Collections.Generic.List<object> dataSetSource)
        {
            dataSet = new System.Collections.Generic.List<TestBase>();
            foreach (var data in dataSetSource)
            {
                dataSet.Add(data as TestBase);
            }
        }

    }
}
