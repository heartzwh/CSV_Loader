//Author: sora

using System;

namespace Sora.Tools.CSVLoader
{
    public class BooleanProperty : BaseProperty
    {
        #region constructor

        #endregion


        #region event/delegate

        #endregion


        #region property

        #endregion


        #region public method
        public override void DrawProperty(float x, float y, ref float width, ref float height)
        {
        }

        #endregion


        #region protected method
        protected override Type GetPropertyType(string rowData) => typeof(bool);

        #endregion


        #region private method

        #endregion


        #region static

        #endregion

    }
}