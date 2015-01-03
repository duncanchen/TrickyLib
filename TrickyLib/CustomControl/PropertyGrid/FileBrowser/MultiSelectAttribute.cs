using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrickyLib.CustomControl.PropertyGrid.FileBrowser
{
    public class MultiSelectAttribute : Attribute
    {
        private bool _multiSelect;
        public bool MultiSelect
        {
            get
            {
                return _multiSelect;
            }
            protected set
            {
                _multiSelect = value;
            }
        }

        public MultiSelectAttribute(bool multiSelect)
        {
            MultiSelect = multiSelect;
        }
    }
}
