using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fiddler;

namespace FiddlerComposerVariables
{
    public partial class Tab : UserControl
    {
        public event EventHandler Save;

        public Tab()
        {
            InitializeComponent();
        }

        public void ChangeVariablesList(VariablesList variablesList)
        {
            bs.DataSource = variablesList;
            tbFilename.Text = variablesList.SaveName;
        }
    }
}
