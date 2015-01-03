using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrickyLib.CustomControl.PropertyGrid.FileBrowser
{
    public class FilterAttribute : Attribute
    {
        private string _filter;
        public string Filter
        {
            get
            {
                return _filter;
            }
            protected set
            {
                _filter = value;
            }
        }

        public FilterAttribute(string filter)
        {
            Filter = filter;
        }
    }
}
