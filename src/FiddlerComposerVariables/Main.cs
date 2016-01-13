using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CsvHelper;
using Fiddler;

namespace FiddlerComposerVariables
{
    public class Main : IAutoTamper
    {
        private TabPage page;
        private Tab tab;
        private VariablesList variablesList;
        private string currentSaveFilename;
        
        private const string defaultSaveName = "Default";
        public readonly string savesPath;

        public Main()
        {
            try
            {
                savesPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/ComposerVariables";
                if (!Directory.Exists(savesPath)) Directory.CreateDirectory(savesPath);
            }
            catch
            {
                Utils.Alert("Error", "Failed to initialize addon.");
            }
        }

        public void OnLoad()
        {
            try
            {
                page = new TabPage("Composer Variables");
                page.ImageIndex = (int) SessionIcons.Script;

                tab = new Tab();
                tab.ChangeVariablesList(variablesList = new VariablesList(savesPath, defaultSaveName));
                tab.Dock = DockStyle.Fill;
                tab.btnLoad.Click += BtnLoad_Click;
                tab.btnNew.Click += BtnNew_Click;
                tab.btnSave.Click += BtnSave_Click;
                page.Controls.Add(tab);

                FiddlerApplication.UI.tabsViews.TabPages.Add(page);
            }
            catch
            {
                Utils.Alert("Error", "Failed to initialize addon.");
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            variablesList.Save();
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            var form = new frmNew();
            form.btnCreate.Click += (ls, le) =>
            {
                if (string.IsNullOrEmpty(form.tbNewName.Text)) return;
                if (GetSaves().Contains(form.tbNewName.Text))
                {
                    Utils.Alert("Error", "List name is already taken.");
                    return;
                }
                variablesList.Save();
                tab.ChangeVariablesList(variablesList = new VariablesList(savesPath, form.tbNewName.Text));
                form.Close();
            };
            form.ShowDialog();
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            var form = new frmLoad(GetSaves().ToArray());
            form.btnLoad.Click += (ls, le) =>
            {
                if (form.lbSaves.SelectedIndex < 0) return;
                var saveName = form.lbSaves.SelectedItem as string;
                if (string.IsNullOrEmpty(saveName)) return;
                tab.ChangeVariablesList(variablesList = new VariablesList(savesPath, saveName));
                form.Close();
            };
            form.ShowDialog();
        }

        public void OnBeforeUnload()
        {
            if (variablesList != null) variablesList.Save();
        }

        public void AutoTamperRequestBefore(Session oSession)
        {
            if (variablesList == null) return;
            foreach (var variable in variablesList)
            {
                oSession.utilReplaceInRequest("{{" + variable.Token + "}}", variable.Value);
                oSession.url = oSession.url.Replace("{{" + variable.Token + "}}", variable.Value);
                foreach (var header in oSession.RequestHeaders)
                    header.Value = header.Value.Replace("{{" + variable.Token + "}}", variable.Value);
            }
        }

        public void AutoTamperRequestAfter(Session oSession)
        {
        }

        public void AutoTamperResponseAfter(Session oSession)
        {
        }

        public void AutoTamperResponseBefore(Session oSession)
        {
        }

        public void OnBeforeReturningError(Session oSession)
        {
        }

        private IEnumerable<string> GetSaves()
        {
            try
            {
                return Directory.GetFiles(savesPath, "*.csv").Select(o => Path.GetFileNameWithoutExtension(o));
            }
            catch
            {
                Utils.Alert("Error", "An error has occured looking for saved variables from \"" + savesPath + "\".");
                return new List<string>();
            }
        }
    }
}