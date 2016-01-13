using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fiddler;

namespace FiddlerComposerVariables
{
    public static class Utils
    {
        public static void Alert(string title, string msg)
        {
            FiddlerApplication.UI.ShowAlert(new frmAlert("Composer Variables - " + title, msg, ""));
        }

        public static void Show(Form form)
        {
            FiddlerApplication.UI.ShowDialog(form);
        }
    }
}
