using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TrickyLib.Extension;

namespace TrickyLib.CustomControl.PropertyGrid.FileBrowser
{
    [Serializable]
    public class CustomFileNameEditor : UITypeEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            bool multiSelect = false;
            string filter = "";
            string initialDir = "";
            
            var filterAttribute = context.PropertyDescriptor.Attributes[typeof(FilterAttribute)];
            var multiSelectAttribute = context.PropertyDescriptor.Attributes[typeof(MultiSelectAttribute)];

            if (filterAttribute != null) filter = (filterAttribute as FilterAttribute).Filter;
            if (multiSelectAttribute != null) multiSelect = (multiSelectAttribute as MultiSelectAttribute).MultiSelect;
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control && value != null && !string.IsNullOrEmpty(value.ToString())) initialDir = Path.GetDirectoryName(value.ToString().Split(';')[0]);

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = multiSelect;
            if (!string.IsNullOrEmpty(filter)) ofd.Filter = filter;
            if (Directory.Exists(initialDir) && !string.IsNullOrEmpty(initialDir)) ofd.InitialDirectory = initialDir;

            if (ofd.ShowDialog() == DialogResult.OK)
                return ofd.FileNames.ConnectWords(";");
            else
                return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
}
