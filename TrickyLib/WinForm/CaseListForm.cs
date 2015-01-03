using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection.Emit;
using System.Reflection;
using TrickyLib.Reflection;

namespace TrickyLib.WinForm
{
    public partial class CaseListForm : Form
    {
        public List<string> CasesPropertyNames { get; set; }
        public List<Type> CasesPropertyTypes { get; set; }
        public List<IEnumerable<object>> CasesProperties { get; set; }
        public List<string> AdditionalFiles { get; set; }

        public CaseListForm()
        {
            InitializeComponent();
            this.CasesDataGrid.MultiSelect = false;

            this.CasesPropertyNames = new List<string>();
            this.CasesPropertyTypes = new List<Type>();
            this.CasesProperties = new List<IEnumerable<object>>();
            this.AdditionalFiles = new List<string>();
        }

        public CaseListForm(List<string> casesPropertyNames, List<Type> casesPropertyTypes, List<IEnumerable<object>> casesProperties, List<string> additionalFiles)
        {
            InitializeComponent();
            this.CasesDataGrid.MultiSelect = false;

            this.CasesPropertyNames = casesPropertyNames;
            this.CasesPropertyTypes = casesPropertyTypes;
            this.CasesProperties = casesProperties;
            this.AdditionalFiles = additionalFiles;

            SetCasesDataGrid();
        }

        public void SetCasesDataGrid()
        {
            this.CasesDataGrid.DataSource = DynamicClass.CreateInstances(this.CasesPropertyNames, this.CasesPropertyTypes, this.CasesProperties);
        }

        private void CasesDataGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (this.CasesDataGrid.SelectedRows.Count > 0)
            {
                List<string> oneCasePropertyNames = null;
                List<Type> oneCasePropertyTypes = null;
                List<object[]> oneCaseProperties = null;

                GetSelectionCase(this.CasesDataGrid.SelectedRows[0].DataBoundItem, this.AdditionalFiles, out oneCasePropertyNames, out oneCasePropertyTypes, out oneCaseProperties);

                if (oneCasePropertyNames != null && oneCasePropertyTypes != null && oneCaseProperties != null)
                    this.OneCaseDataGrid.DataSource = DynamicClass.CreateInstances(oneCasePropertyNames, oneCasePropertyTypes, oneCaseProperties);
            }
        }

        protected virtual void GetSelectionCase(object selectedItem, IEnumerable<string> additionalFiles, out List<string> oneCasePropertyNames, out List<Type> oneCasePropertyTypes, out List<object[]> oneCaseProperties)
        {
            throw new NotImplementedException();
        }
    }
}
