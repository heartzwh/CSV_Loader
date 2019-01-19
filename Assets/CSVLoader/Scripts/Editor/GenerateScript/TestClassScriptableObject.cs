namespace A.B
{
    [System.Serializable]
    public class TestClassScriptableObject : UnityEngine.ScriptableObject
    {
        public System.Collections.Generic.List<TestClass> dataSet;

        public void SetData(System.Collections.Generic.List<object> dataSetSource)
        {
            dataSet = new System.Collections.Generic.List<TestClass>();
            foreach (var data in dataSetSource)
            {
                dataSet.Add(data as TestClass);
            }
        }

    }
}
