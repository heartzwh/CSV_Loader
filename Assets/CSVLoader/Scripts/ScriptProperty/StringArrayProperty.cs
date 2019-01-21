//Author: sora

using System;

namespace Sora.Tools.CSVLoader
{
    [Serializable]
	public class StringArrayProperty : BaseArrayProperty<string[]>
	{
        public override void SetPropertyValue(RawData value)
        {
            if (propertyValue == null) propertyValue = new string[value.width];
            for (var index = 0; index < value.width; index++)
            {
                propertyValue[index] = value[index, 0];
            }
        }
	}
}