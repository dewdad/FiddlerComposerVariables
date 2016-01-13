using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FiddlerComposerVariables
{
    public partial class frmLoad : Form
    {
        public frmLoad(string[] saves)
        {
            InitializeComponent();
            lbSaves.Items.AddRange(saves);
        }

        private void lbSaves_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnLoad.Enabled = lbSaves.SelectedIndex >= 0;
        }
    }
}
